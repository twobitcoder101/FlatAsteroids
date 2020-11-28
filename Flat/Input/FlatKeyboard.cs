using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Flat.Input
{
    public sealed class FlatKeyboard
    {
        private static readonly Lazy<FlatKeyboard> Lazy = new Lazy<FlatKeyboard>(() => new FlatKeyboard());

        public static FlatKeyboard Instance
        {
            get { return Lazy.Value; }
        }

        private KeyboardState prevKeyboardState;
        private KeyboardState currKeyboardState;

        public FlatKeyboard()
        {
            this.prevKeyboardState = Keyboard.GetState();
            this.currKeyboardState = prevKeyboardState;
        }

        public void Update()
        {
            this.prevKeyboardState = this.currKeyboardState;
            this.currKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return this.currKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyClicked(Keys key)
        {
            return this.currKeyboardState.IsKeyDown(key) && !this.prevKeyboardState.IsKeyDown(key);
        }
    }
}
