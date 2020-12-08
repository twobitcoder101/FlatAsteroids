using System;
using Microsoft.Xna.Framework;
using Flat;
using Flat.Graphics;

namespace FlatAsteroids
{
    public class MainShip : Entity
    {
        private bool isRocketForce;
        private Vector2[] rocketVertices;
        private double randomRocketTime;
        private double randomRocketStartTime;

        public MainShip(Vector2[] vertices, Vector2 position, Color color)
            : base(vertices, position, color)
        {
            this.isRocketForce = false;

            this.rocketVertices = new Vector2[3];
            this.rocketVertices[0] = this.vertices[3];
            this.rocketVertices[1] = this.vertices[2];
            this.rocketVertices[2] = new Vector2(-24f, 0f);

            this.randomRocketTime = 60d;
            this.randomRocketStartTime = 0d;
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

        public override void Update(GameTime gameTime, Camera camera)
        {
            double now = gameTime.TotalGameTime.TotalMilliseconds;

            if(now - this.randomRocketStartTime >= this.randomRocketTime)
            {
                this.randomRocketStartTime = now;

                float rocketMinX = -28f;
                float rocketMaxX = -20;
                float rocketMinY = -2f;
                float rocketMaxY = 2f;

                this.rocketVertices[2] = new Vector2(RandomHelper.RandomSingle(rocketMinX, rocketMaxX), RandomHelper.RandomSingle(rocketMinY, rocketMaxY));
            }

            base.Update(gameTime, camera);
        }

        public override void Draw(Shapes shapes, bool displayCollisionCircles)
        {
            if(this.isRocketForce)
            {
                FlatTransform transform = new FlatTransform(this.position, this.angle, 1f);
                shapes.DrawPolygon(this.rocketVertices, transform, 1f, Color.Yellow);
            }

            base.Draw(shapes, displayCollisionCircles);
        }

        public void ApplyRocketForce(float amount)
        {
            Vector2 forceDir = new Vector2(MathF.Cos(this.angle), MathF.Sin(this.angle));
            this.velocity += forceDir * amount;
            this.isRocketForce = true;
        }

        public void DisableRocketForce()
        {
            this.isRocketForce = false;
        }
    }
}
