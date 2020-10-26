using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uso.Mono.Screens
{
    class GameMenu : Screen
    {
        private readonly Theme theme;

        public GameMenu(Theme theme)
        {
            this.theme = theme;
        }
        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(theme.TestFont, "hi", area.Location.ToVector2(), Color.White);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
