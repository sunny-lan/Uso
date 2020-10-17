using System.Threading.Tasks;

namespace Uso.Core.MIDI
{
    interface MidiManager
    {
        Task<MidiOutput> CreateOutput();
    }
}