using Microsoft.Xna.Framework;
using Platformer.Entities.group;
using Platformer.Entities.Types;
using Platformer.Entities.Weapons;
using Platformer.Physics;
using Platformer.Utils;
using System;
using System.Diagnostics;
using static Platformer.Entities.Types.FlatEntity;

namespace Platformer
{
    public class AIManager
    {

        

        private Game1 game;


        public AIManager(Game1 game) { this.game = game; }






        public void Idle(LiveEntity mob)
        {
            // Check if the entity is not already idle
            if (!mob.isIdle)
            {
                // Generate random values for direction and whether to walk or not
                int random = RandomHelper.RandomInteger(0, 100);
                Direction randomDirection = (Direction)RandomHelper.RandomInteger(0, 2);

                // Set idle state to true
                mob.isIdle = true;

                // Reset idle counter
                mob.idleCounter = 0;

                // Store the random direction and random value for later use
                mob.idleRandomDirection = randomDirection;
                mob.idleRandomValue = random;
                mob.currentSpeed = mob.speed;
            }
            else
            {
                mob.idleCounter++;
                mob.currentSpeed = mob.speed / 5;

                // If the random value is less than 50, walk in the stored random direction
                if (mob.idleRandomValue < 50)
                {
                    if(((Mob)mob).thisMobtype == Mob.mobType.WalkingMob || ((Mob)mob).thisMobtype == Mob.mobType.DogMob || ((Mob)mob).thisMobtype == Mob.mobType.WheelMob)
                    {
                        WalkFor(mob, mob.idleRandomDirection, 1);
                    }
                    else if(((Mob)mob).thisMobtype == Mob.mobType.JumpingMob)
                    {
                        JumpFor(mob, 1);
                    }
                    
                }

                // Check if the idle duration has elapsed
                if (mob.idleCounter == 1 * 60)
                {
                    // Set idle state to false
                    mob.isIdle = false;
                }
            }
        }



        public void AdjustGroupMemberPosition(GroupMember currentMember, float targetDistance)
        {
            // Find the player's position
            float playerX = game.player.Body.Position.X;

            // Calculate the target position for the current GroupMember based on the player's position
            float targetX = playerX + targetDistance; // Place the first GroupMember next to the player

            // Calculate the index of the current GroupMember in the group
            int currentIndex = game.group.CurrentGroup.IndexOf(currentMember);

            if(currentIndex > 1)
            {
                currentIndex++;
            }
            // Adjust the target position based on the index to create a difference of 5f between each GroupMember
            targetX += (currentIndex - 3) * targetDistance;

            // Calculate the distance between the current GroupMember and the target position
            float distanceToTarget = Math.Abs(currentMember.Body.Position.X - targetX);

            // Check if the current GroupMember needs to move
            if (distanceToTarget > 0.1f) // Adjust this threshold as needed
            {
                // Calculate the direction of movement
                Direction direction = (currentMember.Body.Position.X < targetX) ? Direction.Right : Direction.Left;

                WalkFor(currentMember, direction, 1);
            }
            else
            {
                // Stop walking if the GroupMember is close enough to the target position
                currentMember.IsWalking = false;
                currentMember.WalkingDirection = "";
            }
        }









        public void FlyAndMove(LiveEntity mob, Direction direction, int flyInterval)
        {

            Fly(mob, flyInterval);


            if (mob.Body.LinearVelocity.Y > 0.2f || mob.Body.LinearVelocity.Y < -0.2f)
            {
                WalkFor(mob, direction, (int)mob.currentSpeed / 2);
            }
        }






        public void JumpAndMoveInAirFor(LiveEntity mob, Direction direction, int jumpInterval)
        {

            JumpFor(mob, jumpInterval);

            if(mob.Body.LinearVelocity.Y > 0.2f || mob.Body.LinearVelocity.Y < -0.2f)
            {
                WalkFor(mob, direction, jumpInterval);
            }
           
        }




