namespace Uso.Core.MIDI
{

    
    public abstract class NoteEvent
    {
        public byte Note;
        public byte Velocity;
    }

    public class NoteOnEvent:NoteEvent { }
    public class NoteOffEvent : NoteEvent { }
}
