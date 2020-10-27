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

    //TODO have more thought about the screen lifecycle 
    // for example when can a screen be returned to etc
    interface Screen:Drawable
    {
        void Update(GameTime gameTime);
    }

    interface ScreenManager
    {
        void Exit();
        void Switch(Screen newScreen);
    }
}
