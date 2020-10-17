using Priority_Queue;
using System;

namespace Uso.Core.Timing
{
    class SimpleTimeSource : TimeSource
    {
        public SimpleTimeSource(long ppq, long tempo)
        {
            PPQ = ppq;
            Tempo = tempo;
            Time = 0;
        }

        public bool Playing { get; private set; } = false;

        public long Tempo { get; set; }

        public double Time { get; private set; }

        public long PPQ { get; private set; }

        public void Pause()
        {
            if (!Playing)
                throw new InvalidOperationException("Already paused");
            Playing = false;
        }

        public void Play()
        {
            if (Playing)
                throw new InvalidOperationException("Already playing");
            Playing = true;
        }

        SimplePriorityQueue<Scheduled, long> tasks=new SimplePriorityQueue<Scheduled, long>();

        public void Schedule(long time, Scheduled scheduled)
        {
            tasks.Enqueue(scheduled, time);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsed">time since update was last called, in microseconds</param>
        public void Update(double elapsed)
        {
            if (Playing)
            {
                Time += elapsed / Tempo * PPQ;
                while (tasks.Count > 0)
                {
                    Scheduled nxt = tasks.First;
                    if (tasks.GetPriority(nxt) > Time) break;
                    nxt();
                    tasks.Remove(nxt);
                }
            }
        }
    }
}
