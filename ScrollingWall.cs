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
    class ScrollingWall : GameObject
    {
        //Size of the wall
        public float wallObjectWidth;
        public float wallObjectHeight;
        public float wallWidth;
        public float wallHeight;

        //Storing the blocks
        private List<Block> blocks;

        // Spacing and counts.
        private static int block_wallHeight = 8; //Wall height in platforms
        private static int platformSpacing = 3;

        //Random Normal
        private double startIndexMean; //The mean index of missing blocks per platform
        private double startIndexStdDev; //The standard deviation of the index of missing blocks per platform
        Random rand; //Random number generator


        static public  float blockMovementFactor = 0.4f / 1000;

       
        // Constructor.
        public ScrollingWall(LabGame game)
        {
            this.game = game;
            wallWidth = (game.boundaryRight - game.boundaryLeft);
            wallHeight = (game.boundaryRight - game.boundaryLeft);
            wallObjectWidth = 3 * (game.boundaryRight - game.boundaryLeft);
            wallObjectHeight = 2 * (game.boundaryRight - game.boundaryLeft);

            rand = new Random();
            blocks = new List<Block>();

            startIndexMean = game.boundaryLeft + (wallWidth / 2);
            startIndexStdDev = 1;



            for (int i = 0; i < block_wallHeight; i++) createNewBlock();
        }

        private void createNewBlock()
        {
            //Get which indexes will start the group
            float blockX = (float)(getRandomNormal(startIndexMean, startIndexStdDev));
            if (blockX >= (game.boundaryRight)) blockX = game.boundaryRight;
            else if (blockX < game.boundaryLeft) blockX = game.boundaryLeft;

            Block block = new Block(game, blockX, -(blocks.Count) * platformSpacing);
            game.Add(block);
            blocks.Add(block);

            int nextMean = rand.Next(2);
            if (startIndexMean == game.boundaryLeft + (wallWidth / 2))
            {
                if (nextMean == 0) startIndexMean = game.boundaryLeft + (wallWidth / 4);
                else startIndexMean = game.boundaryLeft + (3 * wallWidth / 4);
            }
            else if (startIndexMean == game.boundaryLeft + (wallWidth / 4))
            {
                if (nextMean == 0) startIndexMean = game.boundaryLeft + (wallWidth / 2);
                else startIndexMean = game.boundaryLeft + (3 * wallWidth / 4);
            }
            else
            {
                if (nextMean == 0) startIndexMean = game.boundaryLeft + (wallWidth / 4);
                else startIndexMean = game.boundaryLeft + (wallWidth / 2);
            }
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
            //stepTimer -= gameTime.ElapsedGameTime.Milliseconds;
            //if (stepTimer <= 0)
            //{
            toRemove = stepUp(gameTime);
              //  stepTimer = stepWait;
            //}
            if (toRemove) removeBlock();
            System.Diagnostics.Debug.WriteLine("Update end = " + DateTime.Now.Millisecond);
        }

        // Step all enemies down one.
        private Boolean stepUp(GameTime gameTime)
        {   
            
            Boolean toRemove = false;

            for (int i = 0; i < blocks.Count; i++)
            {
                Block block = blocks[i];
                block.pos.Y += game.difficulty*gameTime.ElapsedGameTime.Milliseconds*blockMovementFactor;
                if (block.pos.Y > game.boundaryTop) { toRemove = true; }
            }

            return toRemove;
        }

        private void removeBlock()
        {
            game.Remove(blocks[0]);
            blocks.Remove(blocks[0]);
            createNewBlock();
        }

        // Method for when the game ends.
        private void gameOver()
        {
            game.Exit();
        }
    }
}