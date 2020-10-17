using System;
using System.Collections.Generic;
using System.Text;

namespace Uso.Core.Song
{
    class SongEvent
    {
        /// <summary>
        /// Time that the event starts, in PPQ
        /// </summary>
        public long Time { get; set; }

        public bool Judge { get; set; }
        public bool Display { get; set; }
        public bool Accomp { get; set; }
    }


    class NoteEvent:SongEvent
    {
        public int Velocity { get; set; }
        public int Note { get; set; }

        /// <summary>
        /// Duration of note, in PPQ
        /// </summary>
        public long Duration { get; set; }
    }

    class TempoChangeEvent:SongEvent
    {
        /// <summary>
        /// New tempo, in Microseconds per beat
        /// </summary>
        public long NewTempo { get; set; }

        
    }
}
