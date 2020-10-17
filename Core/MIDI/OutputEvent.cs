namespace Uso.Core.MIDI
{

    
    public interface NoteEvent
    {
        long Time { get; }
        byte Note { get; }
        byte Velocity { get; }
    }

    public interface NoteOnEvent:NoteEvent { }
    public interface NoteOffEvent : NoteEvent { }
}
