using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System;

namespace Uso.Mono.Components
{
    class Cursor
    {
        private readonly Texture2D image;
        public Vector2 ClickPoint;
        public float DefaultScale = 1;

        public Cursor(Texture2D image)
        {
            this.image = image;
        }
        public void Draw(SpriteBatch sb)
        {
            var st = Mouse.GetState();
            float sz = DefaultScale;
            if (st.LeftButton.HasFlag(ButtonState.Pressed))
            {
                sz*=1.1f;
            }
            sb.Draw(
                texture: image,
                destinationRectangle: new Rectangle
                {
                    Location = st.Position - (ClickPoint * sz).ToPoint(),
                    Size = (image.Bounds.Size.ToVector2() * sz).ToPoint(),
                },

                color: Color.White
            );
        }
    }
}
