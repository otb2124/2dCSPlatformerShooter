using Platformer.Entities.group;
using Platformer.Physics;
using System;
using Platformer.Entities.Types;
using Platformer.Entities.Weapons;


namespace Platformer
{
    public class StatsManager
    {
        private int knockbackCounter = 0;
        private const int knockbackDurationFrames = 1 * 60;

        Game1 game;




        public StatsManager(Game1 game) { this.game = game; }



        public void Update(FlatEntity entity)
        {
            HandlePlayerTouch(entity);
        }



        public void DealFallDamage(LiveEntity lent)
        {
            lent.currentHP -= lent.fallCounter / 2;
            lent.isInvincible = true;
            lent.fallCounter = 0;
        }


        public void HandlePlayerTouch(FlatEntity entity)
        {


            if (Collisions.Collide(game.player.Body, entity.Body, out _, out _))
            {

                if (entity is LiveEntity lent)
                {
                    if (!lent.isDead)
                    {
                        if (lent is Mob)
                        {
                            DealDamage(game.player, (Mob)lent);
                        }
                    }

                }


            }

        }




        public void HandleGroupMemberTouch(GroupMember member, FlatEntity entity)
        {

            if (Collisions.Collide(member.Body, entity.Body, out _, out _))
            {



                if (entity is LiveEntity lent)
                {
                    if (!lent.isDead)
                    {
                        if (lent is Mob)
                        {
                            DealDamage(member, (Mob)entity);
                        }
                    }

                }

            }

        }




        public void DealDamage(LiveEntity receiver, LiveEntity dealer)
        {
            if (!receiver.isInvincible)
            {
                receiver.isInvincible = true;

                float totalDamage = dealer.bodyDamage;

                // Apply armor damage reduction if the receiver has armor
                if (receiver.armor > 0)
                {
                    // Calculate the percentage of damage reduction based on armor
                    float damageReductionPercentage = receiver.armor / 200f;

                    float reducedDamage = totalDamage * damageReductionPercentage;

                    // Apply the reduced damage
                    receiver.currentHP -= Math.Max(totalDamage - reducedDamage, 0);
                }
                else
                {
                    // If the receiver has no armor, apply full damage
                    receiver.currentHP -= totalDamage;
                }

                ApplyKnockback(receiver, dealer);
                ApplyKnockback(dealer, receiver);
            }
        }









        public void HandleProjectileLogic(Projectile proj)
        {


            if (proj.lifeCount <= 0)
            {
                game.entitiesToRemoval.Add(proj);
                game.world.RemoveBody(proj.Body);

            }
            else
            {
                // Check for collisions with other entities
                for (int j = 0; j < game.entities.Count; j++)
                {

                    if (proj != game.entities[j])
                    {
                        ProjectileToHit(proj, game.entities[j]);
                    }

                    if (proj == null)
                    {
                        break;
                    }
                }
            }


        }




        public void ProjectileToHit(Projectile projectile, FlatEntity entity)
        {
            //to skip
            if (!(entity is Projectile) && !(entity is InterractiveItem) && !(entity is LadderEntity) && !(entity is DecorationEntity) && !(entity is PlatformEntity) && !(entity is GroupMember && projectile.weapon.owner is GroupMember) && !(entity is NPC))

                if (Collisions.Collide(projectile.Body, entity.Body, out _, out _))
                {

                    if (entity is DestroyableEntity destroyable)
                    {
                        destroyable.currentHP -= projectile.damage;
                        projectile.lifeCount = 0;
                    }



                    if (!(entity is LiveEntity) && projectile.projectileType != Projectile.ProjectileType.granade)
                    {
                        if (entity is DestroyableEntity)
                        {
                            return;
                        }

                        projectile.lifeCount = 0;

                    }
                    else
                    {


                        if (entity is Mob || (entity is GroupMember && projectile.weapon.owner is Mob))
                        {
                            LiveEntity subject = (LiveEntity)entity;

                            if (projectile.projectileType != Projectile.ProjectileType.hook && projectile.projectileType != Projectile.ProjectileType.granade)
                            {
                                if (subject is Mob)
                                {
                                    game.aiManager.AggroOnClosestGroupMemberIfClose((Mob)subject, 80, 100);
                                }

                                if (!subject.isDead)
                                {
                                    subject.currentHP -= projectile.damage;
                                }
                                else
                                {
                                    subject.corpseDecay += 0.01f;
                                }
                            }
                            else if (projectile.projectileType == Projectile.ProjectileType.hook)
                            {


                                if (!projectile.hasGrabbed)
                                {
                                    projectile.lifeCount = projectile.tempLifeCount / 2;
                                }
                                subject.Body.MoveTo(projectile.Body.Position);
                                projectile.hasGrabbed = true;
                                subject.fallCounter = 0;
                                subject.isFalling = false;
                                if (subject is GroupMember)
                                {
                                    subject.currentHP -= projectile.weapon.dmg;
                                }

                                if (projectile.weapon.owner.isDead)
                                {
                                    projectile.lifeCount = 0;
                                }

                            }
                        }


                        if (!projectile.weapon.isSinglular && projectile.projectileType != Projectile.ProjectileType.granade && projectile.projectileType != Projectile.ProjectileType.hook)
                        {
                            projectile.lifeCount = 0;
                        }
                    }

                }
        }

        public bool CheckIfDead(LiveEntity lent)
        {

            if (lent.currentHP <= 0)
            {
                lent.isDead = true;
                return lent.isDead;
            }

            return false;
        }
        public bool CheckIfDead(DestroyableEntity lent)
        {

            if (lent.currentHP <= 0)
            {
                lent.isDestroyed = true;
                return lent.isDestroyed;
            }

            return false;
        }



        public void ApplyKnockback(LiveEntity receiver, LiveEntity dealer)
        {
            FlatVector knockbackDirection = dealer.Body.Position - receiver.Body.Position;
            float knockbackStr = dealer.knockbackStrength;
            if (dealer == receiver)
            {
                knockbackStr /= 2;
            }

            receiver.Body.ApplyForce(-knockbackDirection * knockbackStr);
            knockbackCounter = knockbackDurationFrames;
        }


        public void UpdateKnockback(LiveEntity receiver)
        {
            if (knockbackCounter > 0)
            {
                knockbackCounter--;
                if (knockbackCounter == 0)
                {
                    FlatVector currentVelocity = receiver.Body.LinearVelocity;
                    receiver.Body.LinearVelocity = new FlatVector(0, currentVelocity.Y);
                }
            }
        }



    }

}

