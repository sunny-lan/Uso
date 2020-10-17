using System;
using System.Collections.Generic;
using System.Text;

namespace Uso.Core.Timing
{
    public interface TimeSourceFactory
    {
        TimeSource NewTimeSource(long PPQ, long Tempo);
  
    }

    public interface TimeSource
    {
        /// <summary>
        /// tick speed, in parts per quarter note
        /// </summary>
        long PPQ { get; }
        
        /// <summary>
        /// Time, in PPQ
        /// </summary>
        double Time { get; }

        bool Playing { get; }

        /// <summary>
        /// Tempo, in microseconds per beat
        /// </summary>
        long Tempo { get; set; }

        void Pause();
        void Play();


        /// <summary>
        /// Calls scheduled at time
        /// </summary>
        /// <param name="time">Time, in PPQ</param>
        /// <param name="scheduled">Function to call</param>
        void Schedule(long time, Scheduled scheduled);
    }
    public delegate void Scheduled();
}
