using Microsoft.Xna.Framework;
using Platformer.Graphics.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platformer.Graphics.Menu
{
    public interface UIComponent
    {
        public float height { get; set; }
        public float width { get; set; }

        public float scaledHeight { get; set; }
        public float scaledwidth { get; set; }
        public Vector2 position { get; set; }
        public Vector2 offset { get; set; }
        public Vector2 scalePosition { get; set; }
        public void Draw(Sprites sprites, Shapes shapes, Camera camera)
        {

        }

        public void Update(Game1 game)
        {

        }
    }
}
