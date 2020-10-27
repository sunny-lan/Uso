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
        private readonly ScreenManager mgr;
        private readonly Theme theme;
        private readonly GameMenu previous;

        public SongSelect(ScreenManager mgr, Theme theme, GameMenu previous)
        {
            this.mgr = mgr;
            this.theme = theme;
            this.previous = previous;
        }

        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(theme.TestFont, "press enter to select desire drive", area.Location.ToVector2(), Color.White);

        }

        public void Update(GameTime gameTime)
        {
            var st = Keyboard.GetState();
            if (st.IsKeyDown(Keys.Enter))
            {
                mgr.Switch(new SongLoader(mgr, theme,this, new SelectedSong
                {
                    SongFile="Assets/desire_drive.mid"
                }));
            }
            else if (st.IsKeyDown(Keys.Escape))
            {
                mgr.Switch(previous);
            }

        }
    }
}
