
using System;

namespace Uso.Core.Song
{
    abstract class Event:IComparable<Event>
    {
        /// <summary>
        /// Time that the event starts, in PPQ
        /// </summary>
        public long Time;

        public int CompareTo(Event other)
        {
            return Time.CompareTo(other.Time);
        }
    }

    /// <summary>
    /// This will likely change to allow more output events
    /// </summary>
    class OutputEvent :Event{
        public MIDI.NoteOutput Output;
    }

    abstract class NoteEvent:Event
    {
        public int Velocity;
        public int Note;
    }

    class NoteOnEvent : NoteEvent {
        /// <summary>
        /// The note off event which corresponds to this note on
        /// </summary>
        public NoteOffEvent Match;
    } 

    class NoteOffEvent : NoteEvent {
        /// <summary>
        /// The note on which corresponds to this note off
        /// </summary>
        public NoteOnEvent Match;
    }

    class TempoChangeEvent:Event
    {
        /// <summary>
        /// New tempo, in Microseconds per quarternote
        /// </summary>
        public long NewTempo;
    }
}
