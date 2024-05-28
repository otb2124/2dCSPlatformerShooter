using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Platformer.Entities;
using Platformer.Graphics.Generators;
using Platformer.Physics;
using Platformer.Utils;

namespace Platformer.Entities.Types
{
    public class FlatEntity
    {

        public bool isBossPart = false;
        public FlatBody Body;
        public Color Color;

        public enum Direction
        {
            Right,
            Left
        }



        //projetile
        public FlatEntity(float radius, FlatVector position, Boolean isStatic, Color color)
        {
            if (!(FlatBody.CreateCircleBody(radius, 0.5f, isStatic, 0.5f, out FlatBody body, out string errorMessage)))
            {
                throw new Exception(errorMessage);
            }

            body.MoveTo(position);
            this.Body = body;
            this.Color = color;
        }

        public FlatEntity(float width, float height, FlatVector position, Boolean isStatic, Color color)
        {
            if (!(FlatBody.CreateBoxBody(width, height, 0.5f, isStatic, 0.5f, out FlatBody body, out string errorMessage)))
            {
                throw new Exception(errorMessage);
            }

            body.MoveTo(position);
            this.Body = body;
            this.Color = color;
        }




        //player
        public FlatEntity(FlatWorld world, float width, float height, bool isStatic, FlatVector position)
        {
            if (!(FlatBody.CreateBoxBody(width, height, 1f, isStatic, 0.5f, out FlatBody body, out string errorMessage)))
            {
                throw new Exception(errorMessage);
            }


            body.MoveTo(position);
            this.Body = body;
            world.AddBody(this.Body);
            this.Color = RandomHelper.RandomColor();
        }



        //other
        public FlatEntity(float width, float height, bool isStatic, FlatVector position)
        {
            if (!(FlatBody.CreateBoxBody(width, height, 1f, isStatic, 0.5f, out FlatBody body, out string errorMessage)))
            {
                throw new Exception(errorMessage);
            }


            body.MoveTo(position);
            this.Body = body;
            this.Color = RandomHelper.RandomColor();
        }


        public FlatEntity(float radius, bool isStatic, FlatVector position)
        {
            if (!(FlatBody.CreateCircleBody(radius, 1f, isStatic, 0.5f, out FlatBody body, out string errorMessage)))
            {
                throw new Exception(errorMessage);
            }


            body.MoveTo(position);
            this.Body = body;
            this.Color = RandomHelper.RandomColor();

        }






        public void Draw(Shapes shapes)
        {


            Vector2 position = FlatConverter.ToVector2(this.Body.Position);
            FlatVector fv = FlatConverter.ToFlatVector(position);

            if (this.Body.ShapeType is ShapeType.Circle)
            {
                Vector2 va = Vector2.Zero;
                Vector2 vb = new Vector2(Body.Radius, 0f);

                FlatTransform transform = new FlatTransform(fv, Body.Angle);
                va = FlatUtil.Transform(va, transform);
                vb = FlatUtil.Transform(vb, transform);


                shapes.DrawCircleFill(position, this.Body.Radius, 26, this.Color);
                //shapes.DrawCircle(position, this.Body.Radius, 26, Color.White);
                shapes.DrawLine(va, vb, Color.White);
            }
            else if (this.Body.ShapeType is ShapeType.Box)
            {
                shapes.DrawBoxFill(position, this.Body.Width, this.Body.Height, this.Body.Angle, this.Color);
                //shapes.DrawBox(position, this.Body.Width, this.Body.Height, this.Body.Angle, Color.White);
            }
        }

        public void Update(Game1 game)
        {

        }
    }
}
