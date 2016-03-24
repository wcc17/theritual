using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Ritual.Game
{
    class Level
    {
        ContentManager contentManager;

        int currentPlayer = 0;
        Texture2D activePlayerTexture;
        Texture2D inactivePlayerTexture;
        Texture2D deadPlayerTexture;
        Player player;
        Player inactivePlayer1;
        Player inactivePlayer2;
        Player inactivePlayer3;
        Player inactivePlayer4;

        Texture2D buttonTexture;
        Button button;

        Texture2D ghostTexture;
        Ghost ghost1;
        Ghost ghost2;
        Ghost ghost3;
        Ghost ghost4;
        Ghost ghost5;

        private Boolean spawnGhosts = false;

        Item ritualItem;
        Texture2D itemTexture1;
        Texture2D itemTexture2;
        Texture2D itemTexture3;
        Texture2D itemTexture4;
        Texture2D itemTexture5;

        private SpriteFont font;
        private SpriteFont largerFont;

        ArrayList enemyPool = new ArrayList();
        ArrayList activeEnemies = new ArrayList();
        Texture2D enemyTexture;

        Texture2D heartTexture;

        private int enemyWidth = 55;
        private int enemyHeight = 65;
        private int playerWidth = 60;
        private int playerHeight = 80;
        private int bulletWidth = 25;
        private int bulletHeight = 25;

        Texture2D[] backgrounds = new Texture2D[9];
        private int centerSpace;

        public Level(IServiceProvider serviceProvider)
        {
            CurrentRow = 1;
            CurrentColumn = 1;
            centerSpace = ((backgrounds.Length - 1) / 2);

            columns = 3;
            rows = 3;

            //1600 * 960

            //player 1 = 375, 140
            //player 2 = 490, 220
            //player 3 = 435, 330
            //player 4 = 310, 330
            //player 5 = 260, 220
            this.player = new Player(this, (CurrentColumn * ColumnWidth) + 375, (CurrentRow * RowHeight) + 140);
            this.inactivePlayer1 = new Player(this, (CurrentColumn * ColumnWidth) + 490, (CurrentRow * RowHeight) + 220);
            this.inactivePlayer2 = new Player(this, (CurrentColumn * ColumnWidth) + 435, (CurrentRow * RowHeight) + 330);
            this.inactivePlayer3 = new Player(this, (CurrentColumn * ColumnWidth) + 310, (CurrentRow * RowHeight) + 330);
            this.inactivePlayer4 = new Player(this, (CurrentColumn * ColumnWidth) + 260, (CurrentRow * RowHeight) + 220);

            this.ghost1 = new Ghost(this, (CurrentColumn * ColumnWidth) + 375, (CurrentRow * RowHeight) + 140);
            this.ghost2 = new Ghost(this, (CurrentColumn * ColumnWidth) + 490, (CurrentRow * RowHeight) + 220);
            this.ghost3 = new Ghost(this, (CurrentColumn * ColumnWidth) + 435, (CurrentRow * RowHeight) + 330);
            this.ghost4 = new Ghost(this, (CurrentColumn * ColumnWidth) + 310, (CurrentRow * RowHeight) + 330);
            this.ghost5 = new Ghost(this, (CurrentColumn * ColumnWidth) + 260, (CurrentRow * RowHeight) + 220);

            button = new Button(this, CurrentColumn * ColumnWidth + (ColumnWidth / 2) - 50, CurrentRow * RowHeight + (RowHeight / 2));

            ritualItem = new Item(this);

            Rectangle rectangle = new Rectangle();
            rectangle.Width = 295;
            rectangle.Height = 200;
            Point point = new Point();
            //point x = + 255
            //point y = + 190
            point.X = (CurrentColumn * ColumnWidth) + 255;
            point.Y = (CurrentRow * RowHeight) + 190;
            rectangle.Location = point;
            this.FireAreaBounds = rectangle;

            ActiveBullets = new ArrayList();
            BulletPool = new ArrayList();

            contentManager = new ContentManager(serviceProvider, "Content");

            LoadContent();
            InitializeItem(currentPlayer);
        }

        public void restartGame()
        {
            CurrentRow = 1;
            CurrentColumn = 1;

            this.player = new Player(this, (CurrentColumn * ColumnWidth) + 375, (CurrentRow * RowHeight) + 140);
            this.player.IsDead = false;
            this.player.Health = 3;
            this.inactivePlayer1 = new Player(this, (CurrentColumn * ColumnWidth) + 490, (CurrentRow * RowHeight) + 220);
            this.inactivePlayer2 = new Player(this, (CurrentColumn * ColumnWidth) + 435, (CurrentRow * RowHeight) + 330);
            this.inactivePlayer3 = new Player(this, (CurrentColumn * ColumnWidth) + 310, (CurrentRow * RowHeight) + 330);
            this.inactivePlayer4 = new Player(this, (CurrentColumn * ColumnWidth) + 260, (CurrentRow * RowHeight) + 220);

            player.PlayerTexture = activePlayerTexture;
            inactivePlayer1.PlayerTexture = inactivePlayerTexture;
            inactivePlayer2.PlayerTexture = inactivePlayerTexture;
            inactivePlayer3.PlayerTexture = inactivePlayerTexture;
            inactivePlayer4.PlayerTexture = inactivePlayerTexture;

            player.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer1.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer2.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer3.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer4.DeadPlayerTexture = deadPlayerTexture;

            this.ghost1 = new Ghost(this, (CurrentColumn * ColumnWidth) + 375, (CurrentRow * RowHeight) + 140);
            this.ghost2 = new Ghost(this, (CurrentColumn * ColumnWidth) + 490, (CurrentRow * RowHeight) + 220);
            this.ghost3 = new Ghost(this, (CurrentColumn * ColumnWidth) + 435, (CurrentRow * RowHeight) + 330);
            this.ghost4 = new Ghost(this, (CurrentColumn * ColumnWidth) + 310, (CurrentRow * RowHeight) + 330);
            this.ghost5 = new Ghost(this, (CurrentColumn * ColumnWidth) + 260, (CurrentRow * RowHeight) + 220);

            ghost1.GhostTexture = ghostTexture;
            ghost2.GhostTexture = ghostTexture;
            ghost3.GhostTexture = ghostTexture;
            ghost4.GhostTexture = ghostTexture;
            ghost5.GhostTexture = ghostTexture;

            ritualItem = new Item(this);
            ritualItem.ItemTexture = itemTexture1;
            InitializeItem(currentPlayer);

            ActiveBullets = new ArrayList();
            BulletPool = new ArrayList();
            activeEnemies = new ArrayList();
            enemyPool = new ArrayList();

            spawnGhosts = false;

            currentPlayer = 0;
        }

        public void LoadContent()
        {
            for (int i = 0; i < rows * columns; i++)
            {
                if ( i == centerSpace)
                {
                    backgrounds[i] = ContentManager.Load<Texture2D>("background-scaled");
                }
                else
                {
                    Random random = new Random();
                    int randomBackground = random.Next(1, 4);

                    switch (randomBackground)
                    {
                        case 1:
                            backgrounds[i] = ContentManager.Load<Texture2D>("background-other-scaled");
                            break;
                        case 2:
                            backgrounds[i] = ContentManager.Load<Texture2D>("background-other1-scaled");
                            break;
                        case 3:
                            backgrounds[i] = ContentManager.Load<Texture2D>("background-other2-scaled");
                            break;
                    }
                }
            }

            activePlayerTexture = ContentManager.Load<Texture2D>("person-texture-atlas");
            inactivePlayerTexture = ContentManager.Load<Texture2D>("person-inactive");
            enemyTexture = ContentManager.Load<Texture2D>("enemy-texture-atlas");
            BulletTexture = ContentManager.Load<Texture2D>("bullet");
            heartTexture = ContentManager.Load<Texture2D>("heart");

            itemTexture1 = ContentManager.Load<Texture2D>("bone");
            itemTexture2 = ContentManager.Load<Texture2D>("boot");
            itemTexture3 = ContentManager.Load<Texture2D>("stick");
            itemTexture4 = ContentManager.Load<Texture2D>("rock");
            itemTexture5 = ContentManager.Load<Texture2D>("ring");

            player.PlayerTexture = activePlayerTexture;
            inactivePlayer1.PlayerTexture = inactivePlayerTexture;
            inactivePlayer2.PlayerTexture = inactivePlayerTexture;
            inactivePlayer3.PlayerTexture = inactivePlayerTexture;
            inactivePlayer4.PlayerTexture = inactivePlayerTexture;

            deadPlayerTexture = ContentManager.Load<Texture2D>("person-active-dead");
            player.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer1.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer2.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer3.DeadPlayerTexture = deadPlayerTexture;
            inactivePlayer4.DeadPlayerTexture = deadPlayerTexture;

            ghostTexture = ContentManager.Load<Texture2D>("ghost");
            ghost1.GhostTexture = ghostTexture;
            ghost2.GhostTexture = ghostTexture;
            ghost3.GhostTexture = ghostTexture;
            ghost4.GhostTexture = ghostTexture;
            ghost5.GhostTexture = ghostTexture;

            buttonTexture = ContentManager.Load<Texture2D>("restart-button");
            button.ButtonTexture = buttonTexture;

            font = ContentManager.Load<SpriteFont>("sprite-font");
            largerFont = ContentManager.Load<SpriteFont>("larger-sprite-font");
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            for(int i = 0; i < ActiveBullets.Count; i++)
            {
                Bullet bullet = (Bullet)ActiveBullets[i];
                handleBullets(bullet, gameTime);
            }

            foreach (Enemy enemy in activeEnemies)
            {
                handleEnemies(enemy, gameTime);
            }

            player.Update(gameTime, keyboardState, mouseState);

            ritualItem.Update(gameTime, player.Bounds, player.x, player.y);

            updateBackground();

            if (player.Bounds.Intersects(this.FireAreaBounds) && ritualItem.HasBeenPickedUp)
            {
                //put item on side of screen to show that is been picked up
                //or use a number. the items would be cooler
                //switch to the next inactive player

                if (!player.HasFinished) {
                    switchPlayers();

                    ritualItem.HasBeenPickedUp = false;

                    InitializeItem(currentPlayer);
                }
            }

            if (spawnGhosts)
            {
                ghost1.Update(gameTime);
                ghost2.Update(gameTime);
                ghost3.Update(gameTime);
                ghost4.Update(gameTime);
                ghost5.Update(gameTime);
            }


            if (button.Update(mouseState) && (player.IsDead || player.HasFinished))
            {
                restartGame();
            }
        }

        public void InitializeItem(int itemNumber)
        {
            Vector2 itemPosition = ritualItem.getRandomLocation();
            ritualItem.x = itemPosition.X;
            ritualItem.y = itemPosition.Y;

            switch (itemNumber)
            {
                case 0:
                    ritualItem.ItemTexture = itemTexture1;
                    break;
                case 1:
                    ritualItem.ItemTexture = itemTexture2;
                    break;
                case 2:
                    ritualItem.ItemTexture = itemTexture3;
                    break;
                case 3:
                    ritualItem.ItemTexture = itemTexture4;
                    break;
                case 4:
                    ritualItem.ItemTexture = itemTexture5;
                    break;
            }

            Console.WriteLine("Item position: (" + ritualItem.x + ", " + ritualItem.y + ")");
        }

        public void switchPlayers()
        {
            currentPlayer++;

            //this.player = new Player(this, (CurrentColumn * ColumnWidth) + 375, (CurrentRow * RowHeight) + 140);
            //this.inactivePlayer1 = new Player(this, (CurrentColumn * ColumnWidth) + 490, (CurrentRow * RowHeight) + 220);
            //this.inactivePlayer2 = new Player(this, (CurrentColumn * ColumnWidth) + 435, (CurrentRow * RowHeight) + 330);
            //this.inactivePlayer3 = new Player(this, (CurrentColumn * ColumnWidth) + 310, (CurrentRow * RowHeight) + 330);
            //this.inactivePlayer4 = new Player(this, (CurrentColumn * ColumnWidth) + 260, (CurrentRow * RowHeight) + 220);

            Player oldPlayer = player;
            oldPlayer.PlayerTexture = inactivePlayerTexture;
            switch (currentPlayer)
            {
                case 1:
                    player = inactivePlayer1;
                    player.Health = oldPlayer.Health;
                    inactivePlayer1 = oldPlayer;
                    inactivePlayer1.x = (CurrentColumn * ColumnWidth) + 375;
                    inactivePlayer1.y = (CurrentRow * RowHeight) + 140;
                    inactivePlayer1.CurrentFrame = 0;
                    break;
                case 2:
                    player = inactivePlayer2;
                    player.Health = oldPlayer.Health;
                    inactivePlayer2 = oldPlayer;
                    inactivePlayer2.x = (CurrentColumn * ColumnWidth) + 490;
                    inactivePlayer2.y = (CurrentRow * RowHeight) + 220;
                    inactivePlayer2.CurrentFrame = 0;
                    break;
                case 3:
                    player = inactivePlayer3;
                    player.Health = oldPlayer.Health;
                    inactivePlayer3 = oldPlayer;
                    inactivePlayer3.x = (CurrentColumn * ColumnWidth) + 435;
                    inactivePlayer3.y = (CurrentRow * RowHeight) + 330;
                    inactivePlayer3.CurrentFrame = 0;
                    break;
                case 4:
                    player = inactivePlayer4;
                    player.Health = oldPlayer.Health;
                    inactivePlayer4 = oldPlayer;
                    inactivePlayer4.x = (CurrentColumn * ColumnWidth) + 310;
                    inactivePlayer4.y = (CurrentRow * RowHeight) + 330;
                    inactivePlayer4.CurrentFrame = 0;
                    break;
                case 5:
                    //player stops having control here
                    player.x = (CurrentColumn * ColumnWidth) + 260;
                    player.y = (CurrentRow * RowHeight) + 220;
                    spawnGhosts = true;
                    player.CurrentFrame = 0;

                    player.HasFinished = true;
                    inactivePlayer1.HasFinished = true;
                    inactivePlayer2.HasFinished = true;
                    inactivePlayer3.HasFinished = true;
                    inactivePlayer4.HasFinished = true;
                    break;
            }

            if (!player.HasFinished)
            {
                player.PlayerTexture = activePlayerTexture;
            }
        }

        public void handleEnemies(Enemy enemy, GameTime gameTime)
        {
            enemy.Update(gameTime, player.x, player.y);

            if (player.Bounds.Intersects(enemy.Bounds) && !player.Invincible)
            {
                handlePlayerPushBack(enemy);

                player.Health -= 1;
                player.Invincible = true;

                if (player.Health < 1)
                {
                    player.IsDead = true;
                }
            }
        }

        public void handlePlayerPushBack(Enemy enemy) 
        {
            Rectangle rectangle = Rectangle.Intersect(player.Bounds, enemy.Bounds);
            Console.WriteLine("Left: " + rectangle.Left);
            Console.WriteLine("Right: " + rectangle.Right);
            Console.WriteLine("Center: " + rectangle.Center);
            Console.WriteLine("Bottom: " + rectangle.Bottom);
            Console.WriteLine("Top: " + rectangle.Top);
            Console.WriteLine("Width: " + rectangle.Width);
            Console.WriteLine("Height: " + rectangle.Height);

            player.PushBack = true;
            if (rectangle.Left < player.Bounds.Right)
            {
                //collision from the right, so player should be forced to the left
                player.PushBackDirection = 0;
            }
            if (rectangle.Right > player.Bounds.Left)
            {
                //collision from the left, so player should be forced right
                player.PushBackDirection = 1;
            }
            if (rectangle.Top > player.Bounds.Bottom)
            {
                //collision from the bottom, so player should be forced up
                player.PushBackDirection = 2;
            }
            if (rectangle.Bottom < player.Bounds.Top)
            {
                //collision from the top, so player should be forced down
                player.PushBackDirection = 3;
            }
        }

        public void handleBullets(Bullet bullet, GameTime gameTime)
        {
            bullet.Update(gameTime);

            //delete bullet or see if it has intersected with an enemy
            if (bullet.ReachedDestination)
            { 
                BulletPool.Add(bullet);
                ActiveBullets.Remove(bullet);
            }
            else
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    Enemy enemy = (Enemy)activeEnemies[i];

                    if (enemy.Bounds.Intersects(bullet.Bounds))
                    {
                        enemyPool.Add(enemy);
                        activeEnemies.Remove(enemy);

                        BulletPool.Add(bullet);
                        ActiveBullets.Remove(bullet);
                    }
                }
            }
        }

        public void updateBackground() 
        {
            Boolean screenChange = false;

            if ((player.x + playerWidth) < (CurrentColumn * ColumnWidth))
            {
                if (CurrentColumn > 0)
                {
                    CurrentColumn--;
                    screenChange = true;
                }
            }
            if (player.x > ((CurrentColumn * ColumnWidth) + ColumnWidth))
            {
                if (CurrentColumn < 2)
                {
                    CurrentColumn++;
                    screenChange = true;
                }
            }

            if ((player.y + playerHeight) < (CurrentRow * RowHeight))
            {
                if (CurrentRow > 0)
                {
                    CurrentRow--;
                    screenChange = true;
                }
            }
            if (player.y > ((CurrentRow * RowHeight) + RowHeight))
            {
                if (CurrentRow < 2)
                {
                    CurrentRow++;
                    screenChange = true;
                }
            }

            if (screenChange)
            {
                //to ensure that a monster doesn't spawn on top of the player when going to a new screen
                player.Invincible = true;

                //if we're on the center screen
                if (CurrentColumn == 1 && CurrentRow == 1)
                {
                    //get rid of all active enemies
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        enemyPool.Add(activeEnemies[i]);
                    }
                    activeEnemies.Clear();
                }
                else
                {
                    updateMonsterPositions();
                }
                screenChange = false;
            }
        }

        public void updateMonsterPositions()
        {
            Random random = new Random();

            int numberOfMonsters = random.Next(4, 9);

            //clear active monsters and add them back to the pool
            foreach (Enemy enemy in activeEnemies)
            {
                enemyPool.Add(enemy);
            }

            activeEnemies.Clear();

            for (int k = 0; k < numberOfMonsters; k++) {
                int randomX = random.Next(CurrentColumn * ColumnWidth, (CurrentColumn + 1) * ColumnWidth);
                int randomY = random.Next(CurrentRow * RowHeight, (CurrentRow + 1) * RowHeight);

                if (enemyPool.Count > 0)
                {
                    Enemy enemy = (Enemy)enemyPool[0];
                    enemy.x = randomX;
                    enemy.y = randomY;

                    enemyPool.Remove(enemy);
                    activeEnemies.Add(enemy);

                }
                else
                {
                    Enemy enemy = new Enemy(this);
                    enemy.x = randomX;
                    enemy.y = randomY;
                    enemy.EnemyTexture = enemyTexture;

                    activeEnemies.Add(enemy);
                }
            }

            //Console.WriteLine("Player position: (" + player.x + ", " + player.y + ")");
            //Console.WriteLine("Current Position: (" + CurrentColumn * 800 + ", " + CurrentRow * 480 + ")");
            //Console.WriteLine("Number of active enemies: " + activeEnemies.Count);
            //foreach (Enemy enemy in activeEnemies)
            //{
            //    Console.WriteLine("Enemy Position: (" + enemy.x + ", " + enemy.y + ")");
            //}

            //Console.WriteLine("Number of inactive enemies: " + enemyPool.Count);
            //foreach (Enemy enemy in enemyPool)
            //{
            //    Console.WriteLine("Inactive Enemy Position: (" + enemy.x + ", " + enemy.y + ")");
            //}

            //Console.WriteLine();
            //Console.WriteLine();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int currentBackgroundIndex = 0;

            for(int i = 0; i < rows; i++) {
                int row = i;

                for (int j = 0; j < columns; j++)
                {
                    int column = j;

                    //if statement here ensures that only the spaces one away from the current space are being drawn
                    if( (column >= (CurrentColumn - 1) && column <= CurrentColumn + 1)  && (row >= CurrentRow - 1 && row <= CurrentRow + 1) ) {
                        spriteBatch.Draw(backgrounds[currentBackgroundIndex], new Vector2(ColumnWidth * column, RowHeight * row));
                    } 
                    
                    currentBackgroundIndex++;
                }
            }

            foreach(Enemy enemy in activeEnemies) {
                enemy.Draw(gameTime, spriteBatch);
            }

            foreach (Bullet bullet in ActiveBullets)
            {
                bullet.Draw(gameTime, spriteBatch);
            }

            inactivePlayer1.Draw(gameTime, spriteBatch);
            inactivePlayer2.Draw(gameTime, spriteBatch);
            inactivePlayer3.Draw(gameTime, spriteBatch);
            inactivePlayer4.Draw(gameTime, spriteBatch);

            if (player.HasFinished)
            {
                spriteBatch.DrawString(largerFont, "RITUAL COMPLETE", new Vector2( (ColumnWidth * CurrentColumn) + (ColumnWidth / 2) - 220, (RowHeight * CurrentRow) + (RowHeight / 2) - 180), new Color(167, 157, 110));
            }

            if (player.IsDead)
            {
                spriteBatch.DrawString(largerFont, "GAME OVER", new Vector2((ColumnWidth * CurrentColumn) + (ColumnWidth / 2) - 180, (RowHeight * CurrentRow) + (RowHeight / 2) - 180), new Color(167, 157, 110));
            }

            player.Draw(gameTime, spriteBatch);

            if (spawnGhosts)
            {
                ghost1.Draw(gameTime, spriteBatch);
                ghost2.Draw(gameTime, spriteBatch);
                ghost3.Draw(gameTime, spriteBatch);
                ghost4.Draw(gameTime, spriteBatch);
                ghost5.Draw(gameTime, spriteBatch);
            }

            if (!player.HasFinished)
            {
                ritualItem.Draw(gameTime, spriteBatch);
            }

            for (int i = 1; i <= player.Health; i++)
            {
                spriteBatch.Draw(heartTexture, new Vector2( (CurrentColumn * ColumnWidth) + (15 * i) , (CurrentRow * RowHeight) + 5 ));
            }

            if (player.HasFinished || player.IsDead)
            {
                button.x = CurrentColumn * ColumnWidth + (ColumnWidth / 2) - 40;
                button.y = CurrentRow * RowHeight + (RowHeight / 2);
                button.Draw(gameTime, spriteBatch);
            }
        }

        public int CurrentRow
        {
            get;
            set;
        }

        public int CurrentColumn
        {
            get;
            set;
        }

        public int ColumnWidth {
            get { return 800;  }
        }

        public int RowHeight
        {
            get { return 480; }
        }

        public Rectangle FireAreaBounds
        {
            get;
            set;
        }

        public ArrayList BulletPool
        {
            get;
            set;
        }

        public ArrayList ActiveBullets
        {
            get;
            set;
        }

        public Texture2D BulletTexture
        {
            get;
            set;
        }

        public int columns
        {
            get;
            set;
        }

        public int rows
        {
            get;
            set;
        }

        public ContentManager ContentManager
        {
            get { return contentManager; }
        }
    }
}
