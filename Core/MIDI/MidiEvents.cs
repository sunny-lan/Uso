namespace Uso.Core.MIDI
{
    public abstract class Event
    {
        public byte Channel;

    }
    
    public abstract class NoteEvent:Event
    {
        public byte Note;
        public byte Velocity;

    }

    public class NoteOnEvent:NoteEvent { }
    public class NoteOffEvent : NoteEvent { }

    public class ControlEvent :Event{
        public byte ControlValue;
        public byte Controller;
    }
}