        public void JumpFor(LiveEntity liveEntity, int timeSec)
        {
            if (!liveEntity.IsJumping && liveEntity.jumpCounter == 0)
            {
                // Apply jump force to the entity's body
                liveEntity.Body.Jump(liveEntity.currentJumpForce);

                // Set initial jumping state
                liveEntity.IsJumping = true;

                // Reset jump counter
                liveEntity.jumpCounter = 1;
            }
            else if (liveEntity.IsJumping)
            {
                // Increment jump counter
                liveEntity.jumpCounter++;

                // Check if the jump duration has elapsed
                if (liveEntity.jumpCounter >= timeSec * 60)
                {
                    // Ensure that the entity's jumping state is reset after the jump is complete
                    liveEntity.IsJumping = false;

                    // Reset jump counter
                    liveEntity.jumpCounter = 0;
                }
            }
        }




        public void Fly(LiveEntity liveEntity, int level)
        {
            if (!liveEntity.isFlying && liveEntity.flyCounter == 0)
            {
                // Apply an upward force to the entity's body
                float upwardForce = liveEntity.currentJumpForce;
                liveEntity.Body.ApplyForce(new FlatVector(0, upwardForce));

                // Set initial flying state
                liveEntity.isFlying = true;

                // Reset fly counter
                liveEntity.flyCounter = 1;
            }
            else if (liveEntity.isFlying)
            {
                // Increment fly counter
                liveEntity.flyCounter++;


                // Apply a constant upward force to keep the entity in the air
                float constantUpwardForce = liveEntity.currentFlyForce; // Adjust this value according to your game's requirements
                liveEntity.Body.ApplyForce(new FlatVector(0, constantUpwardForce));

                // Check if the fly duration has elapsed
                if (liveEntity.flyCounter >= level * 60)
                {
                    // Ensure that the entity's flying state is reset after the fly is complete
                    liveEntity.isFlying = false;

                    // Reset fly counter
                    liveEntity.flyCounter = 0;
                }
                if (liveEntity.flyCounter <= level * 10 && liveEntity.flyCounter < level * 60)
                {

                    liveEntity.Body.ApplyForce(new FlatVector(0, 1f));
                }
            }
        }






        public void Jump(LiveEntity lent, float jumpForce)
        {

                lent.Body.Jump(jumpForce);
            

        }




        public void AggroOnClosestGroupMemberIfClose(Mob mob, float aggroDistance, float unAggroDistance)
        {

            // Get the closest group member within the specified aggro distance
            GroupMember closestGroupMember = (GroupMember)GetClosest(mob, typeof(GroupMember));

            if (closestGroupMember != null)
            {
                // Calculate the distance between the mob and the closest group member
                float distanceToGroupMember = FlatMath.Distance(mob.Body.Position, closestGroupMember.Body.Position);

                // Check if the distance is within the aggro distance threshold
                if (distanceToGroupMember <= aggroDistance)
                {
                    // Jump until Y position matches the group member's Y position
                    mob.isAgro = true;
                }
                else if (distanceToGroupMember > unAggroDistance)
                {
                    // If the mob is beyond the unAggro distance, stop being aggressive
                    mob.isAgro = false;
                }



                if (mob.isAgro)
                {
                    // Calculate the direction vector from the mob to the group member
                    FlatVector directionToGroupMember = closestGroupMember.Body.Position - mob.Body.Position;

                    // Normalize the direction vector to get a unit vector
                    directionToGroupMember = FlatMath.Normalize(directionToGroupMember);

                    // Set the direction for the mob to move towards the group member
                    mob.direction = directionToGroupMember;

                    // Determine the direction based on the x-component of the direction vector
                    Direction direction = (directionToGroupMember.X > 0) ? Direction.Right : Direction.Left;

                    // Move towards the group member
                    if (mob.thisMobtype == Mob.mobType.JumpingMob)
                    {
                        JumpAndMoveInAirFor(mob, direction, (int)mob.currentSpeed);
                    }
                    else if (mob.thisMobtype == Mob.mobType.WalkingMob || mob.thisMobtype == Mob.mobType.WheelMob)
                    {
                        WalkFor(mob, direction, 1);
                        JumpTillYMatches(mob, closestGroupMember, mob.currentJumpForce);
                    }
                    else if (mob.thisMobtype == Mob.mobType.DogMob)
                    {
                        if (distanceToGroupMember <= 10f)
                        {
                            JumpAndMoveInAirFor(mob, direction, (int)mob.currentSpeed);
                        }
                        else
                        {
                            WalkFor(mob, direction, 1);
                            JumpTillYMatches(mob, closestGroupMember, mob.currentJumpForce);
                        }
                    }
                    
                    else if (mob.thisMobtype == Mob.mobType.FlyingMob)
                    {
                        FlyAndMove(mob, direction, 3);
                    }
                    else if (mob.thisMobtype == Mob.mobType.ShootingMob)
                    {
                        if (distanceToGroupMember <= 10f)
                        {
                            WalkFor(mob, direction, -1);
                        }
                        else
                        {
                            WalkFor(mob, direction, 1);
                            JumpTillYMatches(mob, closestGroupMember, mob.currentJumpForce);
                        }
                        SetDirectionToClosestInRadiusAndShootTillDead(mob, typeof(GroupMember), aggroDistance*2);
                    }
                    else if(mob.thisMobtype == Mob.mobType.GrabbingMob)
                    {
                        if (distanceToGroupMember <= 10f)
                        {
                            SetDirectionToClosestInRadiusAndShootTillDead(mob, typeof(GroupMember), 10f);
                        }
                    }
                }
            }
            else
            {
                // No group members found, stop moving
                mob.IsWalking = false;
                mob.isAgro = false;
            }
        }






