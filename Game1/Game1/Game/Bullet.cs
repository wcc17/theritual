using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ritual.Game
{
    class Bullet
    {
        Level level;

        private float speed = 700.0f;

        private int bulletWidth = 25;
        private int bulletHeight = 25;

        public Bullet(Level level)
        {
            this.level = level;

            Rectangle r = new Rectangle();
            r.Width = bulletWidth;
            r.Height = bulletHeight;
            this.Bounds = r;
        }

        //will return true if the bullet has reached its destination
        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //adjust location of rectangle
            Rectangle rectangle = this.Bounds;
            Point point = new Point();
            point.X = (int) this.CurrentPosition.X;
            point.Y = (int) this.CurrentPosition.Y;
            rectangle.Location = point;
            this.Bounds = rectangle;

            this.CurrentPosition += Direction * speed * delta;

            if (Vector2.Distance(this.StartPosition, this.CurrentPosition) >= Distance)
            {
                ReachedDestination = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BulletTexture, new Vector2(this.CurrentPosition.X, this.CurrentPosition.Y), Color.White);
        }

        public Texture2D BulletTexture
        {
            get;
            set;
        }

        public Boolean ReachedDestination
        {
            get;
            set;
        }

        public Vector2 CurrentPosition
        {
            get;
            set;
        }

        public Vector2 StartPosition
        {
            get;
            set;
        }

        public Vector2 Direction
        {
            get;
            set;
        }

        public float Distance
        {
            get;
            set;
        }

        public Rectangle Bounds
        {
            get;
            set;
        }
    }
}
