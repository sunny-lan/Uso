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

        public Game(Song.Song s,

            MIDI.Listener output,
            TimeSource ts,
            Display display
            )
        {
            timeManager = ts;
            this.display = display;
            this.output = output;


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

            judger = new Judgement.StandardJudger(s.JudgedEvents, ts, display);

        }


        public interface Display : Judgement.StandardJudgementListener
        {

        }


        public bool Playing { get => timeManager.Playing; }

        public void Play()
        {
            timeManager.Play();
        }

        public void Pause()
        {
            timeManager.Pause();
        }

        //TODO sketch
        public void SendMessage(MIDI.Event evt)
        {
            if (Playing)
            {
                if (evt is MIDI.NoteEvent ne)
                    judger.OnInput(ne);
                evt.Channel = 15;
                output.SendMessage(evt);
            }
        }
    }
}
