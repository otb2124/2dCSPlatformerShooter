using Platformer.Physics;
using System.Drawing;

namespace Platformer.Entities.Types
{
    public class NPC : LiveEntity
    {

        public bool Interraction = false;
        public RectangleF interactionRectangle;
        public float interactionRectangleScaleX;
        public string[] interractions;
        public int interractionID;

        public int[] dialogueIds;
        public int currentDialogueId;


        public NPC(float width, float height, float maxHP, float currentHP, FlatVector pos) : base(width, height, maxHP, currentHP, pos)
        {

            Body.owner = this;

            interactionRectangleScaleX = 2;

            interactionRectangle = new RectangleF(this.Body.Position.X, this.Body.Position.Y, this.Body.Width * interactionRectangleScaleX, this.Body.Height * (interactionRectangleScaleX / 2));

            dialogueIds = new int[] { 0, 1};
            currentDialogueId = 0;

            interractions = new string[] { "Talk", "Trade" };

            interractionID = 0;
    }



        public void Update(Game1 game)
        {

            interactionRectangle = new RectangleF(this.Body.Position.X - this.Body.Width * 2, this.Body.Position.Y - 0.2f, this.Body.Width * interactionRectangleScaleX * 2, this.Body.Height * (interactionRectangleScaleX / 2));

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
