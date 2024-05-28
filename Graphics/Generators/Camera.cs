using System;
using Microsoft.Xna.Framework;
using Platformer.Entities.Types;
using Platformer.Physics;

namespace Platformer.Graphics.Generators
{
    public sealed class Camera
    {

        #region Fields


        private Game1 game;
        private Screen screen;
        public Vector2 position;
        private Matrix view;
        private Matrix proj;

        private float aspectRatio;
        private float fieldOfView;
        private double baseZ;
        private double z;

        public int zoom;
        private bool isStickToPlayer = true;

        private float angle;
        private Vector2 up;

        private const float MinZ = 1f;
        private const float MaxZ = 2000f;

        private const int MinZoom = 1;
        private const int MaxZoom = 32;

        public int counter = 0;
        public bool isMoving = false, hasMoved = false;

        public int ScreenResW, ScreenResH, ScreenW, ScreenH;

        public CameraOperator cameraOperator;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return position; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix Projection
        {
            get { return proj; }
        }

        internal float AspectRatio
        {
            get { return aspectRatio; }
        }

        internal float FieldOfView
        {
            get { return fieldOfView; }
        }

        public double BaseZ
        {
            get { return baseZ; }
        }

        public double Z
        {
            get { return z; }
        }

        public int Zoom
        {
            get { return zoom; }
            set
            {
                zoom = FlatMath.Clamp(value, MinZoom, MaxZoom);
                z = baseZ * (1d / zoom);
            }
        }

        public Vector2 Up
        {
            get { return up; }
        }

        public float Angle
        {
            get { return angle; }
        }

        #endregion

        public Camera(Screen screen, Game1 game)
        {
            this.game = game;
            this.screen = screen ?? throw new ArgumentNullException("screen");
            position = Vector2.Zero;
            view = Matrix.Identity;
            proj = Matrix.Identity;

            aspectRatio = screen.Width / (float)screen.Height;
            fieldOfView = MathHelper.PiOver2;
            baseZ = GetZFromHeight(screen.Height);
            z = baseZ;

            angle = 0f;
            up = new Vector2(MathF.Sin(angle), MathF.Cos(angle));

            zoom = 1;

            ScreenResW = screen.Width;
            ScreenResH = screen.Height;
            ScreenW = game.ResolutionX;
            ScreenH = game.ResolutionY;

            cameraOperator = new CameraOperator(this);

            Update();

        }


        

        public void Update()
        {
            cameraOperator.Update(game);




            Matrix translation = Matrix.CreateTranslation(-position.X, -position.Y, 0);

            // Calculate the view matrix by combining translation with the look-at matrix
            view = translation * Matrix.CreateLookAt(new Vector3(0, 0, (float)z), Vector3.Zero, new Vector3(up, 0f));

            // Update the projection matrix
            proj = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, MinZ, MaxZ);
        }


        private double GetZFromHeight(double height)
        {
            double result = height * 0.5d / Math.Tan(fieldOfView * 0.5d);
            return result;
        }

        private double GetVisibleHeightFromZ(double z)
        {
            double result = z * Math.Tan(fieldOfView * 0.5f) * 2f;
            return result;
        }

        private double GetVisibleHeight()
        {
            double result = 2d * Math.Tan(fieldOfView * 0.5d) * z;
            return result;
        }

        

        public void ResetZ()
        {
            z = baseZ;
            Update();
        }

        public void Move(Vector2 amount)
        {
            position += amount;
        }

        public void MoveTo(Vector2 position)
        {
            this.position = position;
        }

        public void MoveZ(float amount)
        {
            double new_z = z + amount;

            if (new_z < MinZ ||
                new_z > MaxZ)
            {
                return;
            }

            z = new_z;
        }

        public void IncZoom()
        {
            int new_zoom = zoom + 1;

            if (new_zoom < MinZoom || new_zoom > MaxZoom)
            {
                return;
            }

            zoom = new_zoom;
            z = baseZ * (1d / zoom);

        }

        public void DecZoom()
        {
            int new_zoom = zoom - 1;

            if (new_zoom < MinZoom || new_zoom > MaxZoom)
            {
                return;
            }

            zoom = new_zoom;
            z = baseZ * (1d / zoom);

        }



        public void Rotate(float amount)
        {
            angle += amount;
            up = new Vector2(MathF.Sin(angle), MathF.Cos(angle));
        }

        public void GetExtents(out float width, out float height)
        {
            height = (float)GetVisibleHeight();
            width = height * aspectRatio;
        }

        public void GetExtents(out float left, out float right, out float bottom, out float top)
        {
            // Calculate half the width and height of the view
            float halfWidth = (float)(aspectRatio * z * Math.Tan(fieldOfView * 0.5f));
            float halfHeight = (float)(z * Math.Tan(fieldOfView * 0.5f));

            // Calculate the position of the camera's center
            float centerX = position.X;
            float centerY = position.Y;

            // Calculate the bounds of the camera's view
            left = centerX - halfWidth;
            right = centerX + halfWidth;
            bottom = centerY - halfHeight;
            top = centerY + halfHeight;
        }


        public void GetExtents(out Vector2 min, out Vector2 max)
        {
            GetExtents(out float left, out float right, out float bottom, out float top);

            min = new Vector2(left, bottom);
            max = new Vector2(right, top);
        }


        public void MoveLeft(float amount)
        {
            // Move the camera to the left by the specified amount
            position.X -= amount;
        }

        public void MoveRight(float amount)
        {
            // Move the camera to the right by the specified amount
            position.X += amount;
        }

        public void MoveUp(float amount)
        {
            // Move the camera upward by the specified amount
            position.Y += amount;
        }

        public void MoveDown(float amount)
        {
            position.Y -= amount;
        }

        public void CheckPlayer()
        {


            GetExtents(out float left, out float right, out float bottom, out float top);

            // Create an invisible rectangle around the player
            Rectangle playerRect = new Rectangle(
                (int)position.X - 50, // Make the rectangle wider than the player
                (int)position.Y + 50, // Make the rectangle taller than the player
                100, // Width of the rectangle
                100 // Height of the rectangle
            );

            // Check if the camera's view intersects with the player's rectangle
            if (playerRect.Intersects(new Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top))))
            {
                // The player's rectangle is within the camera's view, no need to move the camera
                return;
            }

            // Move the camera to keep the player's rectangle within the camera's view
            if (playerRect.Left < left)
            {
                MoveLeft(left - playerRect.Left);
            }
            else if (playerRect.Right > right)
            {
                MoveRight(playerRect.Right - right);
            }

            /*if (playerRect.Top < top)
            {
                MoveUp(top - playerRect.Top);
            }
            else if (playerRect.Bottom > bottom)
            {
                MoveDown(playerRect.Bottom - bottom);
            }*/


            position.Y = game.player.Body.Position.Y;
        }


        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            // Calculate the scaled position based on the camera's zoom level and screen resolution
            Vector2 relativePosition = worldPosition - Position;
            Vector2 scaledPosition = relativePosition * 20.0f / Zoom;
            Vector2 screenPosition = scaledPosition + new Vector2(ScreenW / 2, ScreenH / 2);

            return screenPosition;
        }

    }
}

