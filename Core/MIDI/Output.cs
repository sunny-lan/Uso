

namespace Uso.Core.MIDI
{
    public interface Output
    {
        void SendMessage(NoteEvent evt);
    }
}