        public void AggroOnPlayerIfClose(Mob mob, float aggroDistance)
        {
            // Calculate the distance between the mob and the player
            float distanceToPlayer = FlatMath.Distance(mob.Body.Position, game.player.Body.Position);

            // Check if the distance is within the aggro distance threshold
            if (distanceToPlayer <= aggroDistance)
            {
                // Calculate the direction vector from the mob to the player
                FlatVector directionToPlayer = game.player.Body.Position - mob.Body.Position;

                // Normalize the direction vector to get a unit vector
                directionToPlayer = FlatMath.Normalize(directionToPlayer);

                // Set the direction for the mob to move towards the player
                mob.direction = directionToPlayer;

                // Determine the direction based on the x-component of the direction vector
                Direction direction = (directionToPlayer.X > 0) ? Direction.Right : Direction.Left;

                if (mob.thisMobtype == Mob.mobType.JumpingMob)
                {
                    JumpAndMoveInAirFor(mob, direction, 1);
                }
                else
                {
                    WalkFor(mob, direction, 1);
                }


                JumpTillYMatches(mob, game.player, 2f);

                mob.isAgro = true;
            }
            else
            {
                // Stop moving if the player is not within the aggro distance
                mob.IsWalking = false;
                mob.isAgro = false;
            }
        }


        public void JumpTillYMatches(LiveEntity liveEntity, LiveEntity target, float threshold)
        {
            // Check if the player's Y position is higher by the specified threshold compared to the liveEntity
            if (target.Body.Position.Y - liveEntity.Body.Position.Y > threshold)
            {
                // Start jumping continuously until the liveEntity's Y position matches the player's Y position
                Jump(liveEntity, liveEntity.currentJumpForce); // You need to define the jumpForce or pass it as a parameter
            }

        }









