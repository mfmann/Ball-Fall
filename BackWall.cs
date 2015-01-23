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

    class BackWall : GameObject
    {

        //Size of the wall
        public float wallObjectWidth;
        public float wallObjectHeight;
        public String texturePath;
        public double timeToLive;
        private bool changing;
        private bool createdNextWall;
        private int background;
        public float wallMovementFactor = 1.4f / 1000;

        // Constructor.
        public BackWall(LabGame game, Vector2 startPos, double timeToLive, int background)
        {
            this.game = game;
            wallObjectWidth = 3 * (game.boundaryRight - game.boundaryLeft);
            wallObjectHeight = 2 * (game.boundaryRight - game.boundaryLeft);
            type = GameObjectType.Wall;
            if (background == 1)
            {
                texturePath = "back1.jpg";
                myModel = game.assets.GetModel("wall", CreateWallModel);
            }
            else if (background == 2)
            {
                texturePath = "back2.jpg";
                myModel = game.assets.GetModel("wall2", CreateWallModel);
            }
            else
            {
                texturePath = "back3.jpeg";
                myModel = game.assets.GetModel("wall3", CreateWallModel);
            }
            this.pos = new Vector3(startPos.X, startPos.Y, 1);
            GetParamsFromModel();

            this.timeToLive = timeToLive;
            this.background = background;
            changing = false;
            createdNextWall = false;
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            basicEffect.View = game.camera.View;
            basicEffect.World = Matrix.Translation(pos);

            if (timeToLive > 0) timeToLive -= gameTime.ElapsedGameTime.Milliseconds;
            
            if ((changing == true) && (createdNextWall == false))
            {
                if(background == 1) game.Add(new BackWall(game, new Vector2(0,-1*wallObjectHeight), 30000, 2));
                else if (background == 2) game.Add(new BackWall(game, new Vector2(0, -1 * wallObjectHeight), 30000, 3));
                else game.Add(new BackWall(game, new Vector2(0, -1 * wallObjectHeight), 30000, 1));
                createdNextWall = true;
            }

            if (changing == true) this.pos.Y += gameTime.ElapsedGameTime.Milliseconds * wallMovementFactor;

            if ((changing == false) && timeToLive <= 0) changing = true;

            if (pos.Y < 0) this.pos.Y += gameTime.ElapsedGameTime.Milliseconds * wallMovementFactor;

            if (pos.Y >= wallObjectHeight) game.gameObjects.Remove(this);
        }

        public MyModel CreateWallModel()
        {
            return game.assets.CreateTexturedWall(texturePath, new Vector2(wallObjectWidth, wallObjectHeight));
        }
    }
}
