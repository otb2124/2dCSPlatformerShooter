using System;
using System.Runtime.CompilerServices;

namespace Platformer.Physics
{
    public readonly struct FlatSize
    {
        public readonly int Width;
        public readonly int Height;

        public FlatSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(FlatSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (obj is FlatSize other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int result = new { Width, Height }.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            string result = string.Format("({0}, {1})", Width, Height);
            return result;
        }
    }
}
