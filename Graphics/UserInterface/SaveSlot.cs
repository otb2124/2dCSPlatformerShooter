using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Graphics.Generators;
using SharpDX.Direct2D1.Effects;
using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class SaveSlot : UIComponent
    {
        public SpriteFont font;
        public Vector2 position { get; set; }
        public Vector2 scalePosition { get; set; }
        public string combinedText; 
        public float scale;
        public Color color;
        public Color oldColor;
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 offset { get; set; }

        public int id;

        private MenuHandler menuHandler;

        public event EventHandler Clicked;




        public SaveSlot(int id, string areaName, string progressString, string timeString, string dateString, SpriteFont font, Vector2 position, float scale, Color color, MenuHandler menuHandler) // Modify the constructor to accept a Sprites instance
        {
            this.menuHandler = menuHandler;
            this.combinedText = $"{id}    {areaName} - {progressString}%    {timeString}    {dateString}";
            this.font = font;
            this.offset = position;
            this.scale = scale;
            this.color = color;
            this.id = id;
            this.oldColor = color;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {

            // Measure the size of the combined text string
            Vector2 textSize = font.MeasureString(combinedText) * scale;

            // Calculate the size of the background rectangle with some padding
            Vector2 backgroundSize = new Vector2(textSize.X + 5, textSize.Y + 10);

            // Calculate the position for the background rectangle
            Vector2 backgroundPosition = new Vector2(position.X, offset.Y + backgroundSize.Y/3);

            // Draw the background rectangle
            shapes.DrawBoxFill(backgroundPosition, backgroundSize.X, backgroundSize.Y, color); // Adjust color and opacity as needed

            // Draw the text string
            sprites.DrawString(font, combinedText, offset - textSize / 2, 0f, Vector2.Zero, scale, Color.White);
        }



        protected virtual void OnClicked()
        {
            menuHandler.handleSaveSlotClick(this);
        }

        protected virtual void OnHover()
        {
            menuHandler.handleSaveSlotOnHover(this);
        }

        protected virtual void UnHover()
        {
            menuHandler.handleSaveSlotUnHover(this);
        }

        public void Update(Game1 game)
        {
            CheckClick(game);
            CheckHover(game);
        }

        public void CheckClick(Game1 game)
        {

            if (IsClicked(game))
            {
                OnClicked();
            }
        }

        private bool IsClicked(Game1 game)
        {
            // Measure the size of the combined text string
            Vector2 textSize = font.MeasureString(combinedText) * scale;

            // Calculate the size of the background rectangle with some padding
            Vector2 backgroundSize = new Vector2(textSize.X + 5, textSize.Y + 10);

            // Calculate the position for the background rectangle
            Vector2 backgroundPosition = new Vector2(position.X - backgroundSize.X / 2, offset.Y + backgroundSize.Y / 3);

            RectangleF bounds = new RectangleF(backgroundPosition.X, backgroundPosition.Y, backgroundSize.X, backgroundSize.Y);
            Vector2 clickPosition = game.mouse.GetMouseWorldPosition(game, game.screen, game.camera);

            if (game.mouse.IsLeftMouseButtonPressed() && bounds.Contains(clickPosition.X, clickPosition.Y))
            {
                return true;
            }

            return false;
        }

        private bool IsOnHover(Game1 game)
        {
            /// Measure the size of the combined text string
            Vector2 textSize = font.MeasureString(combinedText) * scale;

            // Calculate the size of the background rectangle with some padding
            Vector2 backgroundSize = new Vector2(textSize.X + 5, textSize.Y + 1);

            // Calculate the position for the background rectangle
            Vector2 backgroundPosition = new Vector2(position.X - backgroundSize.X/2, offset.Y + backgroundSize.Y / 3);

            RectangleF bounds = new RectangleF(backgroundPosition.X, backgroundPosition.Y, backgroundSize.X, backgroundSize.Y);
            Vector2 clickPosition = game.mouse.GetMouseWorldPosition(game, game.screen, game.camera);

            return bounds.Contains(clickPosition.X, clickPosition.Y);
        }


        public void CheckHover(Game1 game)
        {

            if (IsOnHover(game))
            {
                OnHover();
            }
            else
            {
                UnHover();
            }
        }
    }
}
