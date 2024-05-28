using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.Graphics.Generators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class ConsoleTextField : UIComponent
    {
        public SpriteFont font;
        public Vector2 position { get; set; }
        public StringBuilder text;
        public float scale;
        public Color color;
        public bool isSelected;
        public List<string> commandHistory;
        public int commandIndex;

        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 scalePosition { get; set; }
        public Vector2 offset { get; set; }

        private MenuHandler menuHandler;

        public ConsoleTextField(SpriteFont font, Vector2 position, float scale, Color color, MenuHandler menuHandler)
        {
            this.menuHandler = menuHandler;
            this.font = font;
            this.offset = position;
            this.scale = scale;
            this.color = color;
            this.text = new StringBuilder();
            this.text.Append(">command prompt");
            this.commandHistory = new List<string>();
            this.commandHistory.Add("Debug mode on.");

            this.isSelected = false;
            this.commandIndex = -1;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {
            Color boxColor = new Color(0.4f, 0.4f, 0.4f, 0.001f);
            float lineHeight = 1.1f; // Adjust line height as needed
            int maxDisplayedCommands = 9; // Maximum number of commands to display

            // Draw the main box
            width = 10f;
            height = 1f;
            shapes.DrawBoxFill(position, scaledwidth, scaledHeight, boxColor);
            sprites.DrawString(font, text.ToString(), new Vector2(position.X - 5f, position.Y-0.5f), 0f, Vector2.Zero, 0.05f * (20.0f / camera.Zoom), Color.White);

            // Check if the item is selected
            if (isSelected)
            {
                // Draw the selected box
                width = 40f;
                height = 10f;
                shapes.DrawBoxFill(new Vector2(position.X + 15f, position.Y + 6f), scaledwidth, scaledHeight, boxColor);

                float startY = position.Y - 0.2f + lineHeight;

                // Determine the number of commands to display, considering the maximum limit
                int numCommands = Math.Min(commandHistory.Count, maxDisplayedCommands);

                // Display the command history in reverse order
                for (int i = 0; i < numCommands; i++)
                {
                    // Calculate the offset for each command
                    float offset = startY + i * lineHeight;

                    // Get the index of the command to display
                    int commandIndex = commandHistory.Count - 1 - i;

                    // Draw the command
                    sprites.DrawString(font, commandHistory[commandIndex], new Vector2(position.X - 5f, offset), 0f, Vector2.Zero, 0.05f, Color.White);
                }
            }
        }



        public void Update(Game1 game)
        {
            // Update the position of the console relative to the player's position
            this.position = new Vector2(game.camera.Position.X - 25, game.camera.Position.Y - 15);

            // Check if the text field is clicked
            if (IsClicked(game))
            {
                if (isSelected)
                {
                    isSelected = false;
                }
                else
                {
                    isSelected = true; // Set the text field as selected
                }
                
            }

            if (isSelected)
            {
                // If the text field is selected, inform the menu handler
                menuHandler.handleTextFieldSelected(this);

                // Check if the text field is empty, and if so, replace it with the prompt character
                if (text.ToString() == ">")
                {
                    text.Clear();
                    text.Append(">");
                }
            }
           
        }


        private bool IsClicked(Game1 game)
        {
            // Check if the mouse is clicked within the bounds of the text field
            Vector2 textSize = font.MeasureString(text) * scale;
            RectangleF bounds = new RectangleF(position.X, position.Y, textSize.X+4f, textSize.Y+4f);
            Vector2 clickPosition = game.mouse.GetMouseWorldPosition(game, game.screen, game.camera);

            return game.mouse.IsLeftMouseButtonPressed() && bounds.Contains((int)clickPosition.X, (int)clickPosition.Y);
        }
    }
}
