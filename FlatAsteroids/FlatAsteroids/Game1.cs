using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Flat;
using Flat.Graphics;
using Flat.Input;

namespace FlatAsteroids
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private Screen screen;
        private Sprites sprites;
        private Shapes shapes;
        private Camera camera;

        
        private List<Entity> entities;

        private SoundEffect rocketSound;
        private SoundEffectInstance rocketSoundInstance;

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

            MainShip player = new MainShip(vertices, new Vector2(0, 0), Color.LightGreen);
            this.entities.Add(player);


            int asteroidCount = 5;

            for (int i = 0; i < asteroidCount; i++)
            {
                Asteroid asteroid = new Asteroid(rand, this.camera);
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
                    this.rocketSoundInstance.Volume = 0.2f;
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screen.Set();
            this.GraphicsDevice.Clear(Color.Black);

            this.shapes.Begin(this.camera);

            for (int i = 0; i < this.entities.Count; i++)
            {
                this.entities[i].Draw(this.shapes);
            }

            this.shapes.End();

            this.screen.UnSet();
            this.screen.Present(this.sprites);



            base.Draw(gameTime);
        }
    }
}
