using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Flat.Input
{
    public sealed class FlatKeyboard
    {
        private static Lazy<FlatKeyboard> LazyInstance = new Lazy<FlatKeyboard>(() => new FlatKeyboard());

        public static FlatKeyboard Instance
        {
            get { return LazyInstance.Value; }
        }

        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;
        
        public bool IsKeyAvailable
        {
            get { return this.currKeyboardState.GetPressedKeyCount() > 0; }
        }
        
        private FlatKeyboard()
        {
            this.currKeyboardState = Keyboard.GetState();
            this.prevKeyboardState = this.currKeyboardState;
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

        public bool IsKeyReleased(Keys key)
        {
            return !this.currKeyboardState.IsKeyDown(key) && this.prevKeyboardState.IsKeyDown(key);
        }


        public List<Keys> GetPressedKeys()
        {
            List<Keys> pressedKeys = new List<Keys>();

            // Check if each key is currently pressed
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                // Skip modifier keys and other special keys
                if (key >= Keys.None && key <= Keys.OemClear)
                {
                    if (IsKeyDown(key))
                    {
                        pressedKeys.Add(key);
                    }
                }
                else
                {
                    // Handle special cases for some keys not covered by the enum range
                    switch (key)
                    {
                        case Keys.OemBackslash: // For the slash symbol
                        case Keys.OemQuestion:  // For the question mark symbol
                            if (IsKeyDown(key))
                            {
                                pressedKeys.Add(key);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return pressedKeys;
        }

    }
}
