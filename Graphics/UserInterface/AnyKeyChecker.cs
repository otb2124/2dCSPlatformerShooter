using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.Graphics.Generators;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class AnyKeyCheker : UIComponent
    {
        public SpriteFont font;
        public string text;
        public float scale;
        public Color color;

        private MenuHandler menuHandler;

        public float height {  get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 position { get; set; }
        public Vector2 scalePosition { get; set; }
        public Vector2 offset { get; set; }

        public event EventHandler Clicked;

        public AnyKeyCheker(string text, SpriteFont font, Vector2 position, float scale, Color color, MenuHandler menuHandler) // Modify the constructor to accept a Sprites instance
        {
            this.menuHandler = menuHandler;
            this.text = text;
            this.font = font;
            this.offset = position;
            this.scale = scale;
            this.color = color;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {
            sprites.DrawString(font, text, position, 0f, Vector2.Zero, scale, color);
        }




        public void Update(Game1 game)
        {
            if (IsAnyClicked(game))
            {
                menuHandler.handleAnyClick();
            }
            
        }

        private bool IsAnyClicked(Game1 game)
        {

            List<Keys> pressedKeys = game.keyboard.GetPressedKeys();
            List<MouseButtons> pressedButtons = game.mouse.GetPressedButtons();
            return (pressedKeys.Count + pressedButtons.Count) > 0;

        }

    }
}
