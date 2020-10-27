using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uso.Mono.Input
{
    class InputManager
    {
        public KeyboardState LastState { get; private set; }
        public KeyboardState CurState { get; private set; }

        /// <summary>
        /// Returns true when key was unpressedbefore and now pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DidPress(Keys key)
        {
            return CurState.IsKeyDown(key) && !LastState.IsKeyDown(key);
        }

        public void Update(KeyboardState curState)
        {
            this.LastState = this.CurState;
            this.CurState = curState;
        }
    }
}