        public void FollowIfFar(LiveEntity entity, LiveEntity target, float followDistance)
        {
            // Calculate the distance between the entity and the player
            float distanceToPlayer = FlatMath.Distance(entity.Body.Position, target.Body.Position);

            // Check if the distance exceeds the follow distance threshold
            if (distanceToPlayer > followDistance)
            {
                // Calculate the direction vector from the entity to the player
                FlatVector directionToPlayer = target.Body.Position - entity.Body.Position;

                // Normalize the direction vector to get a unit vector
                directionToPlayer = FlatMath.Normalize(directionToPlayer);

                // Determine the direction based on the x-component of the direction vector
                Direction direction = (directionToPlayer.X > 0) ? Direction.Right : Direction.Left;

                // If the entity was moving towards the target, maintain its direction
                if (entity.direction.X == directionToPlayer.X && entity.direction.Y == directionToPlayer.Y)
                {
                    // Move the entity towards the player
                    WalkFor(entity, direction, 1);
                }
                else
                {
                    // Set the direction for the entity to face the target
                    entity.direction = directionToPlayer;

                    // Move the entity towards the player
                    WalkFor(entity, direction, 1);
                }

                JumpTillYMatches(entity, target, 2f);
            }
            else
            {
                // Stop walking if the entity is within the follow distance
                entity.IsWalking = false;
            }
        }









        public void StandInfrontIfFar(LiveEntity entity, LiveEntity target, float followDistance)
        {
            // Calculate the distance between the entity and the target
            float distanceToTarget = FlatMath.Distance(entity.Body.Position, target.Body.Position);

            // Check if the distance exceeds the follow distance threshold
            if (distanceToTarget > followDistance)
            {


            }
        }








        public void SetDirectionToClosestInRadiusAndShootTillDead(LiveEntity entity, Type targetClass, float distance)
        {
            


            FlatEntity target = GetClosest(entity, targetClass);

            // Set the direction to the closest target
            setDirectionToVisible(entity, target, distance);

            // Check if the target is a LiveEntity and alive
            if (target is LiveEntity liveTarget && !liveTarget.isDead)
            {
                // Check if the entity has a valid direction to shoot
                if (entity.direction.X != 0 || entity.direction.Y != 0)
                {
                    // Shoot continuously until the target is dead
                    Shoot(entity);
                }
                else
                {
                    StopShooting(entity);
                }

            }
            else
            {
                // If the target is dead or not alive, stop shooting
                StopShooting(entity);
            }
        }






        public void SetDirectionToClosestAndShootTillDead(LiveEntity entity, Type targetClass)
        {
            FlatEntity target = GetClosest(entity, targetClass);

            // Set the direction to the closest target
            setDirectionTo(entity, target);

            // Check if the target is a LiveEntity and alive
            if (target is LiveEntity liveTarget && !liveTarget.isDead)
            {
                // Check if the entity has a valid direction to shoot
                if (entity.direction.X != 0 || entity.direction.Y != 0)
                {
                    // Shoot continuously until the target is dead
                    Shoot(entity);
                }
                else
                {
                    StopShooting(entity);
                }

            }
            else
            {
                // If the target is dead or not alive, stop shooting
                StopShooting(entity);
            }
        }






        public void SetDirectionToClosestAndShootFor(LiveEntity entity, Type entityClass, int timeSec)
        {
            FlatEntity target = GetClosest(entity, entityClass);

            setDirectionTo(entity, target);


            if (entity.currentWeapon != null)
            {
                ShootFor(entity, timeSec);
            }
        }





        public void setDirectionToVisible(LiveEntity entity, FlatEntity closest, float minDistance)
        {
            FlatEntity target = closest;

            if (target != null && target is LiveEntity)
            {
                // Calculate the direction vector from the entity to the target
                FlatVector directionToTarget = target.Body.Position - entity.Body.Position;

                // Check if the target is within the minimum distance
                if (FlatConverter.ToVector2(directionToTarget).Length() < minDistance)
                {
                    // Normalize the direction vector to get a unit vector
                    directionToTarget = FlatMath.Normalize(directionToTarget);
                    directionToTarget = new FlatVector(FlatMath.Clamp(directionToTarget.X, -1f, 1f), FlatMath.Clamp(directionToTarget.Y, -1f, 1f));

                    LiveEntity ent = (LiveEntity)target;

                    if (!ent.isDead)
                    {
                        entity.direction = directionToTarget;
                    }
                }
                else
                {
                    // If the target is beyond the minimum distance, set direction to zero
                    entity.direction = FlatVector.Zero;
                }
            }
            else
            {
                // If no valid target found, set direction to zero
                entity.direction = FlatVector.Zero;
            }
        }





