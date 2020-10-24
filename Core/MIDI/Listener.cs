

namespace Uso.Core.MIDI
{


    public interface Listener
    {
        void SendMessage(NoteEvent evt);
    }
}