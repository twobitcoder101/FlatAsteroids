﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Flat;
using Flat.Graphics;
using Flat.Input;
using Flat.Physics;

namespace FlatAsteroids
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private Screen screen;
        private Sprites sprites;
        private Shapes shapes;
        private Camera camera;

        private int loopCounter = 0;

        private List<Entity> entities;

        private SoundEffect rocketSound;
        private SoundEffectInstance rocketSoundInstance;

        private bool displayCollisionCircles = true;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SynchronizeWithVerticalRetrace = true;


            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
        }

        protected override void Initialize()
        {
            DisplayMode dm = this.GraphicsDevice.DisplayMode;
            this.graphics.PreferredBackBufferWidth = (int)(dm.Width * 0.8f);
            this.graphics.PreferredBackBufferHeight = (int)(dm.Height * 0.8f);
            this.graphics.ApplyChanges();


            this.screen = new Screen(this, 1280, 720);
            this.sprites = new Sprites(this);
            this.shapes = new Shapes(this);
            this.camera = new Camera(this.screen);


            Random rand = new Random(0);

            this.entities = new List<Entity>();

            Vector2[] vertices = new Vector2[5];
            vertices[0] = new Vector2(10, 0);
            vertices[1] = new Vector2(-10, -10);
            vertices[2] = new Vector2(-5, -3);
            vertices[3] = new Vector2(-5, 3);
            vertices[4] = new Vector2(-10, 10);

            MainShip player = new MainShip(vertices, new Vector2(0, 0), Color.LightGreen, CommonDensities.Steel, 0.6f);
            this.entities.Add(player);


            int asteroidCount = 30;

            for (int i = 0; i < asteroidCount; i++)
            {
                Asteroid asteroid = new Asteroid(rand, this.camera, CommonDensities.Rock, 0.6f);
                this.entities.Add(asteroid);
            }

            



            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.rocketSound = this.Content.Load<SoundEffect>("expl02");
            this.rocketSoundInstance = this.rocketSound.CreateInstance();
        }

        protected override void Update(GameTime gameTime)
        {
            FlatKeyboard keyboard = FlatKeyboard.Instance;
            keyboard.Update();

            FlatMouse mouse = FlatMouse.Instance;
            mouse.Update();

            if(keyboard.IsKeyClicked(Keys.OemTilde))
            {
                Console.WriteLine(this.loopCounter);
            }

            if(keyboard.IsKeyClicked(Keys.B))
            {
                this.displayCollisionCircles = !this.displayCollisionCircles;
            }

            if(keyboard.IsKeyClicked(Keys.A))
            {
                this.camera.IncZoom();
            }

            if(keyboard.IsKeyClicked(Keys.Z))
            {
                this.camera.DecZoom();
            }

            MainShip player = (MainShip)this.entities[0];

            float playerRotationAmount = MathHelper.Pi * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(keyboard.IsKeyDown(Keys.Left))
            {
                player.Rotate(playerRotationAmount);
            }

            if (keyboard.IsKeyDown(Keys.Right))
            {
                player.Rotate(-playerRotationAmount);
            }

            if(keyboard.IsKeyDown(Keys.Up))
            {
                player.ApplyRocketForce(50f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if (this.rocketSoundInstance.State != SoundState.Playing)
                {
                    this.rocketSoundInstance.Volume = 0.1f;
                    this.rocketSoundInstance.Play();
                }
            }
            else
            {
                player.DisableRocketForce();

                if(this.rocketSoundInstance.State == SoundState.Playing)
                {
                    this.rocketSoundInstance.Stop();
                }
            }


            for (int i = 0; i < this.entities.Count; i++)
            {
                this.entities[i].Update(gameTime, this.camera);
            }


            this.loopCounter = 0;

            for(int i = 0; i < this.entities.Count - 1; i++)
            {
                Entity a = this.entities[i];
                Circle ca = new Circle(a.Position, a.Radius);

                for(int j = i + 1; j < this.entities.Count; j++)
                {
                    this.loopCounter++;

                    Entity b = this.entities[j];
                    Circle cb = new Circle(b.Position, b.Radius);

                    if (Collision.IntersectCircles(ca, cb, out float depth, out Vector2 normal))
                    {
                        Vector2 mtv = depth * normal;

                        a.Move(-mtv / 2f);
                        b.Move(mtv / 2f);

                        Game1.SolveCollision(a, b, normal);

                        a.CircleColor = Color.Red;
                        b.CircleColor = Color.Red;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screen.Set();
            this.GraphicsDevice.Clear(Color.Black);

            this.shapes.Begin(this.camera);

            for (int i = 0; i < this.entities.Count; i++)
            {
                this.entities[i].Draw(this.shapes, this.displayCollisionCircles);
            }

            this.shapes.End();

            this.screen.UnSet();
            this.screen.Present(this.sprites);



            base.Draw(gameTime);
        }

        public static void SolveCollision(Entity a, Entity b, Vector2 normal)
        {
            Vector2 relVel = b.Velocity - a.Velocity;

            if(Util.Dot(relVel, normal) > 0f)
            {
                return;
            }

            float e = MathHelper.Min(a.Restitution, b.Restitution);

            float j = -(1f + e) * Util.Dot(relVel, normal);
            j /= a.InvMass + b.InvMass;

            Vector2 impulse = j * normal;

            a.Velocity -= a.InvMass * impulse;
            b.Velocity += b.InvMass * impulse;
        }
    }
}
