using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ritual.Game
{
    class Player
    {
        Level level;

        private float speedX = 250.0f;
        private float speedY = 250.0f;
        private int playerWidth = 60;
        private int playerHeight = 80;

        private float pushBackTime = 0f;
        private float pushBackSpeed = 800f;
        private float pushBackLimit = 0.08f;
        private float timeSinceDamageTaken = 0f;
        private float timeSinceLastClick = 0f;
        private Boolean canClick = true;

        private int totalFrames;

        private MouseState oldState;

        /// <summary>
        /// Constructs a new player.
        /// </summary>
        public Player(Level level, float startX, float startY)
        {
            Rectangle r = new Rectangle();
            r.Width = playerWidth;
            r.Height = playerHeight;
            this.Bounds = r;

            Health = 3;

            this.level = level;
            this.x = startX;
            this.y = startY;

            Rows = 1;
            Columns = 5;
            CurrentFrame = 0;
            totalFrames = Rows * Columns;

            HasFinished = false;
            IsDead = false;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            float delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            Rectangle rectangle = this.Bounds;
            Point point = new Point();
            point.X = (int)this.x;
            point.Y = (int)this.y;
            rectangle.Location = point;
            this.Bounds = rectangle;

            if (this.Invincible)
            {
                timeSinceDamageTaken += (float) gameTime.ElapsedGameTime.TotalSeconds;

                if (timeSinceDamageTaken > 1.15f)
                {
                    timeSinceDamageTaken = 0f;
                    Invincible = false;
                }
            }

            if (this.PushBack)
            {
                if (this.pushBackTime < pushBackLimit)
                {
                    switch (PushBackDirection)
                    {
                        case 0:
                            //go left
                            this.x -= delta * pushBackSpeed;
                            break;
                        case 1:
                            //go right
                            this.x += delta * pushBackSpeed;
                            break;
                        case 2:
                            //go up
                            this.y -= delta * pushBackSpeed;
                            break;
                        case 3:
                            //go down
                            this.y += delta * pushBackSpeed;
                            break;
                    }

                    pushBackTime += delta;
                }
                else
                {
                    PushBack = false;
                    pushBackTime = 0f;
                }
            }

            GetInput(gameTime, keyboardState, mouseState);
        }

        public void GetInput(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!this.HasFinished && !this.IsDead) {
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    x += delta * speedX;
                    CurrentFrame = 2;

                    if ( (x + playerWidth) > (level.ColumnWidth * level.columns))
                    {
                        x = (level.ColumnWidth * level.columns) - playerWidth;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.A))
                {
                    x -= delta * speedX;
                    CurrentFrame = 1;

                    if (x < 0)
                    {
                        x = 0;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    y += delta * speedY;
                    CurrentFrame = 0;

                    if ((y + playerHeight) > (level.RowHeight * level.rows))
                    {
                        y = (level.RowHeight * level.rows) - playerHeight;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.W))
                {
                    y -= delta * speedY;
                    CurrentFrame = 3;

                    if (y < 0)
                    {
                        y = 0;
                    }
                }

                MouseState newState = mouseState;
                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released && canClick)
                {
                    canClick = false;
                    handleShooting(newState);
                }
                oldState = newState;

                if (!canClick)
                {
                    timeSinceLastClick += delta;
                    if (timeSinceLastClick > 0.2f)
                    {
                        canClick = true;
                        timeSinceLastClick = 0f;
                    }
                }
            }
        }

        public void handleShooting(MouseState newState)
        {
            int mouseX = (level.CurrentColumn * level.ColumnWidth) + newState.X;
            int mouseY = (level.CurrentRow * level.RowHeight) + newState.Y;

            Vector2 startPoint = new Vector2(this.x, this.y);
            Vector2 endPoint = new Vector2(mouseX, mouseY);
            float distance = Vector2.Distance(startPoint, endPoint);
            Vector2 direction = Vector2.Normalize(endPoint - startPoint); 

            //this ensures that the bullet won't disappear until its off screen
            distance += 800;

            if (level.BulletPool.Count > 0)
            {
                Bullet bullet = (Bullet)level.BulletPool[0];
                bullet.CurrentPosition = startPoint;
                bullet.StartPosition = startPoint;
                bullet.Direction = direction;
                bullet.Distance = distance;
                bullet.ReachedDestination = false;

                level.BulletPool.Remove(bullet);
                level.ActiveBullets.Add(bullet);
            }
            else
            {
                Bullet bullet = new Bullet(level);
                bullet.CurrentPosition = startPoint;
                bullet.StartPosition = startPoint;
                bullet.Direction = direction;
                bullet.Distance = distance;
                bullet.ReachedDestination = false;

                bullet.BulletTexture = level.BulletTexture;
                level.ActiveBullets.Add(bullet);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDead && !HasFinished)
            {
                int row = (int)((float)CurrentFrame / (float)Columns);
                int column = CurrentFrame % Columns;

                Rectangle sourceRectangle = new Rectangle(playerWidth * column, playerHeight * row, playerWidth, playerHeight);
                Rectangle destinationRectangle = new Rectangle((int)this.x, (int)this.y, playerWidth, playerHeight);
            
                spriteBatch.Draw(PlayerTexture, destinationRectangle, sourceRectangle, Color.White);
            }
            else
            {
                spriteBatch.Draw(DeadPlayerTexture, new Vector2(this.x, this.y), Color.White);
            }
        }

        public void setLevel(Level level)
        {
            this.level = level;
        }

        public Texture2D PlayerTexture
        {
            get;
            set;
        }

        public Texture2D DeadPlayerTexture
        {
            get;
            set;
        }

        public Boolean HasFinished
        {
            get;
            set;
        }

        public Boolean IsDead
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

        public int CurrentFrame
        {
            get;
            set;
        }

        public Boolean Invincible
        {
            get;
            set;
        }

        public Boolean PushBack
        {
            get;
            set;
        }

        public int PushBackDirection
        {
            get;
            set;
        }

        public int Health
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

        public Rectangle Bounds
        {
            get;
            set;
        }

        public Level Level
        {
            get { return level;  }
        }
    }
}
