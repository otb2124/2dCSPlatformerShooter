using Microsoft.Xna.Framework;
using Platformer.Entities.Types;
using Platformer.Physics;
using Platformer.Utils;
using System;


namespace Platformer.Entities.Weapons
{
    public class Projectile : FlatEntity
    {


        public float lifeCount;
        public float damage;
        public float projectileSpeed;

        private bool hasInitialDirection = false;
        private FlatVector direction;
        private FlatVector modifiedDirection;

        public Weapon weapon;
        public ProjectileType projectileType;

        public float tempLifeCount;
        public bool hasGrabbed;

        public enum ProjectileType
        {
            bullet,
            ray,
            flamethrower,
            granade,
            explosion,
            hook
        }


        public Projectile(float radius, FlatVector pos, bool isStatic, Weapon weapon, ProjectileType type, Color color) : base(radius, pos, isStatic, color)
        {
            lifeCount = weapon.bulletLifecount;
            tempLifeCount = lifeCount;
            damage = weapon.dmg;
            Body.owner = this;
            projectileSpeed = weapon.bulletSpeed;
            Body.LinearVelocity = FlatVector.Zero;
            projectileType = type;
            this.weapon = weapon;
        }

        public Projectile(float width, float height, FlatVector pos, bool isStatic, Weapon weapon, ProjectileType type, Color color) : base(width, height, pos, isStatic, color)
        {
            lifeCount = weapon.bulletLifecount;
            tempLifeCount = lifeCount;
            damage = weapon.dmg;
            Body.owner = this;
            projectileSpeed = weapon.bulletSpeed;
            Body.LinearVelocity = FlatVector.Zero;

            projectileType = type;
            this.weapon = weapon;
        }



        public new void Update(Game1 game)
        {







            if(projectileType == ProjectileType.bullet)
            {
                //movement and spray
                if (!hasInitialDirection)
                {
                    direction = weapon.owner.direction;
                    float sprayAngle = RandomHelper.RandomSingle(-weapon.currentSpray, weapon.currentSpray);
                    modifiedDirection = FlatMath.RotateVector(direction, sprayAngle);

                    hasInitialDirection = true;
                }

                Body.MoveTo(new FlatVector(Body.Position.X + projectileSpeed * modifiedDirection.X,
                                            Body.Position.Y + projectileSpeed * modifiedDirection.Y));

                //lifecount
                lifeCount--;
            }
            else if(projectileType == ProjectileType.flamethrower)
            {
                if (!hasInitialDirection)
                {
                    direction = weapon.owner.direction;
                    float sprayAngle = RandomHelper.RandomSingle(-weapon.currentSpray, weapon.currentSpray);
                    modifiedDirection = FlatMath.RotateVector(direction, sprayAngle);

                    hasInitialDirection = true;
                }

                float newRadius = Body.Radius + 0.1f;
                Body.MoveTo(new FlatVector(Body.Position.X + projectileSpeed * modifiedDirection.X,
                                            Body.Position.Y + projectileSpeed * modifiedDirection.Y));
                this.Body.Radius = newRadius;

                //lifecount
                lifeCount--;
            }
            else if (projectileType == ProjectileType.ray)
            {
                direction = weapon.owner.direction;
                float newWidth = Body.Width;

                if(lifeCount > 0)
                {
                    newWidth = Body.Width + projectileSpeed;
                }

                FlatVector newPos = new FlatVector(
                   weapon.owner.Body.Position.X + (newWidth / 2) * direction.X,
                   weapon.owner.Body.Position.Y + (newWidth / 2) * direction.Y
               );

                Body.MoveTo(newPos);

                Body.RotateTo((float)Math.Atan2(direction.Y, direction.X));
                this.Body.Width = newWidth;

                game.world.RemoveBody(Body);
                Body = new FlatBody(Body, Body.Height, Body.Width);
                Body.owner = this;
                game.world.AddBody(Body);

                //lifecount
                lifeCount--;
            }
            else if (projectileType == ProjectileType.granade)
            {

                if (lifeCount <= 1)
                {
                    Explosion explosion = new Explosion(0.1f, this.Body.Position, this.weapon, Color.Orange, 0.4f);
                    game.world.AddBody(explosion.Body);
                    game.entities.Add(explosion);
                }
                
                //movement and spray
                if (!hasInitialDirection)
                {
                    direction = weapon.owner.direction;
                    float sprayAngle = RandomHelper.RandomSingle(-weapon.currentSpray, weapon.currentSpray);
                    modifiedDirection = FlatMath.RotateVector(direction, sprayAngle);
                    hasInitialDirection = true;
                }


                if(lifeCount > tempLifeCount / 2)
                {
                    Body.MoveTo(new FlatVector(Body.Position.X + projectileSpeed * modifiedDirection.X,
                                           Body.Position.Y + projectileSpeed * modifiedDirection.Y));
                }
                

                //lifecount
                lifeCount--;
            }
            else if(projectileType == ProjectileType.explosion)
            {
                Explosion explosion = this as Explosion;

                if (explosion.lifeCount <= 0)
                {

                }
                else
                {
                    this.Body.Radius += explosion.explosionSpeed;
                }

                explosion.lifeCount--;
            }
            else if (projectileType == ProjectileType.hook)
            {
                if (!hasInitialDirection)
                {
                    direction = weapon.owner.direction;
                    float sprayAngle = RandomHelper.RandomSingle(-weapon.currentSpray, weapon.currentSpray);
                    modifiedDirection = FlatMath.RotateVector(direction, sprayAngle);

                    hasInitialDirection = true;
                }

                if (lifeCount == tempLifeCount / 2)
                {
                    // Calculate the direction towards the owner's position
                    FlatVector toOwner = weapon.owner.Body.Position - Body.Position;
                    modifiedDirection = FlatMath.Normalize(toOwner);
                }


                FlatVector newPosition = new FlatVector(Body.Position.X + projectileSpeed * modifiedDirection.X,
                                               Body.Position.Y + projectileSpeed * modifiedDirection.Y);


                if (lifeCount <= tempLifeCount / 2)
                {
                    float distanceToOwner = FlatMath.Distance(Body.Position, weapon.owner.Body.Position);
                    if (distanceToOwner <= weapon.owner.Body.Width*3f)
                    {
                        if (!hasGrabbed)
                        {
                            lifeCount = 0;
                        }
                        else {
                            lifeCount = 2;
                            newPosition = new FlatVector(weapon.owner.Body.Position.X + weapon.owner.Body.Width*2 * weapon.owner.direction.X,
                                               weapon.owner.Body.Position.Y + weapon.owner.Body.Height * weapon.owner.direction.Y);
                            projectileSpeed = 0;
                        }
                            
                        
                    }
                    else
                    {
                        lifeCount++;
                    }
                }

                


                Body.MoveTo(newPosition);
                lifeCount--;
                
            }






        }








    }
}
