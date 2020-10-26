using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Uso.Core.MIDI;

namespace Uso.Mono.Screens
{
    class MainGame : Game, ScreenManager
    {
        private readonly Stack<Screen> history = new Stack<Screen>();
        private Screen curScreen;
        private readonly GraphicsDeviceManager graphics;
        private Theme theme;
        private GameLayers layers;
        private Task<Theme> themeTask;
        private Task<Listener> midiOutputTask;

        private void switchInternal(Screen screen)
        {
            curScreen = screen;
        }

        public void Back()
        {
            history.Pop();

            if (history.Count == 0)
            {
                Exit();
                return;
            }

            switchInternal(history.Peek());
        }

        public void Switch(Screen newScreen, bool pushToHistory)
        {
            switchInternal(newScreen);
            if (pushToHistory)
            {
                history.Push(newScreen);
            }
        }

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";

            theme = new Theme();
            theme.LoadBasic(Content);
            this.layers = new GameLayers
            {
                MainLayer = new SpriteBatch(GraphicsDevice),
            };

            var midiManager = new UWP.UWPMidiManager();
            this.themeTask = Task.Run(() => theme.LoadFromContent(Content));
            this.midiOutputTask = midiManager.CreateOutput();

            Func<Task<Screen>> loadAll = async () =>
            {
                return new GameMenu(await themeTask);
            };

            switchInternal(new LoadingScreen(this, loadAll(), theme)
            {
                PushToHistory = true,
            });
        }

        protected override void Update(GameTime gameTime)
        {
            curScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            layers.MainLayer.Begin();
            curScreen.Draw(this.layers, GraphicsDevice.Viewport.Bounds);
            layers.MainLayer.End();
            base.Draw(gameTime);
        }
    }
}
