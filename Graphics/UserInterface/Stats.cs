using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Platformer.Graphics.Generators;

namespace Platformer.Graphics.Menu
{
    public sealed class Stats
    {
        private static Lazy<Stats> LazyInstance = new Lazy<Stats>(() => new Stats());

        public static Stats Instance
        {
            get { return LazyInstance.Value; }
        }

        private Sprites sprites;
        private SpriteFont font;
        private bool started;
        private float y;

        private Stats()
        {
            sprites = null;
            font = null;
            started = false;
            y = 0f;
        }

        public void Begin(Sprites sprites, SpriteFont font)
        {
            if (started)
            {
                throw new Exception("Already started.");
            }

            this.sprites = sprites ?? throw new ArgumentNullException("sprites");
            this.font = font ?? throw new ArgumentNullException("font");

            y = 0;
            started = true;

            this.sprites.Begin(textureFiltering: true);
        }

        public void End()
        {
            if (!started)
            {
                throw new Exception("Never started.");
            }

            started = false;
            sprites.End();
        }

        public void Draw(object obj)
        {
            Draw(obj, Color.White);
        }

        public void Draw(object obj, Color color)
        {
            if (!started)
            {
                throw new Exception("Not started.");
            }

            string text = obj.ToString();
            Vector2 size = font.MeasureString(text);

            sprites.DrawString(font, text, new Vector2(2, y - 2), Color.Black);
            sprites.DrawString(font, text, new Vector2(0, y), color);

            y += size.Y;
        }

        public void Draw(string text)
        {
            Draw(text, Color.White);
        }

        public void Draw(string text, Color color)
        {
            if (!started)
            {
                throw new Exception("Not started.");
            }

            Vector2 size = font.MeasureString(text);

            sprites.DrawString(font, text, new Vector2(2, y - 2), Color.Black);
            sprites.DrawString(font, text, new Vector2(0, y), color);

            y += size.Y;
        }
    }
}
