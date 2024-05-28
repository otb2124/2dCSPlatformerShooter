using Microsoft.Xna.Framework;
using Platformer.Physics;


namespace Platformer.Entities.Types
{
    public class DestroyableEntity : FlatEntity
    {


        public float maxHP;
        public float currentHP;
        public bool isDestroyed;

        public DestroyableEntity(float width, float height, bool isStatic, float maxHP, float currentHP, FlatVector pos, float Rotation) : base(width, height, isStatic, pos)
        {
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.Body.Rotate(Rotation);
        }

        public DestroyableEntity(float radius, bool isStatic, float maxHP, float currentHP, FlatVector pos, float Rotation) : base(radius, isStatic, pos)
        {
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.Body.Rotate(Rotation);
        }


        public void Update(Game1 game)
        {
            if (game.statsManager.CheckIfDead(this))
            {
                game.aSetter.removeEntity(this);
                return;
            }
        }
    }
}
