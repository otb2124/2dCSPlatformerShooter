using System;

namespace Platformer.Physics
{
    public readonly struct FlatVector
    {
        public readonly float X;
        public readonly float Y;

        public static readonly FlatVector Zero = new FlatVector(0f, 0f);

        public FlatVector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static FlatVector operator +(FlatVector a, FlatVector b)
        {
            return new FlatVector(a.X + b.X, a.Y + b.Y);
        }

        public static FlatVector operator -(FlatVector a, FlatVector b)
        {
            return new FlatVector(a.X - b.X, a.Y - b.Y);
        }

        public static FlatVector operator -(FlatVector v)
        {
            return new FlatVector(-v.X, -v.Y);
        }

        public static FlatVector operator *(FlatVector v, float s)
        {
            return new FlatVector(v.X * s, v.Y * s);
        }

        public static FlatVector operator *(float s, FlatVector v)
        {
            return new FlatVector(v.X * s, v.Y * s);
        }

        public static FlatVector operator /(FlatVector v, float s)
        {
            return new FlatVector(v.X / s, v.Y / s);
        }

        internal static FlatVector Transform(FlatVector v, FlatTransform transform)
        {
            return new FlatVector(
                transform.Cos * v.X - transform.Sin * v.Y + transform.PositionX,
                transform.Sin * v.X + transform.Cos * v.Y + transform.PositionY);
        }

        public bool Equals(FlatVector other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is FlatVector other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return new { X, Y }.GetHashCode();
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
