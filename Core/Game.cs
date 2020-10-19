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


        public static async Task<Game> NewGame(Song.Song s, MIDI.Manager midiManager,  TimeSourceFactory tf)
        {
            TimeSource timeManager = tf.NewTimeSource(s.PPQ, s.InitialTempo);
            var output = await midiManager.CreateOutput();
            foreach (Song.Event e in s.Events)
            {
                timeManager.Schedule(e.Time, () =>
                {
                   
                    if (e.Accomp)
                    {
                        if(e is Song.NoteEvent e1)
                        {
                            output.SendMessage(Song.MIDIAdapter.Convert(e1));
                        }
                        else
                        {
                            throw new ArgumentException("Bad accomp event. Must be a NoteEvent");
                        }
                    }

                    if(e is Song.TempoChangeEvent t1)
                    {
                        timeManager.Tempo = t1.NewTempo;
                    }
                });
            }
            return new Game
            {
                timeManager = timeManager
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
