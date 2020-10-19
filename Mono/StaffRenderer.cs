using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core.Song;
using Uso.Core.Timing;

namespace Uso.Mono
{
    interface MusicView
    {
        long StartTime { get; }
        long StopTime { get; }
    }

    class Theme
    {
        public Texture2D Lane;
        public Texture2D Note;
    }

    static class ThemeLoader
    {
        public static Theme LoadFromContent(ContentManager mgr)
        {
            return new Theme
            {
                Lane = mgr.Load<Texture2D>("Images/Lane"),
                Note = mgr.Load<Texture2D>("Images/Note"),
            };
        }
    }

    /// <summary>
    /// Renders a single staff of music
    /// </summary>
    class StaffRenderer

    {
        private Theme ui;
        private MusicView vw;
        private Song s;

        /// <summary>
        /// Maps from MIDI Note number to y position of line
        /// </summary>
        private Dictionary<int, int> lanePositions = new Dictionary<int, int>();

        public StaffRenderer(Theme ui, MusicView vw, Song s)
        {
            this.ui = ui;
            this.vw = vw;
            this.s = s;
        }

        /// <summary>
        /// Render the staff within given rectangle.
        /// Ignores height of rectangle
        /// Doesn't Calls begin and end on sprite batches
        /// </summary>
        public void Draw(SpriteBatch sb, Rectangle area)
        {
            float conversion = area.Width / (vw.StopTime - vw.StartTime);
            int idx = s.Events.GetFirstIdx(vw.StartTime);
            while (s.Events[idx].Time < vw.StopTime)
            {
                var evt = s.Events[idx];
                if (evt.Display)
                {
                    switch (evt)
                    {
                        case NoteOnEvent n1:
                            sb.Draw(ui.Note, new Vector2
                            {
                                X = area.X + (n1.Time - vw.StartTime) * conversion,
                                Y = lanePositions[n1.Note] + area.Y,
                            }, Color.White);
                            break;
                        default:
                            throw new ArgumentException("Cannot display event");
                    }
                }
            }

        }
    }
}
