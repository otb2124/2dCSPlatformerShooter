using Microsoft.Xna.Framework;
using Platformer.Physics;
using System;



namespace Platformer.Entities.Types
{
    public class Mob : LiveEntity
    {

        public enum mobType
        {
            JumpingMob,
            WalkingMob,
            FlyingMob,
            DogMob,
            WheelMob,
            ShootingMob,
            GrabbingMob,
            SlayerMob,

            Unknown
        }

        public float AgroRange, unAgroRange;
        public Boolean isAgro = false;
        public mobType thisMobtype;

        public Mob(float width, float height, float maxHP, float currentHP, float dmg, Boolean isAgro, mobType thisMobtype, FlatVector pos) : base(width, height, maxHP, currentHP, pos)
        {
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.isAgro = isAgro;
            this.bodyDamage = dmg;
            this.jumpForce = 1f;
            this.currentJumpForce = jumpForce;
            this.thisMobtype = thisMobtype;
            this.AgroRange = 10;
            this.unAgroRange = 50;
            this.Body.owner = this;

            if(thisMobtype == mobType.JumpingMob)
            {
                speed = 5;
                currentSpeed = 5;
            }
            else if (thisMobtype == mobType.FlyingMob)
            {
                speed = 5;
                currentSpeed = 5;
            }
            else if (thisMobtype == mobType.DogMob)
            {
                speed = 5;
                currentSpeed = 5;
            }
            else if(thisMobtype == mobType.ShootingMob)
            {
                weapons = new Weapon[2];
                weapons[0] = new Weapon(1, 0.5f, 30, 60, 20, 120, 0.1f, Weapons.Projectile.ProjectileType.bullet, this);
                weapons[1] = new Weapon(1, 0.5f, 30, 60, 20, 120, 0.1f, Weapons.Projectile.ProjectileType.bullet, this);
                currentWeapon = weapons[0];
            }
            else if (thisMobtype == mobType.GrabbingMob)
            {
                weapons = new Weapon[2];
                weapons[0] = new Weapon(0.1f, 1f, 30, 120, 1, 120, 0.1f, Weapons.Projectile.ProjectileType.hook, this);
                weapons[1] = new Weapon(1, 1f, 30, 120, 1, 120, 0.1f, Weapons.Projectile.ProjectileType.hook, this);
                currentWeapon = weapons[0];
                Body.IsStatic = true;
                AgroRange = 30;
            }
        }


        public Mob(float radius, float maxHP, float currentHP, float dmg, Boolean isAgro, mobType thisMobtype, FlatVector pos) : base(radius, maxHP, currentHP, pos)
        {
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.isAgro = isAgro;
            this.bodyDamage = dmg;
            this.jumpForce = 1f;
            this.currentJumpForce = jumpForce;
            this.thisMobtype = thisMobtype;
            this.AgroRange = 10;
            this.unAgroRange = 50;
            this.speed = 2;
            this.currentSpeed = 2;
            this.Body.owner = this;
        }


        public void Update(Game1 game)
        {

            if (!game.statsManager.CheckIfDead(this))
            {
                if (!isAgro)
                {
                    this.Color = this.oldColor;
                    game.aiManager.Idle(this);
                }
                else
                {
                    this.Color = Color.Red;
                }


                game.aiManager.AggroOnClosestGroupMemberIfClose(this, AgroRange, unAgroRange);
            }
           
            
            

            base.Update(game);
        }


        public static mobType ConvertToMobType(string input)
        {
            switch (input)
            {
                case "JumpingMob":
                    return mobType.JumpingMob;
                case "WalkingMob":
                    return mobType.WalkingMob;
                case "DogMob":
                    return mobType.DogMob;
                case "WheelMob":
                    return mobType.WheelMob;
                case "FlyingMob":
                    return mobType.FlyingMob;
                case "GrabbingMob":
                    return mobType.GrabbingMob;
                case "ShootingMob":
                    return mobType.ShootingMob;
                case "SlayerMob":
                    return mobType.SlayerMob;
                default:
                    return mobType.Unknown;
            }

        }

    }
}
