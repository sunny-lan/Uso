using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Uso.Core.MIDI;
using Uso.Mono.Customization;

namespace Uso.Mono.Screens
{
    class MainGame : Game, ScreenManager
    {
        private readonly Stack<Screen> history = new Stack<Screen>();
        private Screen curScreen;
        private readonly GraphicsDeviceManager graphics;
        private readonly RasterizerState RasterizerState;
        private Theme theme;
        private GameLayers layers;
        private Task<Theme> themeTask;

        public void Switch(Screen newScreen)
        {
            curScreen = newScreen;
        }

        public MainGame()
        {
            
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            //graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            //RasterizerState = new RasterizerState { MultiSampleAntiAlias = true };

        }

        //private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        //{

        //    graphics.PreferMultiSampling = true;
        //}

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";

            theme = new Theme();
            theme.LoadBasic(Content);
            this.layers = new GameLayers
            {
                MainLayer = new SpriteBatch(GraphicsDevice),
            };

            this.themeTask = Task.Run(() => theme.LoadFromContent(Content));

            Func<Task<Screen>> loadAll = async () =>
            {
                return new GameMenu(await themeTask, this);
            };

            Switch(new LoadingScreen(this, loadAll(), theme));
        }

        protected override void Update(GameTime gameTime)
        {
            curScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            layers.MainLayer.Begin(
                blendState: BlendState.Additive
            //SpriteSortMode.Immediate,
            //null,
            //null,
            //null,
            //RasterizerState
            ); ;
            curScreen.Draw(this.layers,new Rectangle
            {
                Location=Point.Zero,
                Width = GraphicsDevice.Viewport.Width,
                Height = GraphicsDevice.Viewport.Height,
            });
            layers.MainLayer.End();
            base.Draw(gameTime);
        }
    }
}
