using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Uso.Mono.Customization;

namespace Uso.Mono.Screens
{
    /// <summary>
    /// subject to change in the future
    /// </summary>
    class SelectedSong
    {
        public string SongFile;
    }

    class SongSelect : Screen
    {
        private readonly MainGame.Globals globals;
        private readonly GameMenu previous;

        public SongSelect(MainGame.Globals globals, GameMenu previous)
        {
            this.globals = globals;
            this.previous = previous;
        }

        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(globals.Theme.TestFont, "press enter to select desire drive", area.Location.ToVector2(), Color.White);

        }

        public void Update(GameTime gameTime)
        {
            var st = Keyboard.GetState();
            if (globals.InputManager.DidPress(Keys.Enter))
            {
                globals.ScreenManager.Switch(new SongLoader(globals,this, new SelectedSong
                {
                    SongFile="Assets/desire_drive.mid"
                }));
            }
            else if (globals.InputManager.DidPress(Keys.Escape))
            {
                globals.ScreenManager.Switch(previous);
            }

        }
    }
}
