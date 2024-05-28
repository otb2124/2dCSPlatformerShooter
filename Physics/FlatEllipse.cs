using System;
using Microsoft.Xna.Framework;

namespace Platformer.Physics
{
    public readonly struct FlatEllipse
    {
        public readonly Vector2 Center;
        public readonly Vector2 Radius;

        public FlatEllipse(Vector2 center, Vector2 radius)
        {
            Center = center;
            Radius = radius;
        }

        public void GetExtents(out FlatBox box)
        {
            Vector2 min = new Vector2(Center.X - Radius.X, Center.Y - Radius.Y);
            Vector2 max = new Vector2(Center.X + Radius.X, Center.Y + Radius.Y);
            box = new FlatBox(min, max);
        }

        public bool Contains(Vector2 v, out float d)
        {
            // https://math.stackexchange.com/questions/76457/check-if-a-point-is-within-an-ellipse

            float dx = v.X - Center.X;
            float dy = v.Y - Center.Y;

            float num1 = dx * dx;
            float den1 = Radius.X * Radius.X;

            float num2 = dy * dy;
            float den2 = Radius.Y * Radius.Y;

            d = num1 / den1 + num2 / den2;

            // todo: Should this be less than OR EQUAL TO instead of just less than?
            return d < 1f;
        }

        public bool Equals(FlatEllipse other)
        {
            return Center == other.Center && Radius == other.Radius;
        }

        public override bool Equals(object obj)
        {
            if (obj is FlatEllipse other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = new { Center, Radius }.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            string result = string.Format("Center: {0}, Radius(X): {1}, Radius(Y): {2}", Center, Radius.X, Radius.Y);
            return result;
        }
    }
}
