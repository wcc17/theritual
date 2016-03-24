using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ritual.Game
{
    class Enemy
    {
        Level level;

        private float speedX = 90.0f;
        private float speedY = 90.0f;

        private int enemyWidth = 55;
        private int enemyHeight = 65;

        private int currentFrame;
        private int totalFrames;

        /// <summary> 
        /// Constructs a new enemy.
        /// </summary>
        public Enemy(Level level)
        {
            this.level = level;

            Rectangle r = new Rectangle();
            r.Width = enemyWidth;
            r.Height = enemyHeight;
            this.Bounds = r;

            Rows = 1;
            Columns = 2;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void Update(GameTime gameTime, float playerX, float playerY)
        {
            Rectangle rectangle = this.Bounds;
            Point point = new Point();
            point.X = (int)this.x;
            point.Y = (int)this.y;
            rectangle.Location = point;
            this.Bounds = rectangle;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //move toward player
            //when enemy touches player, player takes damage
            if (this.x < playerX)
            {
                this.x += delta * speedX;
                currentFrame = 1;
            }
            if (this.x > playerX)
            {
                this.x -= delta * speedX;
                currentFrame = 0;
            }
            if (this.y < playerY)
            {
                this.y += delta * speedY;
            }
            if (this.y > playerY)
            {
                this.y -= delta * speedY;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(enemyWidth * column, enemyHeight * row, enemyWidth, enemyHeight);
            Rectangle destinationRectangle = new Rectangle((int)this.x, (int)this.y, enemyWidth, enemyHeight);

            spriteBatch.Draw(EnemyTexture, destinationRectangle, sourceRectangle, Color.White);
        }

        public Texture2D EnemyTexture
        {
            get;
            set;
        }

        public int Rows
        {
            get;
            set;
        }

        public int Columns
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

        public Level Level
        {
            get { return level; }
        }

        public Rectangle Bounds
        {
            get;
            set;
        }
    }
}
