using System;
using Microsoft.Xna.Framework;

namespace Flat
{
    public static class RandomHelper
    {
        private static Random StaticRand = new Random();

        public static float RandomSingle(Random rand, float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min");
            }

            return min + (float)rand.NextDouble() * (max - min);
        }

        public static float RandomSingle(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min");
            }

            return min + (float)StaticRand.NextDouble() * (max - min);
        }

        public static Vector2 RandomDirection(Random rand)
        {
            float angle = RandomHelper.RandomSingle(rand, 0f, MathHelper.TwoPi);
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }
    }
}
