using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core.MIDI;

namespace Uso.Core.Song
{
    interface Song
    {
        List<Event> Events { get; }

        long PPQ { get; }

        /// <summary>
        /// initial tempo, in microseconds per quarter note
        /// </summary>
        long InitialTempo { get; }
    }
}
