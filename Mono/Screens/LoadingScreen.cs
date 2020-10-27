using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Threading.Tasks;
using Uso.Mono.Customization;

namespace Uso.Mono.Screens
{
    class LoadingScreen : Screen
    {
        private readonly ScreenManager mgr;
        private readonly Task<Screen> t;
        private readonly Theme theme;


        public LoadingScreen(
            ScreenManager mgr,
            Task<Screen> t,
            Theme theme
        )
        {
            this.mgr = mgr;
            this.t = t;
            this.theme = theme;
        }
        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(theme.TestFont, "loading", area.Location.ToVector2(), Color.White);
        }
        bool switched;
        public void Update(GameTime gameTime)
        {
            if (t.IsCompleted )
            {
                if (switched) throw new InvalidAsynchronousStateException("Should be finished loading already");
                switched = true;
                mgr.Switch(t.Result);
            }
        }
    }
}
