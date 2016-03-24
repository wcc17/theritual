using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ritual.Game
{
    class Button
    {
        Level level;

        private int buttonWidth = 100;
        private int buttonHeight = 50;

        private MouseState oldState;

        public Button(Level level, float x, float y) 
        {
            this.level = level;

            Rectangle r = new Rectangle();
            r.Width = buttonWidth;
            r.Height = buttonHeight;
            Point point = new Point();
            point.X = (int) x;
            point.Y = (int) y;
            r.Location = point;
            this.Bounds = r;

            this.x = x;
            this.y = y;
        }

        public Boolean Update(MouseState mouseState)
        {
            Rectangle r = new Rectangle();
            r.Width = buttonWidth;
            r.Height = buttonHeight;
            Point point = new Point();
            point.X = (int)x;
            point.Y = (int)y;
            r.Location = point;
            this.Bounds = r;

            MouseState newState = mouseState;
            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                int mouseX = (level.CurrentColumn * level.ColumnWidth) + newState.X;
                int mouseY = (level.CurrentRow * level.RowHeight) + newState.Y;

                if( (mouseX > Bounds.Location.X) 
                    && (mouseX < Bounds.Location.X + Bounds.Width) 
                    && (mouseY > Bounds.Location.Y
                    && (mouseY < Bounds.Location.Y + Bounds.Height))) {
                    Console.WriteLine("Button clicked");
        
                    oldState = newState;
                    return true;
                } 
                else 
                {
                    oldState = newState;
                    return false;
                }
            } 
            else 
            {
                oldState = newState;
                return false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ButtonTexture, new Vector2(this.x, this.y), Color.White);
        }

        public Texture2D ButtonTexture
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
