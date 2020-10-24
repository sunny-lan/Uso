using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core.MIDI;

namespace Uso.Core.Song
{
    class Song
    {
        /// <summary>
        /// events which will be used to judge user input
        /// the generic type may be changed to allow more types later
        /// </summary>
        public List<NoteEvent> JudgedEvents;

        /// <summary>
        /// events which will be shown on a staff
        /// this may change later to be split to multiple staffs
        /// </summary>
        public List<NoteEvent> DisplayEvents;

        public List<TempoChangeEvent> TempoChanges;
        public List<Event> OtherEvents; 

        /// <summary>
        /// Ticks per quarter note (constant value per song)
        /// </summary>
        public long PPQ;

        /// <summary>
        /// initial tempo, in microseconds per quarter note
        /// </summary>
        public long InitialTempo;

        public TimeSignature InitialSignature;

        public Song(List<NoteEvent> judgedEvents, List<NoteEvent> displayEvents, List<Event> otherEvents, long ppq, long initialTempo, TimeSignature initialSignature)
        {
            JudgedEvents = judgedEvents;
            DisplayEvents = displayEvents;
            OtherEvents = otherEvents;
            JudgedEvents.Sort();
            DisplayEvents.Sort();
            OtherEvents.Sort();
            PPQ = ppq;
            InitialTempo = initialTempo;
            InitialSignature = initialSignature;

            TempoChanges = new List<TempoChangeEvent>();
            TempoChanges.Add(new TempoChangeEvent
            {
                NewTempo = initialTempo,
            });
            foreach(var e in OtherEvents)
            {
                if(e is TempoChangeEvent t)
                {
                    TempoChanges.Add(t);
                }
            }
        }

        public long GetTempoAt(long time)
        {
            return TempoChanges[TempoChanges.GetFirstIdx(time)].NewTempo;
        }
    }

    static class SongExtensions
    {
        /// <summary>
        /// Returns the index of the first event after time
        /// </summary>
        /// <param name="s">The list to search</param>
        /// <param name="time">The time, in PPQ</param>
        /// <returns></returns>
        public static int GetFirstIdx<T>(this List<T> s, double time) where T:Event
        {
            int lo = 0, hi = s.Count;
            while (lo != hi)
            {
                int mid = (lo + hi) / 2;
                if (s[mid].Time < time)
                    lo = mid + 1;
                else
                    hi = mid;
            }
            return lo;
        }
    }
}
