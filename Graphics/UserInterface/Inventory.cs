using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Entities.Types;
using Platformer.Graphics.Generators;

namespace Platformer.Graphics.Menu
{
    public class Inventory : UIComponent
    {
        public Vector2 position {  get; set; }
        public Vector2 scalePosition { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 offset { get; set; }
        public Color color;
        public SpriteFont font;
        public LiveEntity lent;

        public Inventory(LiveEntity lent, SpriteFont font, Vector2 positionOffset, float width, float height, Color color)
        {
            this.position = Vector2.Zero;
            this.offset = positionOffset;
            this.width = width;
            this.height = height;
            this.color = color;
            this.font = font;
            this.lent = lent;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {
            int rows1 = 2;
            int cols1 = 2;

            int rows2 = 1;
            int cols2 = 3;

            // Calculate box width and height for the first grid
            float boxWidth1 = width / cols1 / 2; // Adjusted for smaller size
            float boxHeight1 = height / rows1 / 2; // Adjusted for smaller size

            // Calculate text scale
            float textScale = 0.1f;

            // Draw background
            // shapes.DrawBoxFill(position, width, height, color);

            // Draw first grid (2x2)
            for (int row = 0; row < rows1; row++)
            {
                for (int col = 0; col < cols1; col++)
                {
                    // Calculate position for the current box in the first grid
                    Vector2 boxPosition = new Vector2(scalePosition.X + col * boxWidth1 * 1.5f, scalePosition.Y + row * boxHeight1 * 1.5f); // Adjusted for smaller size

                    shapes.DrawBoxFill(boxPosition, boxWidth1, boxHeight1, Color.Gray);

                    string itemName = lent.inventory[row * cols1 + col].Name;
                }
            }

            // Draw second grid (3x1)
            for (int row = 0; row < rows2; row++)
            {
                for (int col = 0; col < cols2; col++)
                {
                    // Calculate position for the current box in the second grid
                    Vector2 boxPosition = new Vector2(scalePosition.X + col * boxWidth1 * 1.5f - boxWidth1, scalePosition.Y - boxHeight1 * 1.5f - height/4); // Adjusted for smaller size and reversed y-axis

                    // Draw the box for the second grid
                    shapes.DrawBoxFill(boxPosition, boxWidth1, boxHeight1, Color.Gray);

                    // Calculate text position for the current item in the second grid
                    string itemName = lent.inventory[cols1 * rows1 + row * cols2 + col].Name;
                }
            }
        }





        public void Update(Game1 game)
        {
            scalePosition = FlatConverter.ToVector2(lent.Body.Position);
        }
    }
}
