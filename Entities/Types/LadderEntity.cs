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
    public class LadderEntity : StaticEntity
    {

        public RectangleF entryRect;
        public float interactionRectangleScaleX = 2;
        public bool Interraction = false;

        public LadderEntity(float width, float height, FlatVector pos, float Rotation) : base(width, height, pos, Rotation)
        {
            entryRect = new RectangleF(pos.X-width/2, pos.Y-height/2, width, height);
            Body.owner = this;
        }



        public void Update(Game1 game)
        {
            if (entryRect.Contains(new PointF(game.player.Body.Position.X, game.player.Body.Position.Y)))
            {
                game.player.isOnLadder = true;
                Interraction = true;
            }
            else
            {
                if (Interraction)
                {
                    game.player.isOnLadder = false;
                    Interraction = false;
                }
            }






            base.Update(game);
        }

    }
}
