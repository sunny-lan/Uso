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

        private MidiManager midiManager;

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
                var g =await Uso.Core.Game.NewGame(Core.Song.MidiSong.fromMidi(m), midiManager, this);
                g.Play();
            });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            foreach (var t in timeSources)
                t.Update(gameTime.ElapsedGameTime.TotalMilliseconds * 1000);

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
