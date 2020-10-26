
using System;
using Windows.Devices.Display.Core;

namespace Uso.Core.Song
{
    public abstract class Event
    {
        /// <summary>
        /// Time that the event starts, in PPQ
        /// </summary>
        public long Time;

    }

    /// <summary>
    /// This will likely change to allow more output events
    /// </summary>
    class OutputEvent :Event{
        public MIDI.Event Output;

    }

    public abstract class NoteEvent:Event
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
