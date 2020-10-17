using System.Threading.Tasks;
using Uso.Core.MIDI;
using Uso.Core.Timing;

namespace Uso.Core
{
    /// <summary>
    /// Represents a play of a single game
    /// </summary>
    class Game
    {
        TimeSource timeManager;


        public static async Task<Game> NewGame(Song.Song s, MidiManager midiManager,  TimeSourceFactory tf)
        {
            TimeSource timeManager = tf.NewTimeSource(s.PPQ, s.InitialTempo);
            var device = await midiManager.CreateOutput();
            foreach (MIDIOutputEvent e in s.Accomp)
            {
                timeManager.Schedule(e.Tick, () =>
                {
                    device.SendMessage(e);
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
