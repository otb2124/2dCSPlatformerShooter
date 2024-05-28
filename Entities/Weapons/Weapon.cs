using Microsoft.Xna.Framework;
using Platformer.Entities.group;
using Platformer.Entities.items;
using Platformer.Entities;
using Platformer.Physics;
using SharpDX.MediaFoundation.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Platformer.Entities.Types;
using Platformer.Entities.Weapons;
using System.Windows.Forms;

namespace Platformer.Entities
{
    public class Weapon : Item
    {
        

        public float dmg;
        public Projectile projectile;
        public LiveEntity owner;
        public float bulletSpeed;

        public float ammoAmount;
        public float currentAmmoAmount;


        public bool isReloading = false;


        public float frequency;
        public float frequencyCounter;

        public float reload;
        public float reloadCounter;

        public float bulletLifecount;


        public float spray;
        public float currentSpray;

        public bool isSinglular = false;
        public bool projectileIsAdded = false;

        public Projectile.ProjectileType projectileT;


        public Weapon(float dmg, float bulletSpeed, float bulletFrequency, float bulletLifecount, float ammoAmount, float reloadSpeed, float spray, Projectile.ProjectileType projectileType, LiveEntity owner)
        {
            
            this.bulletSpeed = bulletSpeed;
            this.ammoAmount = ammoAmount;
            this.currentAmmoAmount = ammoAmount;
            this.dmg = dmg;
            this.frequency = bulletFrequency;
            this.bulletLifecount = bulletLifecount;
            this.reload = reloadSpeed;
            this.spray = spray;

            this.isStackable = false;
            this.amount = 1;

            this.owner = owner;
            this.projectileT = projectileType;
        }




        public void setProjectile(Game1 game)
        {
            // Adjust the offset based on the shooter's direction
            float offsetX = 1.5f;
            if (owner.direction.X < 0)
            {
                // If the shooter is facing left, adjust the offsetX to a negative value
                offsetX *= -1;
            }

            // Calculate the position of the projectile relative to the shooter
            FlatVector pos = new FlatVector(owner.Body.Position.X + offsetX, owner.Body.Position.Y);

            if (projectileT == Projectile.ProjectileType.ray)
            {
                isSinglular = true;
                projectile = new Projectile(2f, 1f, pos, true, this, projectileT, Color.Blue);
            }
            else if(projectileT == Projectile.ProjectileType.bullet)
            {
                projectile = new Projectile(0.1f, pos, true, this, projectileT, Color.Yellow);
            }
            else if (projectileT == Projectile.ProjectileType.flamethrower)
            {
                projectile = new Projectile(1f, pos, true, this, projectileT, Color.Orange);
            }
            else if (projectileT == Projectile.ProjectileType.granade)
            {
                projectile = new Projectile(0.3f, pos, false, this, projectileT, Color.DarkGreen);
            }
            else if (projectileT == Projectile.ProjectileType.hook)
            {
                if(projectile != null)
                {
                    if(projectile.lifeCount > 0)
                    {
                        if (projectile.hasGrabbed)
                        {
                            projectile.lifeCount = -1;
                            projectile.hasGrabbed = false;
                            return;
                        }
                    }
                    
                }
                projectile = new Projectile(1f, 1f, pos, true, this, projectileT, Color.Gray);
            }

            game.world.AddBody(projectile.Body);
            game.entities.Add(projectile);
        }



        public void Update(Game1 game)
        {

            if (!isSinglular)
            {
                if (owner.isShooting)
                {
                    frequencyCounter--;
                }
                else
                {
                    frequencyCounter = frequency;
                }
            }
            else
            {
                if (owner.isShooting)
                {
                    if (!projectileIsAdded)
                    {
                        setProjectile(game);
                        projectileIsAdded = true;
                    }
                    
                }
                else
                {
                    if (projectileIsAdded)
                    {
                        projectile.lifeCount = 0;
                        projectileIsAdded = false;
                    }
                    
                }
            }
            



            if (isReloading)
            {
                reloadCounter++;

                if (reloadCounter == reload)
                {

                    reloadCounter = 0;
                    currentAmmoAmount = ammoAmount;
                    isReloading = false;
                }
            }

            if (currentAmmoAmount <= 0)
            {
                isReloading = true;
            }
            else
            {


                if(!isSinglular)
                {
                    if (frequencyCounter <= 0)
                    {
                        setProjectile(game);
                        currentAmmoAmount--;
                        frequencyCounter = frequency;
                    }
                }
                
            }
        }


    }
}
