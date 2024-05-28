using Platformer.Physics;
using System.Drawing;

namespace Platformer.Entities.Types
{
    public class InterractiveItem : FlatEntity
    {


        public RectangleF interactionRectangle;
        public float interactionRectangleScaleX;
        public string[] interractions;
        public int interractionID;

        public int eventID;

        public bool Interraction = false;

        public InterractiveItem(float width, float height, bool isStatic, int eventID, FlatVector pos) : base(width, height, isStatic, pos)
        {
            this.eventID = eventID;
            Body.owner = this;

            interactionRectangleScaleX = 2;

            interactionRectangle = new RectangleF(this.Body.Position.X, this.Body.Position.Y, this.Body.Width * interactionRectangleScaleX, this.Body.Height * (interactionRectangleScaleX / 2));
            interractions = new string[] { "Walk In" };
            interractionID = 0;
        }





        public void Update(Game1 game)
        {

            interactionRectangle = new RectangleF(this.Body.Position.X - this.Body.Width * 2, this.Body.Position.Y - 0.2f - this.Body.Height/2, this.Body.Width * interactionRectangleScaleX * 2, this.Body.Height * (interactionRectangleScaleX / 2));


            if (interactionRectangle.Contains(new PointF(game.player.Body.Position.X, game.player.Body.Position.Y)))
            {
                game.player.isInterracting = this;
                Interraction = true;
            }
            else
            {
                if (Interraction)
                {
                    game.player.isInterracting = null;
                    Interraction = false;
                }
            }
            





            base.Update(game);
        }
    }
}
