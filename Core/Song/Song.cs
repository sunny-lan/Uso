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

    static class SongExtensions
    {
        /// <summary>
        /// Returns the index of the first event after time
        /// </summary>
        /// <param name="s">The list to search</param>
        /// <param name="time">The time, in PPQ</param>
        /// <returns></returns>
        public static int GetFirstIdx(this List<Event> s, long time)
        {
            int lo = 0, hi = s.Count;
            while (lo != hi)
            {
                int mid = (lo + hi) / 2;
                if (s[lo].Time < time)
                    lo = mid + 1;
                else
                    hi = mid;
            }
            return lo;
        }
    }
}
