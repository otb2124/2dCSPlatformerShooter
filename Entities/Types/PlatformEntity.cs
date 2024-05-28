using Microsoft.VisualBasic;
using Platformer.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Entities.Types
{
    public class PlatformEntity : StaticEntity
    {

        public bool isCollidable = false;
        public RectangleF entryRect;
        public bool Interraction = false;

        public PlatformEntity(float width, float height, FlatVector pos, float Rotation) : base(width, height, pos, Rotation)
        {
            entryRect = new RectangleF(pos.X - width / 2, pos.Y, width, height*4);
            Body.owner = this;
        }



        public void Update(Game1 game)
        {

            if (entryRect.Contains(new PointF(game.player.Body.Position.X, game.player.Body.Position.Y)))
            {
                game.player.isOnPlatform = true;
                Interraction = true;
            }
            else
            {
                if (Interraction)
                {
                    game.player.isOnPlatform = false;
                    Interraction = false;
                }
            }




            if (game.player.Body.Position.Y > this.Body.Position.Y)
            {
                isCollidable = true;
            }
            else
            {
                isCollidable = false;
            }

            base.Update(game);
        }

    }
}
