using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using Uso.Core.Song;

namespace Uso.Mono
{
    interface MusicView
    {
        double StartTime { get; }
        double StopTime { get; }
    }

    class Theme
    {
        public Texture2D Lane;
        public Texture2D Note;
        public SpriteFont TestFont;
    }

    static class ThemeLoader
    {
        public static void LoadFromContent(this Theme t,ContentManager mgr)
        {
            t.Lane = mgr.Load<Texture2D>("Images/lane");
            t.Note = mgr.Load<Texture2D>("Images/note");
        }

        public static void LoadBasic(this Theme t, ContentManager mgr)
        {
            t.TestFont = mgr.Load<SpriteFont>("Fonts/test");
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

            //default
            
        }

        private void DrawNote(SpriteBatch sb, Rectangle area, NoteOnEvent n1, NoteOffEvent n2)
        {
            double conversion = area.Width / (vw.StopTime - vw.StartTime);

            sb.Draw(ui.Lane, new Rectangle
            {
                X = n1.Note * 10 + area.Y ,
                Y = (int)(area.X + (n1.Time - vw.StartTime) * conversion),
                Height = (int)((n2.Time - n1.Time) * conversion),
                Width = 10,
            }, Color.White);
        }

        /// <summary>
        /// Render the staff within given rectangle.
        /// Ignores height of rectangle
        /// Doesn't Calls begin and end on sprite batches
        /// </summary>
        public void Draw(SpriteBatch sb, Rectangle area)
        {
            
            for (int idx = s.DisplayEvents.GetFirstIdx(vw.StartTime); 
                idx < s.DisplayEvents.Count;idx++)
            {
                var evt = s.DisplayEvents[idx];
                if (evt.Time > vw.StopTime) goto outer;
                switch (evt)
                {
                    case NoteOffEvent n2:
                        NoteOnEvent n1 = n2.Match;
                        DrawNote(sb, area, n1, n2);
                        break;
                    case NoteOnEvent n3:
                        NoteOffEvent n4 = n3.Match;//TODO performance
                        DrawNote(sb, area, n3, n4);
                        break;
                    default:
                        throw new ArgumentException("Cannot display event");
                }
            }
        outer:;

        }
    }
}
