using System;
using Microsoft.Xna.Framework;

namespace Flat
{
    public readonly struct Circle
    {
        public readonly Vector2 Center;
        public readonly float Radius;

        public Circle(Vector2 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Circle(float x, float y, float radius)
        {
            this.Center = new Vector2(x, y);
            this.Radius = radius;
        }

    }
}
