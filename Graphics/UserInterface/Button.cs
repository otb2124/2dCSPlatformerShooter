using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Graphics.Generators;
using SharpDX.Direct2D1.Effects;
using System;
using System.ComponentModel.Design;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Graphics.Menu
{
    public class Button : UIComponent
    {
        public SpriteFont font;
        public Vector2 position {  get; set; }
        public Vector2 scalePosition { get; set; }
        public string text;
        public float scale;
        public Color color;
        public float height { get; set; }
        public float width { get; set; }
        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 offset { get; set; }

        private MenuHandler menuHandler;

        public event EventHandler Clicked;

        public Button(string text, SpriteFont font, Vector2 position, float scale, Color color, MenuHandler menuHandler) // Modify the constructor to accept a Sprites instance
        {
            this.menuHandler = menuHandler;
            this.text = text;
            this.font = font;
            this.offset = position;
            this.scale = scale;
            this.color = color;
        }

        public void Draw(Sprites sprites, Shapes shapes, Camera camera) // Change the Draw method to not accept a SpriteBatch
        {
            sprites.DrawString(font, text, position, 0f, Vector2.Zero, scale, color);
        }

        protected virtual void OnClicked()
        {
            menuHandler.handleButtonClick(this);
        }

        protected virtual void OnHover()
        {
            menuHandler.handleButtonHover(this);
        }

        protected virtual void UnHover()
        {
            menuHandler.handleButtonUnhover(this);
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
            Vector2 textSize = font.MeasureString(text) * scale;
            RectangleF bounds = new RectangleF(position.X, position.Y, textSize.X, textSize.Y);
            Vector2 clickPosition = game.mouse.GetMouseWorldPosition(game, game.screen, game.camera);

            if (game.mouse.IsLeftMouseButtonPressed() && bounds.Contains(clickPosition.X, clickPosition.Y))
            {
                return true;
            }

            return false;

        }

        private bool IsOnHover(Game1 game)
        {
            Vector2 textSize = font.MeasureString(text) * scale;
            RectangleF bounds = new RectangleF(position.X, position.Y, textSize.X, textSize.Y);
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
