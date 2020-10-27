using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Uso.Core.MIDI;
using Uso.Mono.Components;
using Uso.Mono.Customization;
using Uso.Mono.Input;

namespace Uso.Mono.Screens
{
    class MainGame : Game, ScreenManager
    {
        private readonly Stack<Screen> history = new Stack<Screen>();
        private Screen curScreen;
        private readonly GraphicsDeviceManager graphics;

        //private readonly RasterizerState RasterizerState;
        private GameLayers layers;
        private Task<Theme> themeTask;

        private Cursor cursor;

        /// <summary>
        /// Commonly used objects passed to pretty much every class
        /// Subject to change
        /// </summary>
        public class Globals
        {
            public InputManager InputManager;
            public Theme Theme;
            public ScreenManager ScreenManager;
        }
        private readonly Globals globals;

        public void Switch(Screen newScreen)
        {
            curScreen = newScreen;
        }

        public MainGame()
        {

            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = false;
            //graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            //RasterizerState = new RasterizerState { MultiSampleAntiAlias = true };

            globals = new Globals
            {
                InputManager = new InputManager(),
                ScreenManager = this,
                Theme = new Theme(),
            };
        }

        //private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        //{

        //    graphics.PreferMultiSampling = true;
        //}

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";

            globals.Theme.LoadBasic(Content);
            this.layers = new GameLayers
            {
                MainLayer = new SpriteBatch(GraphicsDevice),
            };

            this.themeTask = Task.Run(() => globals.Theme.LoadFromContent(Content));

            Func<Task<Screen>> loadAll = async () =>
            {
                await themeTask;
                this.cursor = new Cursor(globals.Theme.Cursor)
                {
                    ClickPoint = new Vector2
                    {
                        X=127,
                        Y=106,
                    },
                    DefaultScale=0.5f,
                };
                return new GameMenu(globals);
            };

            Switch(new LoadingScreen(globals, loadAll()));
        }

        protected override void Update(GameTime gameTime)
        {
            globals.InputManager.Update(Keyboard.GetState());
            curScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            layers.MainLayer.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive
            //null,
            //null,
            //null,
            //RasterizerState
            ); ;
            curScreen.Draw(this.layers, new Rectangle
            {
                Location = Point.Zero,
                Width = GraphicsDevice.Viewport.Width,
                Height = GraphicsDevice.Viewport.Height,
            });
            if(this.cursor!=null)
                this.cursor.Draw(layers.MainLayer);
            layers.MainLayer.End();
            base.Draw(gameTime);
        }
    }
}
