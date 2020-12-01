using System;
using Microsoft.Xna.Framework;

namespace Flat.Physics
{
    public static class Collision
    {
        public static bool IntersectCircles(Circle a, Circle b)
        {
            float distSquared = Util.DistanceSquared(a.Center, b.Center);
            float r2 = a.Radius + b.Radius;
            float radiusSquared = r2 * r2;

            if(distSquared >= radiusSquared)
            {
                return false;
            }

            return true;
        }
    }
}
