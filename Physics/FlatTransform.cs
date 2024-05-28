using System;

namespace Platformer.Physics
{
    public struct FlatTransform
    {
        public readonly static FlatTransform Zero = new FlatTransform(0f, 0f, 0f);

        public readonly float PositionX;
        public readonly float PositionY;
        public readonly float Sin;
        public readonly float Cos;


        public float CosScaleX => Cos;
        public float SinScaleY => -Sin;
        public float SinScaleX => Sin;
        public float CosScaleY => Cos;

        public FlatTransform(FlatVector position, float angle)
        {
            PositionX = position.X;
            PositionY = position.Y;
            Sin = MathF.Sin(angle);
            Cos = MathF.Cos(angle);
        }

        public FlatTransform(float x, float y, float angle)
        {
            PositionX = x;
            PositionY = y;
            Sin = MathF.Sin(angle);
            Cos = MathF.Cos(angle);
        }


    }
}
