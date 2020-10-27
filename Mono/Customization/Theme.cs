using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uso.Mono.Customization
{

    class Theme
    {
        public Texture2D Lane;
        public Texture2D Note;
        public SpriteFont TestFont;
        public Texture2D Cursor;
    }

    static class ThemeLoader
    {
        public static Theme LoadFromContent(this Theme t, ContentManager mgr)
        {
            t.Lane = mgr.Load<Texture2D>("Images/lane");
            t.Note = mgr.Load<Texture2D>("Images/note");
            t.Cursor = mgr.Load<Texture2D>("Images/cursor");
            return t;
        }

        public static Theme LoadBasic(this Theme t, ContentManager mgr)
        {
            t.TestFont = mgr.Load<SpriteFont>("Fonts/test");
            return t;
        }
    }

}
