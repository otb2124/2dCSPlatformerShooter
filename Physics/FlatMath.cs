using System;

namespace Platformer.Physics
{
    public static class FlatMath
    {
        /// <summary>
        /// Equal to 1/2 of a millimeter.
        /// </summary>
        public static readonly float VerySmallAmount = 0.0005f;

        public static float Clamp(float value, float min, float max)
        {
            if (min == max)
            {
                return min;
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min is greater than the max.");
            }

            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (min == max)
            {
                return min;
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min is greater than the max.");
            }

            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        public static float LengthSquared(FlatVector v)
        {
            return v.X * v.X + v.Y * v.Y;
        }

        public static float Length(FlatVector v)
        {
            return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static float DistanceSquared(FlatVector a, FlatVector b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        public static float Distance(FlatVector a, FlatVector b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        public static FlatVector Normalize(FlatVector v)
        {
            float len = Length(v);
            return new FlatVector(v.X / len, v.Y / len);
        }

        public static float Dot(FlatVector a, FlatVector b)
        {
            // a · b = ax * bx + ay * by
            return a.X * b.X + a.Y * b.Y;
        }

        public static float Cross(FlatVector a, FlatVector b)
        {
            // cz = ax * by − ay * bx
            return a.X * b.Y - a.Y * b.X;
        }

        public static bool NearlyEqual(float a, float b)
        {
            return MathF.Abs(a - b) < VerySmallAmount;
        }

        public static bool NearlyEqual(FlatVector a, FlatVector b)
        {
            return DistanceSquared(a, b) < VerySmallAmount * VerySmallAmount;
        }


        public static FlatVector RotateVector(FlatVector vector, float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            float newX = vector.X * cos - vector.Y * sin;
            float newY = vector.X * sin + vector.Y * cos;

            return new FlatVector(newX, newY);
        }


        public static FlatVector ClampMagnitude(FlatVector vector, float maxLength)
        {
            float lengthSquared = LengthSquared(vector);

            if (lengthSquared > maxLength * maxLength)
            {
                float scaleFactor = maxLength / MathF.Sqrt(lengthSquared);
                return new FlatVector(vector.X * scaleFactor, vector.Y * scaleFactor);
            }

            return vector;
        }


        public static float AngleBetween(FlatVector a, FlatVector b)
        {
            // Calculate the dot product
            float dotProduct = Dot(a, b);

            // Calculate the magnitudes of the vectors
            float magnitudeA = Length(a);
            float magnitudeB = Length(b);

            // Calculate the angle in radians using the arccosine function
            float angle = (float)Math.Acos(dotProduct / (magnitudeA * magnitudeB));

            // Convert the angle to degrees
            float angleInDegrees = angle * (180f / (float)Math.PI);

            return angleInDegrees;
        }


    }
}
