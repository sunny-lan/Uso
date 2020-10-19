
namespace Uso.Core.Song
{
    class Event
    {
        /// <summary>
        /// Time that the event starts, in PPQ
        /// </summary>
        public long Time { get; set; }

        public virtual bool Judge { get; set; }
        public virtual bool Display { get; set; }
        public virtual bool Accomp { get; set; }
    }


    class NoteEvent:Event
    {
        public int Velocity { get; set; }
        public int Note { get; set; }
    }

    class NoteOnEvent : NoteEvent {
        public NoteOffEvent Match;
    } 

    class NoteOffEvent : NoteEvent {
        public NoteOnEvent Match;
    }

    class TempoChangeEvent:Event
    {
        /// <summary>
        /// New tempo, in Microseconds per quarternote
        /// </summary>
        public long NewTempo { get; set; }

        public override bool Judge { get => false; }
    }
}
