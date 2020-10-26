

namespace Uso.Core.MIDI
{


    public interface Listener
    {
        void SendMessage(MIDI.Event evt);
    }
}