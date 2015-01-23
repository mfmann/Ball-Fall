using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;


    // Enemy Controller class.
    class EnemyController : GameObject
    {
        //Storing the blocks
        private List<Block[]> blockRows;

        // Spacing and counts.
        private static int wallWidth = 14; //Wall width in blocks
        private static int wallHeight = 8; //Wall height in platforms
        private static int platformSpacing = 3; //Number of blocks between each platform
        private static int platformLength = 3;

        // Timing and movement.
        private float stepSize = 0.03f;
        private float stepWait = 0;
        private float stepTimer = 0;

        //Random Normal
        private double startIndexMean; //The mean index of missing blocks per platform
        private double startIndexStdDev; //The standard deviation of the index of missing blocks per platform
        Random rand; //Random number generator
        

        static public float blockMovementFactor = 0.4f / 1000;

       
        // Constructor.
        public EnemyController(LabGame game)
        {//Vector3.
            this.game = game;
            rand = new Random();
            blockRows = new List<Block[]>();

            startIndexMean = wallWidth / 4;
            startIndexStdDev = 1;



            for (int i = 0; i < wallHeight; i++) createNewBlockRow();
        }

        private void createNewBlockRow()
        {
            Block[] row = new Block[wallWidth];
            for (int i = 0; i < wallWidth; i++)
            {
                row[i] = null; ;
            }

            //Get which indexes will start the group
            int startIndex = (int)(getRandomNormal(startIndexMean - platformLength / 2, startIndexStdDev));
            if (startIndex >= (wallWidth - platformLength)) startIndex = wallWidth - 1 - platformLength;
            else if (startIndex < 0) startIndex = 0;

            for (int i = startIndex; i < platformLength + startIndex; i++)
            {
                row[i] = new Block(game, i - (wallWidth / 2), -(blockRows.Count) * platformSpacing);
            }

            //Fill the game with blocks
            for (int i = 0; i < wallWidth; i++)
            {
                if (row[i] != null) game.Add(row[i]);
            }

            int nextMean = rand.Next(2);
            if (startIndexMean == wallWidth / 2)
            {
                if (nextMean == 0) startIndexMean = wallWidth / 4;
                else startIndexMean = 3 * wallWidth / 4;
            }
            else if (startIndexMean == wallWidth / 4)
            {
                if (nextMean == 0) startIndexMean = wallWidth / 2;
                else startIndexMean = 3 * wallWidth / 4;
            }
            else
            {
                if (nextMean == 0) startIndexMean = wallWidth / 4;
                else startIndexMean = wallWidth / 2;
            }

            //Add row to Block rows
            blockRows.Add(row);
        }

        //Courtesy of StackOverflow user "yoyoyoyosef": http://stackoverflow.com/users/25571/yoyoyoyosef
        private double getRandomNormal(double mean, double stdDev)
        {
            double v1;//these are uniform (0,1) random doubles
            double v2;

            v1 = rand.NextDouble();
            v2 = rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(v1)) * Math.Sin(2.0 * Math.PI * v2); //random normal(0,1)

            double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }


        // Frame update method.
        public override void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine("Update start = " + DateTime.Now.Millisecond);
            System.Diagnostics.Debug.WriteLine("Block count = "+game.gameObjects.Count);
            // Move the enemies a step once the step timer has run out and reset step timer.
            // TASK 3: Moves according to game difficulty.  N.B.  This is a simple way of achieving this, you may prefer to impletement
            // SetDifficulty methods in a manner similar to Tapped if you were to do more complicated things with the difficulty
            Boolean toRemove = false;
            stepTimer -= gameTime.ElapsedGameTime.Milliseconds;
            //if (stepTimer <= 0)
            //{
                toRemove = stepUp(gameTime);
              //  stepTimer = stepWait;
            //}
            if (toRemove) removeRow();
            System.Diagnostics.Debug.WriteLine("Update end = " + DateTime.Now.Millisecond);
        }

        // Step all enemies down one.
        private Boolean stepUp(GameTime gameTime)
        {   
            
            Boolean toRemove = false;

            for (int i = 0; i < blockRows.Count; i++)
            {
                for (int j = 0; j < wallWidth; j++)
                {
                    Block block = blockRows[i][j];
                    if (block != null)
                    {
                        block.pos.Y += game.difficulty*gameTime.ElapsedGameTime.Milliseconds*blockMovementFactor;
                        if (block.pos.Y > game.boundaryTop) { toRemove = true; }
                    }
                }
            }

            return toRemove;
        }

        private void removeRow()
        {
            for (int i = 0; i < wallWidth; i++)
            {
                game.Remove(blockRows[0][i]);
            }
            blockRows.Remove(blockRows[0]);
            createNewBlockRow();
        }

        // Method for when the game ends.
        private void gameOver()
        {
            game.Exit();
        }
    }
}