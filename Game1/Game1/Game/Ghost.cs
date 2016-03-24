using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ritual.Game
{
    class Ghost
    {
        Level level;

        private float speedY = 25.0f;

        private int ghostWidth = 60;
        private int ghostHeight = 80;

        public Ghost(Level level, float startX, float startY)
        {
            this.level = level;
            this.x = startX;
            this.y = startY;
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        public void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.y -= delta * speedY;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GhostTexture, new Vector2(this.x, this.y), Color.White);
        }

        public Texture2D GhostTexture
        {
            get;
            set;
        }

        public float x
        {
            get;
            set;
        }

        public float y
        {
            get;
            set;
        }
    }
}
