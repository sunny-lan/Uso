namespace Uso.Core.MIDI
{

    
    public abstract class NoteOutput
    {
        public byte Note;
        public byte Velocity;
    }

    public class NoteOnOutput:NoteOutput { }
    public class NoteOffOutput : NoteOutput { }
}
