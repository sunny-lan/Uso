using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Uso.Mono.Customization;

namespace Uso.Mono.Screens
{
    class GameMenu : Screen
    {
        private readonly SongSelect songSelect;
        private readonly MainGame.Globals globals;

        public GameMenu(MainGame.Globals globals)
        {
            this.songSelect = new SongSelect(globals,this);
            this.globals = globals;
        }
        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(globals.Theme.TestFont, "press enter to start", area.Location.ToVector2(), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (globals.InputManager.DidPress(Keys.Enter))
            {
                globals.ScreenManager.Switch(songSelect);
            }
            else if (globals.InputManager.DidPress(Keys.Escape))
            {
                globals.ScreenManager.Exit();
            }


        }
    }
}
