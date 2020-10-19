using System.Threading.Tasks;
using Uso.Core.Timing;
using System;

namespace Uso.Core
{
    /// <summary>
    /// Represents a play of a single game
    /// </summary>
    class Game
    {
        TimeSource timeManager;


        public static async Task<Game> NewGame(Song.Song s, MIDI.Manager midiManager,  TimeSource ts)
        {
            var output = await midiManager.CreateOutput();
            ts.Tempo = s.InitialTempo;
            foreach (Song.Event e in s.OtherEvents) {

                if (e is Song.TempoChangeEvent t1)
                {
                    ts.Schedule(e.Time, ()=>ts.Tempo = t1.NewTempo);
                }

                
                if (e is Song.OutputEvent o)
                {
                    ts.Schedule(e.Time, () => output.SendMessage(o.Output));
                }
            }
            return new Game
            {
                timeManager = ts
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

    }
}
