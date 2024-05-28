using System;
using Microsoft.Xna.Framework;

namespace Platformer.Physics
{
    public readonly struct FlatBox
    {
        public readonly Vector2 Min;
        public readonly Vector2 Max;

        public static readonly FlatBox Empty = new FlatBox(Vector2.Zero, Vector2.Zero);

        public FlatBox(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public FlatBox(Vector2 center, float width, float height)
        {
            float left = center.X - width * 0.5f;
            float right = left + width;
            float bottom = center.Y - height * 0.5f;
            float top = bottom + height;

            Min = new Vector2(left, bottom);
            Max = new Vector2(right, top);
        }

        public FlatBox(float minX, float maxX, float minY, float maxY)
        {
            Min = new Vector2(minX, minY);
            Max = new Vector2(maxX, maxY);
        }

        public bool Equals(in FlatBox other)
        {
            return Min == other.Min && Max == other.Max;
        }

        public override bool Equals(object obj)
        {
            if (obj is FlatBox other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = new { Min, Max }.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            string result = string.Format("Min: {0}, Max: {1}", Min, Max);
            return result;
        }
    }
}
