using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ritual.Game;

namespace Ritual
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RitualGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Level level;
        Camera camera;

        //(2,2)
        private float startX = 800;
        private float startY = 480;

        public RitualGame()
        {
            this.Window.Title = "The Ritual";

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //not sure why this wasn't here in the first place.
            //had to add this because GraphicsDevice was null when loading content in Level.cs
            if (this.graphics != null)
            {
                this.graphics.CreateDevice();
            }

            level = new Level(Content.ServiceProvider);

            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);
            camera.Position = new Vector2(startX, startY);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            level.LoadContent();
           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Console.WriteLine("Camera position: (" + camera.Position.X + ", " + camera.Position.Y + ")");
            //Console.WriteLine("Expected position: (" + level.CurrentColumn * 800 + ", " + level.CurrentRow * 480);

            if ( ((level.CurrentColumn * 800) == camera.Position.X) && ((level.CurrentRow * 480) == camera.Position.Y) )
            {
                level.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
            }

            HandleCamera(gameTime);

            base.Update(gameTime);
        }

        protected void HandleCamera(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float newViewPortX = level.CurrentColumn * 800;
            float newViewPortY = level.CurrentRow * 480;

            if (camera.Position.X > newViewPortX)
            {
                camera.Position -= new Vector2(900, 0) * delta;

                //if we went too far, set position to where we expect it to be
                if (camera.Position.X < newViewPortX)
                {
                    camera.Position = new Vector2(newViewPortX, newViewPortY);
                }
            }
            if (camera.Position.X < newViewPortX)
            {
                camera.Position += new Vector2(900, 0) * delta;

                if (camera.Position.X > newViewPortX)
                {
                    camera.Position = new Vector2(newViewPortX, newViewPortY);
                }
            }

            if (camera.Position.Y > newViewPortY)
            {
                camera.Position -= new Vector2(0, 900) * delta;

                if (camera.Position.Y < newViewPortY)
                {
                    camera.Position = new Vector2(newViewPortX, newViewPortY);
                }
            }
            if (camera.Position.Y < newViewPortY)
            {
                camera.Position += new Vector2(0, 900) * delta;

                if (camera.Position.Y > newViewPortY)
                {
                    camera.Position = new Vector2(newViewPortX, newViewPortY);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            Matrix viewMatrix = camera.GetViewMatrix();

            spriteBatch.Begin(transformMatrix: viewMatrix);
            level.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
