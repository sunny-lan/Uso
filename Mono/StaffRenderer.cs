using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Uso.Core.Judgement;
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
        public static void LoadFromContent(this Theme t, ContentManager mgr)
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
        private readonly Theme ui;
        private readonly MusicView vw;
        private readonly Song s;


        /// <summary>
        /// Maps from MIDI Note number to lane
        /// </summary>
        private readonly Dictionary<int, Lane> lanes = new Dictionary<int, Lane>();
        class Lane
        {
            public int Position;
            public bool Lit;
        }

        public StaffRenderer(Theme ui, MusicView vw, Song s)
        {
            this.ui = ui;
            this.vw = vw;
            this.s = s;

            //default
            for (int i = 0; i < 120; i++)
            {
                lanes.Add(i, new Lane
                {
                    Position = i,
                    Lit = false,
                });
            }

        }

        private Color Find(long time)
        {
            if (time % s.PPQ == 0) return Color.Red;
            if (time % (s.PPQ / 2) == 0) return Color.Blue;
            if (time % (s.PPQ / 4) == 0) return Color.Yellow;
            return Color.White;
        }

        private void DrawNote(SpriteBatch sb, Rectangle area, NoteOnEvent n1, NoteOffEvent n2)
        {
            double conversion = area.Width / (vw.StopTime - vw.StartTime);

            Lane l = lanes[n1.Note];

            sb.Draw(ui.Lane, new Rectangle
            {
                X = l.Position * 10 + area.X,
                Y = area.Height - (int)(area.X + (n2.Time - vw.StartTime) * conversion),
                Height = (int)((n2.Time - n1.Time) * conversion),
                Width = 10,
            }, Find(n1.Time));
        }


        /// <summary>
        /// Render the staff within given rectangle.
        /// Ignores height of rectangle
        /// Doesn't Calls begin and end on sprite batches
        /// </summary>
        public void Draw(SpriteBatch sb, Rectangle area)
        {
            foreach (var lane in lanes.Values)
            {
                if (lane.Lit)
                {
                    sb.Draw(ui.Lane, new Rectangle
                    {
                        X = lane.Position * 10 + area.X,
                        Y = 0,
                        Height = area.Height,
                        Width = 10,
                    }, Color.White);
                }
            }

            for (int idx = s.DisplayEvents.GetFirstIdx(vw.StartTime);
                idx < s.DisplayEvents.Count; idx++)
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
                        NoteOffEvent n4 = n3.Match;//TODO make note only draw once
                        DrawNote(sb, area, n3, n4);
                        break;
                    default:
                        throw new ArgumentException("Cannot display event");
                }
            }
        outer:;

        }

        public void JudgmentPassed(StandardJudgement j)
        {
            switch (j.Input)
            {
                case Core.MIDI.NoteOnEvent on:
                    lanes[on.Note].Lit = true;
                    break;
                case Core.MIDI.NoteOffEvent off:
                    lanes[off.Note].Lit = false;
                    break;
                default:
                    throw new NotImplementedException();
            }

            
        }

    }
}
