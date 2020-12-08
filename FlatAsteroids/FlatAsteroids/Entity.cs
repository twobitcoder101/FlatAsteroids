using System;
using Microsoft.Xna.Framework;
using Flat;
using Flat.Graphics;
using Flat.Physics;

namespace FlatAsteroids
{
    public abstract class Entity 
    {
        protected Vector2[] vertices;
        protected Vector2 position;
        protected Vector2 velocity;
        protected float angle;
        protected Color color;
        protected float radius;        
        public Color CircleColor;

        public Vector2 Position
        {
            get { return this.position; }
        }

        public float Radius
        {
            get { return this.radius; }
        }

        public Entity(Vector2[] vertices, Vector2 position, Color color)
        {
            this.vertices = vertices;
            this.position = position;
            this.velocity = Vector2.Zero;
            this.angle = 0f;
            this.color = color;

            if (vertices != null)
            {
                this.radius = Entity.FindCollisionCircleRadius(vertices);
            }

        }
        
        protected static float FindCollisionCircleRadius(Vector2[] vertices)
        {
            float polygonArea = PolygonHelper.FindPolygonArea(vertices);
            return MathF.Sqrt(polygonArea / MathHelper.Pi);
        }

        public void Move(Vector2 amount)
        {
            this.position += amount;
        }

        public virtual void Update(GameTime gameTime, Camera camera)
        {
            this.position += this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            camera.GetExtents(out Vector2 camMin, out Vector2 camMax);

            float camViewWidth = camMax.X - camMin.X;
            float camViewHeight = camMax.Y - camMin.Y;

            if (this.position.X < camMin.X) { this.position.X += camViewWidth; }
            if (this.position.X > camMax.X) { this.position.X -= camViewWidth; }
            if (this.position.Y < camMin.Y) { this.position.Y += camViewHeight; }
            if (this.position.Y > camMax.Y) { this.position.Y -= camViewHeight; }

            this.CircleColor = Color.White;
        }

        public virtual void Draw(Shapes shapes, bool displayCollisionCircles = false)
        {
            FlatTransform transform = new FlatTransform(this.position, this.angle, 1f);
            shapes.DrawPolygon(this.vertices, transform, 1f, this.color);

            if (displayCollisionCircles)
            {
                shapes.DrawCircle(this.position.X, this.position.Y, this.radius, 32, 1f, this.CircleColor);
            }
        }
    }
}
