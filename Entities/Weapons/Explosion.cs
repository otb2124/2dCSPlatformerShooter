using Microsoft.Xna.Framework;
using Platformer.Physics;
using System.Runtime.CompilerServices;

namespace Platformer.Entities.Weapons
{
    public class Explosion : Projectile
    {


        public float explosionSpeed;

        public Explosion(float radius, FlatVector pos, Weapon weapon, Color color, float explosionSpeed) : base(radius, pos, true, weapon, ProjectileType.explosion, color)
        {
            this.lifeCount = 30;
            this.explosionSpeed = explosionSpeed;
            this.Body.Mass = 0.1f;
            this.Body.Density = 1f;
        }


    }
}
