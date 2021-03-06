﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uso.Core.Judgement;
using Uso.Core.MIDI.Parser;
using Uso.Core.Song;
using Uso.Core.Timing;
using Uso.Mono;
using Uso.Mono.Components;
using Uso.Mono.Customization;

namespace Uso.UWP

{
    public class TestGame : Game,Core.Game.Display
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch sb;

        private Core.MIDI.Manager midiManager;
        private Theme theme;
        private SimpleTimeSource ts;
        private StaffRenderer sR;
        private bool loading = true;
        private Mono.KeyboardMIDIInput inp;

        private Core.Game g;
        private ComboCounter ctr;

        public TestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            midiManager = new UWPMidiManager();

            base.Initialize();
        }

        private class MV : MusicView
        {
            public TimeSource ts;
            public long interval;
            public double StartTime => ts.Time;

            public double StopTime => ts.Time + interval;
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);
            theme = new Theme();
            theme.LoadBasic(Content);

            var loadMidi = Task.Run(() => new MidiFile("Assets/desire_drive.mid"));
            var loadTheme = Task.Run(() => theme.LoadFromContent(Content));
            var output = midiManager.CreateOutput();

            Task.Run(async () =>
            {
                // TODO: use this.Content to load your game content here
                var s = MidiToSong.FromMidi(await loadMidi);


                ts = new SimpleTimeSource(s.PPQ, s.InitialTempo);
                //var stf = new StaffRenderer(theme, , s);
                await loadTheme;
                sR = new StaffRenderer(theme, new MV
                {
                    ts = ts,
                    interval = s.PPQ * 4,
                }, s);

                var mout = await output;

                g = new Core.Game(s, mout, ts, this);
                inp = new Mono.KeyboardMIDIInput(g);

                ctr = new ComboCounter(theme.TestFont, ts);

                loading = false;
                g.Play();
            });
        }

        private long fsTime = -1;


        protected override void Update(GameTime gameTime)
        {
            var st = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            if (!loading)
            {

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
                }
            }
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            sb.Begin();
            if (loading)
            {
                sb.DrawString(theme.TestFont, "loading...", Vector2.Zero, Color.White);
            }
            else
            {
                sR.Draw(sb, new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = GraphicsDevice.Viewport.Width,
                    Height = GraphicsDevice.Viewport.Height,
                });
                sb.DrawString(theme.TestFont, "" +Math.Round( ts.Time/ts.PPQ/4), Vector2.Zero, Color.White);


                ctr.Draw(sb, new Vector2
                {
                    X = 100,
                    Y = 100,
                });
            }
            sb.End();

            base.Draw(gameTime);
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
