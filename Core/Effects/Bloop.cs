using Uso.Core.Timing;

namespace Uso.Core.Effects
{
    class Bloop
    {
        private readonly TimeSource t;

        public Bloop(TimeSource t)
        {
            this.t = t;
        }


        private double start, middle, end;

        /// <summary>
        /// Bloop starting at now, peaking after beginLen, and ending after endLen
        /// TODO: blooping before another bloop prevents the other bloop from ever reaching max bloop
        /// </summary>
        public void Do(double beginLen, double endLen )
        {
            start = t.Time;
            middle = start + beginLen;
            end = middle + endLen;
        }


        /// <summary>
        /// Bloops, except controls the exact time the bloop occurs
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="middle"></param>
        /// <param name="end"></param>
        public void Do2(double middle, double end)
        {
            start = t.Time;
            this.middle = middle;
            this.end = end;
        }



        public double Value
        {
            get
            {
                if (t.Time <= middle)
                {
                    return MathUtil.Unlerp(t.Time, start, middle);
                }
                else if (t.Time <= end)
                {
                    return MathUtil.Unlerp(t.Time, end, middle);
                }
                else
                {
                    return 0;
                }
            }
        }


    }
}
