using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Final
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D shipTexture;
        Rectangle shipRectangle;
        Random rnd;
        int velocityX = 0;
        int velocityY = 0;
        int random;
        int timer;
        int timer1;
        int lives = 3;
        private List<Enemy> enemies;
        private List<Shot> shots;
        private SpriteFont font;
        private int score = 0;
        private int spawn = 30;
        private bool pressReady; // for shots so you cant spam them


        public Game1()
        {
            rnd = new Random();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            enemies = new List<Enemy>();
            shots = new List<Shot>();
            graphics.PreferredBackBufferWidth = 450;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 650;   // set this value to the desired height of your window
            graphics.ApplyChanges();

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

            timer = 0;
            timer1 = 15;

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
            shipTexture = Content.Load<Texture2D>("ship");
            shipRectangle = new Rectangle(225 - shipTexture.Width / 10, 650 - shipTexture.Height / 5, shipTexture.Width / 5, shipTexture.Height / 5);
            Enemy.texture = Content.Load<Texture2D>("enemy");
            Shot.texture = Content.Load<Texture2D>("shot");
            font = Content.Load<SpriteFont>("Score");


            // TODO: use this.Content to load your game content here
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
            int loop = 0, loop1 = 0;

            // TODO: Add your update logic here
            KeyboardState key = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            if (shipRectangle.X >= 445 - shipTexture.Width / 5)
            {
                shipRectangle.X = 445 - shipTexture.Width / 5;
            }
            else if (shipRectangle.X <= 0)
            {
                shipRectangle.X = 0;
            }

            if (shipRectangle.Y >= 645 - shipTexture.Width / 5)
            {
                shipRectangle.Y = 645 - shipTexture.Width / 5;
            }
            else if (shipRectangle.Y <= 0)
            {
                shipRectangle.Y = 0;
            }





            if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                velocityX--;
            }
            else if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                velocityX++;

            }
            else
            {
                if (velocityX > 0)
                    velocityX--;
                else if (velocityX < 0)
                    velocityX++;
                else
                    velocityX = 0;
            }
            if (velocityX > 5)
                velocityX = 5;
            else if (velocityX < -5)
                velocityX = -5;

            shipRectangle.X += velocityX;

            if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                velocityY--;
            }
            else if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                velocityY++;

            }
            else
            {
                if (velocityY > 0)
                    velocityY--;
                else if (velocityY < 0)
                    velocityY++;
                else
                    velocityY = 0;
            }
            if (velocityY > 5)
                velocityY = 5;
            else if (velocityY < -5)
                velocityY = -5;

            shipRectangle.Y += velocityY;

            base.Update(gameTime);

            random = rnd.Next(0, 380);

            timer++;
            timer1++;
            if (score > 60)
            {
                spawn = 25;
            }
            else if (score > 120)
            {
                spawn = 10;
            }

            if (timer % spawn == 0 && score > 30)
            {
                //enemies.Add(new Enemy(0, random, 610));
                enemies.Add(new MoveEnemy(0, random, 610));
            }

            if (timer1 % spawn == 0)
            {
                enemies.Add(new Enemy(0, random, 610));
            }

            if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && pressReady)
            {
                shots.Add(new Shot(shipRectangle.Y, shipRectangle.X + 8, 0));
                pressReady = false;
            }
            if (key.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                pressReady = true;
            }

            

            loop1 = 0;
            while (loop1 < shots.Count)
            {
                if (!shots[loop1].active())
                {
                    shots.RemoveAt(loop1);
                }
                else
                {
                    for (loop = 0; loop < enemies.Count; loop++)
                    {
                        if (loop1 < shots.Count && shots[loop1].didHit(enemies[loop].rectangle()))
                        {
                            enemies.RemoveAt(loop);
                            shots.RemoveAt(loop1);
                            score++;
                        }

                    }
                }
                loop1++;
            }

            loop = 0;
            while (loop < enemies.Count)
            {
                if (!enemies[loop].alive())
                {
                    enemies.RemoveAt(loop);
                }
                else
                {
                    if (enemies[loop].didHit(shipRectangle))
                    {
                        enemies.RemoveAt(loop);
                        lives--;
                        if (lives == 0)
                        {
                            HighScoreManager highScoreManager = new HighScoreManager();
                            int playerScore = score;
                            highScoreManager.RecordHighScore(playerScore);
                            Exit();
                        }
                    }
                    loop++;
                }

            }



        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Navy);


            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(shipTexture, shipRectangle, Color.White);
            foreach (Enemy enemy in enemies)
            {
                enemy.draw(spriteBatch);
                enemy.update();
            }
            foreach (Shot shot in shots)
            {
                shot.draw(spriteBatch);
                shot.update();
            }

            //draw score and lives
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 10), Color.White);
            spriteBatch.DrawString(font, "Lives: " + lives, new Vector2(0, 30), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    class Enemy
    {
        private const int WIDTH = 70, HEIGHT = 40;
        private double currentSpeed = 5;
        private int startY, endY;
        protected Rectangle enemyRectangle;
        private static Texture2D enemyTexture;
        private bool isAlive;
        private double enemySpeed = 0.3;

        public Enemy(int yStart, int X, int yEnd)
        {
            isAlive = true;
            enemyRectangle = new Rectangle(X, yStart, WIDTH, HEIGHT);
            startY = yStart;
            endY = yEnd;
        }

        public static Texture2D texture
        {
            get
            {
                return enemyTexture;
            }
            set
            {
                enemyTexture = value;
            }


        }

        public bool didHit(Rectangle theObject)
        {
            return enemyRectangle.Intersects(theObject);
        }

        public int Y
        {
            get
            {
                int temp = -999;
                if (isAlive)
                {
                    temp = enemyRectangle.Y;
                }
                return temp;
            }
            set
            {
                enemyRectangle.Y = value;
            }
        }
        public bool alive()
        {
            return isAlive;
        }

        public Rectangle rectangle()
        {
            return enemyRectangle;
        }

        public virtual void update()
        {
            if (isAlive)
            {

                if (enemyRectangle.Y > endY)
                {
                    isAlive = false;
                }
                enemyRectangle.Y += (int)currentSpeed;
                currentSpeed += enemySpeed;
            }

        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                spriteBatch.Draw(enemyTexture, enemyRectangle, Color.White);
            }
        }


    }

    class Shot
    {
        private const int SPEED = 10, WIDTH = 28, HEIGHT = 30;
        private int startY, endY;
        private Rectangle shotRectangle;
        private static Texture2D shotTexture;
        private bool isActive;

        public Shot(int yStart, int X, int yEnd)
        {
            isActive = true;
            shotRectangle = new Rectangle(X, yStart, WIDTH, HEIGHT);
            startY = yStart;
            endY = yEnd;
        }

        public static Texture2D texture
        {
            get
            {
                return shotTexture;
            }
            set
            {
                shotTexture = value;
            }


        }
        public bool didHit(Rectangle theObject)
        {
            return shotRectangle.Intersects(theObject);
        }

        public int Y
        {
            get
            {
                int temp = -999;
                if (isActive)
                {
                    temp = shotRectangle.Y;
                }
                return temp;
            }
            set
            {
                shotRectangle.Y = value;
            }
        }
        public bool active()
        {
            return isActive;
        }
        public void update()
        {
            if (isActive)
            {

                if (shotRectangle.Y < endY)
                {
                    isActive = false;
                }
                shotRectangle.Y -= SPEED;
            }

        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.Draw(shotTexture, shotRectangle, Color.White);
            }
        }


    }

    class MoveEnemy : Enemy
    {
        private double speed;
        private int dir;
        private int timer1;

        public MoveEnemy(int yStart, int X, int yEnd) : base(yStart, X, yEnd)
        {
            speed = 5;
            dir = 1;
        }
        public override void update()
        {
            timer1++;
            if (timer1 % 20 == 0)
            {
                dir = dir * -1;
            }

            enemyRectangle.X += (dir * (int)speed);

            if (enemyRectangle.X > 450 - enemyRectangle.Width)
            {
                dir = -1;
            }
            else if (enemyRectangle.X < 0)
            {
                dir = 1;
            }

            base.update();
        }
    }

    public class HighScoreManager
    {
        private List<string> highScores;

        public HighScoreManager()
        {
            highScores = LoadHighScores();
        }

        public void RecordHighScore(int score)
        {
            string name = PromptForName();

            // Add the high score to the list
            highScores.Add(name + " - " + score);

            SaveHighScores();

            ShowHighScores();
        }

        private string PromptForName()
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Enter your name:", "High Score", "");

            return name;
        }

        private void SaveHighScores()
        {
            // Save high scores to a file or any other destination
            string filePath = "highscore.txt";

            highScores.Sort((a, b) => GetScoreValue(b).CompareTo(GetScoreValue(a)));
            if (highScores.Count > 10)
            {
                highScores = highScores.GetRange(0, 10);
            }

            File.WriteAllText(filePath, string.Empty);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                foreach (string score in highScores)
                {
                    writer.WriteLine(score);
                }
                writer.Close();
            }

        }

        public void ShowHighScores()
        {
            string scoresText = "High Scores:\n\n";

            foreach (string score in highScores)
            {
                scoresText += score + "\n";
            }

            System.Windows.Forms.MessageBox.Show(scoresText);
        }

        private List<string> LoadHighScores()
        {
            string filePath = "highscore.txt";
            List<string> scores = new List<string>();

            if (File.Exists(filePath))
            {
                scores = File.ReadAllLines(filePath).ToList();
            }

            return scores;
        }

        private int GetScoreValue(string scoreEntry)
        {
            int separatorIndex = scoreEntry.LastIndexOf('-');
            string scoreValue = scoreEntry.Substring(separatorIndex + 1).Trim();
            int score = int.Parse(scoreValue);

            return score;
            
        }
    }
}










