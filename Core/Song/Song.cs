using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core.MIDI;

namespace Uso.Core.Song
{
    interface Song
    {
        /// <summary>
        /// The accomp notes
        /// </summary>
        List<NoteEvent> Accomp { get; }

        long PPQ { get; }

        /// <summary>
        /// initial tempo, in microseconds per beat
        /// </summary>
        long InitialTempo { get; }
    }
}
