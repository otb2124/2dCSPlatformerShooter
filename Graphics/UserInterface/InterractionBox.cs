using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Graphics.Generators;
using System;

namespace Platformer.Graphics.Menu
{
    public class InterractionBox : UIComponent
    {
        public Vector2 position {  get; set; }
        public Vector2 scalePosition { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 offset { get; set; }
        public float padding = 0.2f; // Padding around the text
        public Color color;
        public SpriteFont font;
        public string text;

        public InterractionBox(string text, SpriteFont font, Color color)
        {
            this.position = Vector2.Zero;
            this.color = color;
            this.font = font;
            this.text = text;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {
            // Calculate text size
            Vector2 textSize = font.MeasureString(text) * 0.15f;

            // Calculate box size based on text size and padding
            float boxWidth = textSize.X + 2 * padding;
            float boxHeight = textSize.Y + 2.5f * padding;

            // Draw background box
            shapes.DrawBoxFill(position, boxWidth, boxHeight, Color.Gray);

            // Calculate text position within the box
            Vector2 textPosition = new Vector2(position.X - textSize.X/2, position.Y - textSize.Y/2);

            // Draw text
            sprites.DrawString(font, text, textPosition, 0f, Vector2.Zero, 0.15f, color);
        }

        public void Update(Game1 game)
        {
            position = new Vector2(game.player.isInterracting.Body.Position.X, game.player.isInterracting.Body.Position.Y + game.player.isInterracting.Body.Height + 2f);
        }
    }
}
