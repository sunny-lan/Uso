using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core;
using Uso.Core.Functions;
using Uso.Core.Timing;

namespace Uso.Mono.Components
{
    class ComboCounter
    {
        private readonly SpriteFont font;
        private readonly TimeSource t;
        private readonly Bloop counterBloop, overlayBloop;

        /// <summary>
        /// Time division to update the counter on, in PPQ. Defaults to 16th notes
        /// </summary>
        public long BloopDivision;

        /// <summary>
        /// Measured in times original size
        /// </summary>
        public double OverlayBloopSize = 1.5, CounterBloopSize = 0.8;

        /// <summary>
        /// Length of the bloop. Defaults to 64th and 16th notes
        /// </summary>
        public double CounterBloopBegin, CounterBloopEnd;

        /// <summary>
        /// Alpha out of 256
        /// </summary>
        public double OverlayOpacityBegin = 200;

        public ComboCounter(SpriteFont font, TimeSource t)
        {
            this.font = font;
            this.t = t;
            counterBloop = new Bloop(t);
            overlayBloop = new Bloop(t);

            BloopDivision = t.PPQ / 4;
            CounterBloopBegin = t.PPQ / 16;
            CounterBloopEnd = t.PPQ / 4;

        }

        string oldText = "0x";
        string newText = "0x";
        Color overlayColor = Color.White;

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            double counterSize = MathUtil.Lerp(counterBloop.Value, 1, CounterBloopSize);
            //draw counter
            sb.DrawString(font, newText, position,
                Color.White, 0, Vector2.Zero,
                (float)counterSize,
                SpriteEffects.None,
                0
            );

            //draw overlay
            var v2 = overlayBloop.Value;
            if (v2 > 0)
            {
                overlayColor.A = (byte)MathUtil.Lerp(v2, 0, OverlayOpacityBegin);
                sb.DrawString(font, newText, position,
                    overlayColor, 0, Vector2.Zero,
                    (float)MathUtil.Lerp(v2, counterSize, OverlayBloopSize),
                    SpriteEffects.None,
                    0
                );
            }
        }

        long nextBloop = -1;

        public void Increment(int newCount)
        {
            overlayColor.A = (byte)OverlayOpacityBegin;
            newText = newCount + "x";
            long delayedBloop = (long)Math.Round(t.Time / BloopDivision + 1) * BloopDivision;
            overlayBloop.Do2(t.Time, delayedBloop);

            //bloop on the next beet
            if (nextBloop != delayedBloop)
            {
                nextBloop = delayedBloop;
                t.Schedule(nextBloop, () =>
                {
                    oldText = newText;
                    counterBloop.Do(CounterBloopBegin, CounterBloopEnd);
                });
            }
        }
    }
}