        public void setDirectionTo(LiveEntity entity, FlatEntity closest)
        {
            FlatEntity target = closest;

            if (target != null && target is LiveEntity)
            {
                // Calculate the direction vector from the entity to the target
                FlatVector directionToTarget = target.Body.Position - entity.Body.Position;

                // Normalize the direction vector to get a unit vector
                directionToTarget = FlatMath.Normalize(directionToTarget);
                directionToTarget = new FlatVector(FlatMath.Clamp(directionToTarget.X, -1f, 1f), FlatMath.Clamp(directionToTarget.Y, -1f, 1f));


                LiveEntity ent = (LiveEntity)target;

                if (!ent.isDead)
                {
                    entity.direction = directionToTarget;
                }



            }
        }






        public FlatEntity GetClosest(FlatEntity from, Type entityClass)
        {
            FlatEntity closestEntity = null;
            float closestDistanceSquared = float.MaxValue;

            foreach (var entity in game.entities)
            {
                // Check if the entity is an instance of the specified entity class
                if (entityClass.IsInstanceOfType(entity))
                {
                    // Calculate the squared distance between the 'from' entity and the current entity
                    float distanceSquared = Vector2.DistanceSquared(FlatConverter.ToVector2(from.Body.Position), FlatConverter.ToVector2(entity.Body.Position));

                    // If the distance is closer than the current closest, update the closest entity and distance
                    if (distanceSquared < closestDistanceSquared)
                    {
                        closestEntity = entity;
                        closestDistanceSquared = distanceSquared;
                    }
                }
            }

            return closestEntity;
        }



        public void ShootFor(LiveEntity entity, int timeSec)
        {
            if (!entity.isShooting)
            {
                // Set shooting state to true
                entity.isShooting = true;
                entity.shootingFrames = timeSec * 60; // Assuming 60 FPS

                // Reset shooting counter
                entity.shootingCounter = 0;

                // Perform shooting action

                // After shooting duration elapses, set shooting state to false
                entity.isShooting = true;
            }
            else
            {
                entity.shootingCounter++;

                if (entity.shootingCounter >= entity.shootingFrames)
                {
                    entity.isShooting = false;
                    entity.shootingCounter = 0;
                }
            }

        }

        public void StopShooting(LiveEntity entity)
        {
            // Set shooting state to false
            entity.isShooting = false;

            // Reset shooting counter
            entity.shootingCounter = 0;
        }


        public void Shoot(LiveEntity entity)
        {
            // Set shooting state to true
            entity.isShooting = true;

        }








        public void WalkFor(LiveEntity liveEntity, Direction direction, int timeSec)
        {
            if (!liveEntity.IsWalking)
            {
                // Adjust walk speed as needed
                liveEntity.walkSpeed = liveEntity.currentSpeed / 6;

                // Calculate total walk time in frames
                liveEntity.walkTime = timeSec * 60;


                // Set initial walking direction
                liveEntity.WalkingDirection = direction.ToString().ToLower();

                // Start walking
                liveEntity.IsWalking = true;

                // Reset walk counter
                liveEntity.walkCounter = 0;
            }
            else
            {
                // Increment walk counter
                liveEntity.walkCounter++;

                // Determine the direction multiplier based on the direction
                int directionMultiplier = (direction == Direction.Right) ? 1 : -1;

                // Calculate the distance to move based on elapsed time and walk speed
                float distanceToMove = liveEntity.walkSpeed * directionMultiplier;

                // Update the entity's position
                liveEntity.Body.MoveTo(new FlatVector(liveEntity.Body.Position.X + distanceToMove, liveEntity.Body.Position.Y));

                // Check if the walk time has elapsed
                if (liveEntity.walkCounter >= liveEntity.walkTime)
                {
                    // Stop walking
                    liveEntity.IsWalking = false;
                    liveEntity.WalkingDirection = "";
                }
            }
        }

    }
}
