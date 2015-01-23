using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
//ai enemy
    class Chaser : GameObject
    {
  

        //private float speed = 0.006f;
        private float projectileSpeed = 20;
        Vector2 velocity = new Vector2(0,0);
        float gravity = -0.0000009f;//-0.0001f;
        float accelerometerScaling = 0.00005f;
        public float radius = 0.3f;
        private PathFinding.ArrayNodeController pathingMapHandler;
        float jumpScaling = 0.00006f;
        const float minTheta = (float)Math.PI / 4;
            long lastJumpTime=0;
            long maxLastJumpTime = 100;
            float speedFactor = 0.002f / 1000;
        public Chaser(LabGame game,Vector3 pos)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("chaser", CreateChaserModel);
            this.pos = pos;
            GetParamsFromModel();
            pathingMapHandler = new PathFinding.ArrayNodeController(100, 100, this.game.boundaryRight, this.game.boundaryTop, 0, 0);//TODO needs to be fixed
        }

        public MyModel CreateChaserModel()
        {
            return game.assets.CreateBall(new Vector3(radius,radius,radius),Color.BlueViolet);

        }

  


        // Frame update.
        public override void Update(GameTime gameTime)
        {

        
            // TASK 1: Determine velocity based on accelerometer reading
       //     velocity.X += gameTime.ElapsedGameTime.Milliseconds*(float)game.accelerometerReading.AccelerationX*accelerometerScaling;
       //     velocity.Y+=gravity*gameTime.ElapsedGameTime.Milliseconds;
            //collision
            Player player=null;

            pathingMapHandler.resetGrid();


            foreach (GameObject i in game.gameObjects)
            {
                if (i is Player) { player = (Player)i; }
                if (!(i is Block)) { continue; }
                pathingMapHandler.updateBlocking((Block)i);
            
            }
            Project.PathFinding.Path<PathFinding.ArrayNodeController.PathingNode> path = pathingMapHandler.getPath(new Vector2(pos[0], pos[1]), new Vector2(player.pos[0], player.pos[1]));
            if (path != null) { 
            var firstPos = path.First();
            Vector2 movementPosition = firstPos.getRealPos();
            Vector2 movementDirection = Vector2.Normalize(movementPosition - new Vector2(pos[0], pos[1]));
            velocity += movementDirection * speedFactor * gameTime.ElapsedGameTime.Milliseconds;

            Random rng = new Random();
            if (movementDirection.Equals(Vector2.Zero)) { velocity += new Vector2(rng.NextFloat(-1,1),rng.NextFloat(-1,1)); }
        }
            foreach (GameObject i in game.gameObjects)
                {

                    if (!(i is Block)) { continue; }

                var tempVelocity=BallBlockCollisionHandler.adjustedVelocity(new Vector2(pos.X,pos.Y),velocity,radius,new Vector2(i.pos.X,i.pos.Y),new Vector2(0,0),1,1);
                velocity = tempVelocity;

            }
            pos.X += velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            pos.Y += velocity.Y * gameTime.ElapsedGameTime.Milliseconds;
            pos.Y += gameTime.ElapsedGameTime.Milliseconds * EnemyController.blockMovementFactor * game.difficulty;
            // Keep within the boundaries.
           // if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            //if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }

            basicEffect.World = Matrix.Translation(pos);

        }

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.Exit();


        }

        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {

            lastJumpTime = getCurrentTimeValue();
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            pos.X += (float)args.Delta.Translation.X / 100;
        }
        public long getCurrentTimeValue() 
            {
                return (long)(DateTime.Now - new DateTime(1970, 1, 1, 1, 1, 1)).TotalMilliseconds;
        }
    }
}