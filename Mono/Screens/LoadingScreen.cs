using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Threading.Tasks;
using Uso.Mono.Customization;

namespace Uso.Mono.Screens
{
    class LoadingScreen : Screen
    {
        private readonly MainGame.Globals globals;
        private readonly Task<Screen> t;


        public LoadingScreen(
            MainGame.Globals globals,
            Task<Screen> t
        )
        {
            this.globals = globals;
            this.t = t;
        }
        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(globals.Theme.TestFont, "loading", area.Location.ToVector2(), Color.White);
        }
        bool switched;
        public void Update(GameTime gameTime)
        {
            if (t.IsCompleted )
            {
                if (switched) throw new InvalidAsynchronousStateException("Should be finished loading already");
                switched = true;
                globals.ScreenManager.Switch(t.Result);
            }
        }
    }
}
