using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Graphics.Generators;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class Label : UIComponent
    {
        public SpriteFont font;
        public Vector2 position {  get; set; }
        public Vector2 scalePosition { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 offset { get; set; }
        public string text;
        public float scale;
        public Color color;


        public Label(string text, SpriteFont font, Vector2 position, float scale, Color color)
        {
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

        public void Update() { }
    }
}
