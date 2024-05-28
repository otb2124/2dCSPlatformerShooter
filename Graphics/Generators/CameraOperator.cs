using Platformer.Entities.Types;
using System;
using System;
using Microsoft.Xna.Framework;
using Platformer.Entities.Types;
using Platformer.Physics;

namespace Platformer.Graphics.Generators
{
    public class CameraOperator
    {

        Camera camera;

        public CameraOperator(Camera camera)
        {
            this.camera = camera;
        }

        public void Update(Game1 game)
        {

            if (game.gameState == game.PLAYSTATE)
            {
                camera.position = FlatConverter.ToVector2(game.player.Body.Position);
            }
            else if (game.gameState == game.DIALOGUESTATE)
            {
                if (game.player.isInterracting != null)
                {


                    if (!camera.hasMoved)
                    {
                        MoveFor(FlatConverter.ToVector2(game.aiManager.GetClosest(game.player, typeof(NPC)).Body.Position), 10);
                    }
                    else
                    {

                    }

                }

            }
            else if (game.gameState == game.MENUSTATE)
            {
                camera.position = Vector2.Zero;
            }






           

        }







        public void MoveFor(Vector2 target, float time)
        {
            // Calculate the direction from the camera position to the target position
            Vector2 direction = Vector2.Normalize(target - camera.position);

            // Calculate the total frames needed to complete the movement
            float frames = time * 60;

            // Calculate the distance to the target position
            float distanceToTarget = Vector2.Distance(camera.position, target);

            // Calculate the necessary speed to cover the distance in the given time
            float speed = distanceToTarget / time;

            // Calculate the incremental movement per frame based on the desired speed and time
            float distancePerFrame = distanceToTarget / frames;
            Vector2 deltaMovement = direction * distancePerFrame * speed;

            // Check if the camera has reached or passed the target position
            if (camera.counter >= frames || distanceToTarget <= distancePerFrame)
            {

                // Reset movement-related variables
                camera.isMoving = false;
                camera.counter = 0;
                camera.hasMoved = true;
            }
            else
            {
                // Move the camera towards the target position
                camera.position += deltaMovement;

                // Increment the counter
                camera.counter++;
                camera.isMoving = true;
            }
        }



    }
}
