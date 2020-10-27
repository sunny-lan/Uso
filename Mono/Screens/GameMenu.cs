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
        private readonly Theme theme;
        private readonly ScreenManager mgr;
        private readonly SongSelect songSelect;

        public GameMenu(Theme theme, ScreenManager mgr)
        {
            this.theme = theme;
            this.mgr = mgr;
            this.songSelect = new SongSelect(mgr, theme,this);
        }
        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(theme.TestFont, "press enter to start", area.Location.ToVector2(), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            var st = Keyboard.GetState();
            if (st.IsKeyDown(Keys.Enter))
            {
                mgr.Switch(songSelect);
            }
            else if (st.IsKeyDown(Keys.Escape))
            {
                mgr.Exit();
            }


        }
    }
}
