using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uso.Core.MIDI;
using Uso.Core.MIDI.Parser;
using Uso.Core.Song;
using Uso.Core.Timing;

namespace Uso.UWP

{
    public class TestGame : Game, TimeSourceFactory
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Manager midiManager;

        public TestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            midiManager = new GsSynthManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            var m = new MidiFile("Assets/test.mid");
            Task.Run(async () =>
            {
                var g =await Uso.Core.Game.NewGame(MidiSong.FromMidi(m), midiManager, this);
                g.Play();
            });
        }

        private long fsTime=-1;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            long now = FileSystemTime.Now1;
            if (fsTime != -1)
            {
                // TODO: Add your update logic here
                foreach (var t in timeSources)
                    t.Update((now-fsTime) /10.0);
            }
            fsTime = now;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here


            base.Draw(gameTime);
        }

        private List<SimpleTimeSource> timeSources = new List<SimpleTimeSource>();

        public TimeSource NewTimeSource(long PPQ, long Tempo)
        {
            var x = new SimpleTimeSource(PPQ, Tempo);
            timeSources.Add(x);
            return x;
        }

    }
}
