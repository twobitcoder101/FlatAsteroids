using System;
using Microsoft.Xna.Framework;

namespace FlatAsteroids
{
    public class MainShip : Entity
    {

        public MainShip(Vector2[] vertices, Vector2 position, Color color)
            : base(vertices, position, color)
        {

        }

        public void Rotate(float amount)
        {
            this.angle += amount;

            if(this.angle < 0f)
            {
                this.angle += MathHelper.TwoPi;
            }

            if(this.angle >= MathHelper.TwoPi)
            {
                this.angle -= MathHelper.TwoPi;
            }
        }

        public void ApplyForce(float amount)
        {
            Vector2 forceDir = new Vector2(MathF.Cos(this.angle), MathF.Sin(this.angle));
            this.velocity += forceDir * amount;
        }
    }
}
