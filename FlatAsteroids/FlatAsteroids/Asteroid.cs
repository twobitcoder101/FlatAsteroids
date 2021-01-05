using System;
using Microsoft.Xna.Framework;
using Flat;
using Flat.Graphics;

namespace FlatAsteroids
{
    public class Asteroid : Entity
    {
        public Asteroid(Random rand, Camera camera, float density, float restitution)
            : base(null, Vector2.Zero, Color.Brown, density, restitution)
        {
            int minPoints = 6;
            int maxPoints = 10;

            int points = rand.Next(minPoints, maxPoints);

            this.vertices = new Vector2[points];

            float deltaAngle = MathHelper.TwoPi / (float)points;
            float angle = 0f;

            float minDist = 12f;
            float maxDist = 24f;

            for(int i = 0; i < points; i++)
            {
                float dist = RandomHelper.RandomSingle(rand, minDist, maxDist);

                float x = MathF.Cos(angle) * dist;
                float y = MathF.Sin(angle) * dist;

                this.vertices[i] = new Vector2(x, y);

                angle += deltaAngle;
            }

            camera.GetExtents(out Vector2 camMin, out Vector2 camMax);

            camMin *= 0.75f;
            camMax *= 0.75f;

            float px = RandomHelper.RandomSingle(rand, camMin.X, camMax.X);
            float py = RandomHelper.RandomSingle(rand, camMin.Y, camMax.Y);

            this.position = new Vector2(px, py);


            float minSpeed = 20f;
            float maxSpeed = 40f;

            Vector2 velDir = RandomHelper.RandomDirection(rand);
            float speed = RandomHelper.RandomSingle(rand, minSpeed, maxSpeed);

            this.velocity = velDir * speed;

            this.radius = Entity.FindCollisionCircleRadius(vertices);

            this.area = MathHelper.Pi * this.radius * this.radius;
            this.mass = this.area * density;
            this.invMass = 1f / this.mass;
        }

    }
}
