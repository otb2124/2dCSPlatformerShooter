using Platformer.Entities.items;
using Platformer.Physics;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;

namespace Platformer.Entities.Types
{
    public class LiveEntity : DynamicEntity
    {
        public float maxHP;
        public float currentHP;
        public float corpseDecay;

        public float armor;

        public float speed = 1;
        public float currentSpeed = 1;
        public float jumpForce = 1f;
        public float currentJumpForce = 1f;
        public float flyForce = 1f;
        public float currentFlyForce = 1f;

        public bool isInvincible = false;
        public int invincibleFrames = 1 * 60;
        public int invincibleCounter = 0;
        public Color oldColor;

        public FlatVector direction;


        public List<Item> inventory;
        public Weapon[] weapons;
        public Weapon currentWeapon;
        public bool SecondpWeapon = false;


        //Body Damage
        public float bodyDamage;
        public float knockbackStrength = 30f;



        //Current Status
        public bool isShooting = false;
        public bool isCrouching = false;
        public bool wasCrouching = false;
        public bool isSprinting = false;
        public bool wasSprinting = false;

        public float bodyCrouchHeight;
        public float bodyCrouchY;


        //isDead
        public bool isDead = false;
        public bool isDying = false;



        //Ladder/Platform
        public bool isOnLadder = false;
        public bool isOnPlatform = false;


        //fallDamage
        public float CurrentFall;
        public float FallLimit = -30;
        public bool isFalling = false;
        public float fallCounter = 0;


        //aiManager
        public bool IsWalking = false;
        public string WalkingDirection = "";

        public float walkSpeed;

        public bool IsJumping = false;
        public bool isFlying = false;
        public bool isIdle = false;

        public int walkTime;
        public int walkCounter;

        public int shootingFrames;
        public int shootingCounter;

        public int jumpCounter;

        public int flyCounter;

        public int idleCounter;


        public Direction idleRandomDirection;
        public int idleRandomValue;



        public LiveEntity(float width, float height, float maxHP, float currentHP, FlatVector pos) : base(width, height, pos, 0)
        {
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.oldColor = Color;
            this.corpseDecay = 0;
        }

        public LiveEntity(float radius, float maxHP, float currentHP, FlatVector pos) : base(radius, pos, 0)
        {
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.oldColor = Color;
            this.corpseDecay = 0;
        }



        public void Update(Game1 game)
        {

            
            //check if dead
            if (game.statsManager.CheckIfDead(this))
            {

                if (!isDying)
                {
                    this.Color = Color.DarkGray;
                    this.Body.ApplyForce(new FlatVector(-direction.X * currentJumpForce*50, currentJumpForce*50));
                    isDying = true;
                    currentJumpForce = 0;
                    currentSpeed = 0;
                }


                if (this.Body.Height > 0.1f)
                {
                    this.Body.Height -= corpseDecay + 0.001f;
                    game.world.RemoveBody(Body);
                    Body = new FlatBody(Body, this.Body.Height, Body.Width);
                    Body.owner = this;
                    game.world.AddBody(Body);
                    corpseDecay = 0;
                }
                else if(this.Body.Height <= 0.1f)
                {
                    game.world.RemoveBody(this.Body);
                    game.entities.Remove(this);
                    game.uiManager.refresh();
                }


                return;
            }





            //fallDamage
            CurrentFall = Body.LinearVelocity.Y;

            if(CurrentFall < FallLimit)
            {
                isFalling = true;
                fallCounter++;
            }
            else
            {

                if (isFalling)
                {
                    game.statsManager.DealFallDamage(this);
                    isFalling = false;
                }

            }




            //Invincibility
            if (isInvincible)
            {
                this.Color = Color.White;
                invincibleCounter++;

                // Check if the invincibility period has elapsed
                if (invincibleCounter >= invincibleFrames)
                {
                    // Reset invincibility state and counter
                    isInvincible = false;
                    this.Color = this.oldColor;
                    invincibleCounter = 0;
                }
            }







            if (weapons != null)
            {


                currentWeapon.Update(game);






                if (SecondpWeapon)
                {
                    currentWeapon = weapons[1];
                }
                else
                {
                    currentWeapon = weapons[0];
                }







                if (isCrouching != wasCrouching)
                {
                    // Crouching state has changed since last frame
                    if (isCrouching)
                    {
                        game.world.RemoveBody(Body);
                        Body = new FlatBody(Body, bodyCrouchHeight, Body.Width);
                        Body.owner = this;
                        game.world.AddBody(Body);
                    }
                    else
                    {
                        game.world.RemoveBody(Body);
                        Body = new FlatBody(Body, bodyCrouchHeight * 2, Body.Width);
                        Body.owner = this;
                        game.world.AddBody(Body);
                        currentSpeed = speed;
                    }

                    // Update the previous crouching state
                    wasCrouching = isCrouching;
                }

                if (isCrouching)
                {
                    currentSpeed = speed / 3;
                    weapons[0].currentSpray = weapons[0].spray / 3;
                    weapons[1].currentSpray = weapons[1].spray / 3;
                    isSprinting = false;
                }
                else
                {
                    weapons[0].currentSpray = weapons[0].spray;
                    weapons[1].currentSpray = weapons[1].spray;
                }




                if (isSprinting != wasSprinting)
                {
                    //stopped sprinting
                    if (!isSprinting)
                    {
                        currentSpeed = speed;
                    }

                    wasSprinting = isSprinting;
                }
                if (isSprinting)
                {
                    currentSpeed = speed * 1.5f;
                    isCrouching = false;
                }

            }
        }

    }
}
