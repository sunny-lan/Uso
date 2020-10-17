

namespace Uso.Core.MIDI
{
    public interface MidiOutput
    {
        void SendMessage(MIDIOutputEvent evt);
    }
}