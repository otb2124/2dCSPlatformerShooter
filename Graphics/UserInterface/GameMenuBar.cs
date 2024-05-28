using Microsoft.Xna.Framework;
using Platformer.Graphics.Generators;
using System;
using System.Diagnostics;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class GameMenuBar : UIComponent
    {

        public Vector2 position {  get; set; }
        public Vector2 scalePosition { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 offset { get; set; }
        public Color color;


        public float value;

        private int lastClickedIndex = -1;

        public GameMenuBar(Vector2 positionOffset, float width, float height, Color color)
        {

            this.offset = positionOffset;
            this.position = Vector2.Zero;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {
            // Draw the background bar
            shapes.DrawBoxFill(scalePosition, scaledwidth, scaledHeight, color);

            // Calculate the total padding between all boxes
            float totalPadding = width * 0.075f;

            // Calculate the total width available for all boxes after considering padding
            float totalAvailableWidth = scaledwidth - totalPadding * 6; // 6 boxes + 7 padding spaces

            // Calculate the width of each individual box
            float boxWidth = totalAvailableWidth / 6f;

            // Calculate the padding between each box
            float padding = totalPadding;

            float newHeight = scaledHeight / 1.5f;

            // Draw each box
            for (int i = 0; i < 6; i++)
            {
                // Calculate the position of the current box
                Vector2 boxPosition = new Vector2(scalePosition.X + padding - scaledwidth / 2 + 0.25f, scalePosition.Y - 0.2f);

                // Draw the current box
                shapes.DrawBoxFill(boxPosition, boxWidth, newHeight, Color.White); // Adjust color as needed

                // Move to the next box position
                padding += boxWidth + totalPadding; // Add box width and padding for the next box
            }
        }





        public void Update(Game1 game)
        {
            int boxIndex = checkClick(game);


            if (boxIndex != -1)
            {
                if (boxIndex == 0)
                {
                    game.uiManager.hideGameMenu();
                }
                else
                {
                    game.uiManager.setGameMenu(boxIndex);
                }
            }
            else
            {
                //game.uiManager.hideGameMenu();
            }
        }



        public int checkClick(Game1 game)
        {
            int index = -1;
             // Variable to store the last clicked box index

            // Calculate the total padding between all boxes
            float totalPadding = width * 0.075f;

            // Calculate the total width available for all boxes after considering padding
            float totalAvailableWidth = width - totalPadding * 6; // 6 boxes + 7 padding spaces

            // Calculate the width of each individual box
            float boxWidth = totalAvailableWidth / 6f;

            // Calculate the padding between each box
            float padding = totalPadding;

            // Check for mouse click in each box
            for (int i = 0; i < 6; i++)
            {
                Vector2 boxPosition = new Vector2(position.X + padding - width / 2 + 0.25f, position.Y - 0.2f);

                RectangleF boxBounds = new RectangleF(boxPosition.X, boxPosition.Y-2, boxWidth, height);

                Vector2 clickPosition = game.mouse.GetMouseWorldPosition(game, game.screen, game.camera);

                if (game.mouse.IsLeftMouseButtonPressed() && boxBounds.Contains(clickPosition.X, clickPosition.Y))
                {
                    if (lastClickedIndex == i)
                    {
                        index = 0; // Reset index if clicked twice
                        lastClickedIndex = -1;
                    }
                    else
                    {
                        index = i + 1;
                        lastClickedIndex = i;
                    }

                }

                padding += boxWidth + totalPadding;
            }

            return index;
        

    }



}
}
