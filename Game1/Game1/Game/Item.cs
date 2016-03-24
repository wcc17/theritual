using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ritual.Game
{
    class Item
    {
        Level level;

        private int itemWidth = 25;
        private int itemHeight = 25;

        public Item(Level level)
        {
            this.level = level;

            Rectangle r = new Rectangle();
            r.Width = itemWidth;
            r.Height = itemHeight;
            this.Bounds = r;

            this.HasBeenPickedUp = false;
        }

        public void Update(GameTime gameTime, Rectangle playerBounds, float playerX, float playerY)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rectangle rectangle = this.Bounds;
            Point point = new Point();
            point.X = (int)this.x;
            point.Y = (int)this.y;
            rectangle.Location = point;
            this.Bounds = rectangle;

            if (playerBounds.Intersects(this.Bounds))
            {
                this.HasBeenPickedUp = true;
            }

            if (this.HasBeenPickedUp)
            {
                this.x = playerX + 20;
                this.y = playerY - 25;
                point.X = (int)playerX;
                point.Y = (int)playerY;
                rectangle.Location = point;
                this.Bounds = rectangle;
            }
        }

        public Vector2 getRandomLocation()
        {
            //need a random location someone during the level, but not in the starter area
            Random random = new Random();

            //0 - 0
            //1 - 800
            //2 - 1600
            //3 - 2400
            //4 - 3200

            //0 - 0
            //1 - 480
            //2 - 960
            //3 - 1440
            //4 - 1920

            int randomX = random.Next(0, 2400 - itemWidth);
            int randomY = random.Next(0, 1440 - itemHeight);

            int count = 0;
            while( (randomX > 800 && randomX < 1600) && (randomY > 480 && randomY < 960) ) {
                randomX = random.Next(0, 2400 - itemWidth);
                randomY = random.Next(0, 14400 - itemHeight);
                count++;
            }

            //int randomX = 900;
            //int randomY = 500;

            return new Vector2(randomX, randomY);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ItemTexture, new Vector2(this.x, this.y), Color.White);
        }

        public Texture2D ItemTexture
        {
            get;
            set;
        }

        public Boolean HasBeenPickedUp
        {
            get;
            set;
        }

        public Rectangle Bounds
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
