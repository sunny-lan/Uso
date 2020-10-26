using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uso.Mono.Screens
{
    class GameLayers
    {
        public SpriteBatch MainLayer;
    }

    interface Drawable
    {
        void Draw(GameLayers output, Rectangle area);
    }

    interface Screen:Drawable
    {
        void Update(GameTime gameTime);
    }

    interface ScreenManager
    {
        void Switch(Screen newScreen, bool pushToHistory);
        void Back();
    }
}
