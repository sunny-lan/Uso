using System.Collections.Generic;

namespace Uso.Core.Song
{
    class TimeSignature
    {
        /// <summary>
        /// Length of divisions, in PPQ
        /// </summary>
        long Division;

        /// <summary>
        /// Length of bar, in number of divisions
        /// </summary>
        long BarLength;
    }


    class Staff
    {
        List<NoteEvent> Events;
        TimeSignature InitialSignature;
    }
}
