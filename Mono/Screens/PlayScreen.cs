using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core.Judgement;
using Uso.Core.MIDI;
using Uso.Core.Song;
using Uso.Core.Timing;
using Uso.Mono.Components;
using Uso.Mono.Customization;
using Uso.UWP;

namespace Uso.Mono.Screens
{
    class PlayScreen : Screen, Uso.Core.Game.Display
    {
        private readonly ScreenManager mgr;
        private readonly Theme theme;
        private readonly SongLoader previous;
        private readonly SimpleTimeSource ts;
        private readonly StaffRenderer sR;
        private readonly Core.Game g;
        private readonly KeyboardMIDIInput inp;
        private readonly ComboCounter ctr;

        private class MV : MusicView
        {
            public TimeSource ts;
            public long interval;
            public double StartTime => ts.Time;

            public double StopTime => ts.Time + interval;
        }

        /// <summary>
        /// Subject to change
        /// </summary>
        public class SongSettings
        {
            public Core.MIDI.Listener MidiOut;

            public SongSettings(Listener midiOut)
            {
                MidiOut = midiOut;
            }
        }
        public PlayScreen(ScreenManager mgr, Theme theme, SongLoader previous, Song s, SongSettings songSettings)
        {
            this.mgr = mgr;
            this.theme = theme;
            this.previous = previous;


            ts = new SimpleTimeSource(s.PPQ, s.InitialTempo);
            //var stf = new StaffRenderer(theme, , s);
            sR = new StaffRenderer(theme, new MV
            {
                ts = ts,
                interval = s.PPQ * 4,
            }, s);


            g = new Core.Game(s, songSettings.MidiOut, ts, this);
            inp = new Mono.KeyboardMIDIInput(g);

            ctr = new ComboCounter(theme.TestFont, ts);

            g.Play();
        }

        private long fsTime = -1;


        public void Update(GameTime gameTime)
        {
            var st = Keyboard.GetState();
            long now = FileSystemTime.Now1;
            if (fsTime != -1)
            {
                //limit frame to 100ms
                ts.Update(Math.Min(100 * 1000, (now - fsTime) / 10.0));
            }
            fsTime = now;

            inp.Update(st);


            if (st.IsKeyDown(Keys.Escape))
            {
                if (g.Playing) g.Pause();
                else g.Play();
            }else if (st.IsKeyDown(Keys.OemTilde))
            {
                //TODO think more about this screen switching logic
                mgr.Switch(previous);
            }
        }



        public void Draw(GameLayers output, Rectangle area)
        {
            var pos = area.Location.ToVector2();
            var sb = output.MainLayer;
            sR.Draw(sb,area);
            sb.DrawString(theme.TestFont, "" + Math.Round(ts.Time / ts.PPQ / 4), pos, Color.White);


            ctr.Draw(sb,pos+new Vector2
            {
                X = 100,
                Y = 100,
            });

        }

        public void ComboBroken(int newCombo, StandardJudgement reason)
        {
        }

        public void ComboUp(int newCombo, StandardJudgement reason)
        {
            ctr.Increment(newCombo);
        }

        public void JudgmentPassed(StandardJudgement judgement)
        {
            sR.JudgmentPassed(judgement);
        }

        public void ScoreChanged(StandardScore score, StandardJudgement reason)
        {
        }
    }
}
