using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Graphics.Generators;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class TitlePanel : UIComponent
    {

        public Vector2 position {  get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 scalePosition { get; set; }
        public Vector2 offset { get; set; }
        public Color color;
        public SpriteFont font;

        public float value;
        public string text;

        public List<UIComponent> components;

        public TitlePanel(string text, SpriteFont font, float width, float height, Color color)
        {
            this.position = Vector2.Zero;
            this.offset = new Vector2(0, -2f);
            this.width = width;
            this.height = height;
            this.color = color;
            this.font = font;
            this.text = text;
            components = new List<UIComponent>();
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {
            // Draw the panel
            if(text != "Inventory")
            {
                shapes.DrawBoxFill(position, width, height, color);
            }
            

            Vector2 textSize = font.MeasureString(text) * 0.1f; // Apply the scale factor
            Vector2 textPosition = new Vector2(position.X + (width - textSize.X) / 2 - width/2, position.Y+height/2.25f);

            // Draw the text
            sprites.DrawString(font, text, textPosition, 0f, Vector2.Zero, 0.1f, Color.Black);


            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i] != null)
                {
                    this.components[i].Draw(sprites, shapes, camera);
                }
            }
            
        }






        public void Update(Game1 game)
        {

            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.components[i] != null)
                {
                    this.components[i].Update(game);
                }
            }
        }



        public void add(UIComponent component)
        {
            this.components.Add(component);
        }

    }
}
