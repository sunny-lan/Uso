using System.Threading.Tasks;
using Uso.Core.Timing;
using System;

namespace Uso.Core
{
    /// <summary>
    /// Represents a play of a single game
    /// </summary>
    class Game : MIDI.Listener
    {
        private TimeSource timeManager;
        private Judgement.StandardJudger judger;
        private Display display;
        private MIDI.Listener output;

        public interface Display : Judgement.StandardJudgementListener
        {

        }

        public static async Task<Game> NewGame(
            Song.Song s,
            MIDI.Manager midiManager,
            TimeSource ts,
            Display display
        )
        {

            var output = await midiManager.CreateOutput();
            ts.Tempo = s.InitialTempo;
            foreach (Song.Event e in s.OtherEvents)
            {

                if (e is Song.TempoChangeEvent t1)
                {
                    ts.Schedule(e.Time, () => ts.Tempo = t1.NewTempo);
                }


                if (e is Song.OutputEvent o)
                {
                    ts.Schedule(e.Time, () => output.SendMessage(o.Output));
                }
            }
            return new Game
            {
                timeManager = ts,

                judger = new Judgement.StandardJudger(s.JudgedEvents, ts, display),

                display = display,

                output=output,
            };
        }
        private Game() { }

        public bool Playing { get => timeManager.Playing; }

        public void Play()
        {
            timeManager.Play();
        }

        public void Pause()
        {
            timeManager.Pause();
        }

        public void SendMessage(MIDI.NoteEvent evt)
        {
            if (Playing)
            {
                judger.OnInput(evt);
                output.SendMessage(evt);
            }
        }
    }
}
