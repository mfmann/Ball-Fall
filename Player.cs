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
    // Player class.
    class Player : GameObject
    {
        //private float speed = 0.006f;
        private float projectileSpeed = 20;
        Vector2 velocity = new Vector2(0,0);
        float gravity = -0.0000009f;//-0.0001f;
        float accelerometerScaling = 0.00007f;
        public float radius = 0.5f;
        float accelerationScaling = 0.2f / 1000f;
        float jumpScaling = 0.00012f;
        const float minTheta = (float)Math.PI / 4;
            long lastJumpTime=0;
            long maxLastJumpTime = 100;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player", CreatePlayerModel);
            pos = new SharpDX.Vector3(0, game.boundaryTop - 0.5f, 0);
            GetParamsFromModel();
        }

        public MyModel CreatePlayerModel()
        {
            return game.assets.CreateBall(new Vector3(radius,radius,radius),Color.Orange);

        }


        // Frame update.
        public override void Update(GameTime gameTime)
        {
            pos.Y += gameTime.ElapsedGameTime.Milliseconds * EnemyController.blockMovementFactor * game.difficulty;
            Boolean inContactWithTop = false;


            // TASK 1: Determine velocity based on accelerometer reading
            float accelerometerVelocity = (float)game.accelerometerReading.AccelerationX * accelerometerScaling;
            float adjustmentVelocity = accelerationScaling * Math.Sign(accelerometerVelocity - velocity.X);
            velocity.X += gameTime.ElapsedGameTime.Milliseconds*(float)game.accelerometerReading.AccelerationX*accelerometerScaling;
            velocity.Y+=gravity*gameTime.ElapsedGameTime.Milliseconds;
            //collision

            foreach (GameObject i in game.gameObjects)
                {

                    if (i is Chaser)
                    {
                        var dist=(Vector2.Distance(new Vector2(i.pos.X,i.pos.Y),new Vector2(pos.X,pos.Y)));
  
                        var totalRadiusLength=radius+((Chaser)i).radius;
                        if (dist<totalRadiusLength)
                        {
                            LoseGame();
                        }
                    }
                    if (!(i is Block)) { continue; }
                
                var tempVelocity=BallBlockCollisionHandler.adjustedVelocity(new Vector2(pos.X,pos.Y),velocity,radius,new Vector2(i.pos.X,i.pos.Y),new Vector2(0,0),Block.blockWidth,Block.blockHeight);
                if ((tempVelocity!=velocity)&&(Vector2.Dot(Vector2.Normalize(tempVelocity - velocity), new Vector2(0, 1)) >= Math.Sin(minTheta)))
                    {
                        inContactWithTop = true;
                }
                velocity = tempVelocity;

            }


            if (inContactWithTop && lastJumpTime > getCurrentTimeValue() - maxLastJumpTime - gameTime.ElapsedGameTime.TotalMilliseconds)
            { lastJumpTime = 0;
            velocity += new Vector2(0, gameTime.ElapsedGameTime.Milliseconds * jumpScaling);
            }

            pos.X += velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            pos.Y += velocity.Y * gameTime.ElapsedGameTime.Milliseconds;

            // Keep within the boundaries.
           // if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            //if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }

            basicEffect.World = Matrix.Translation(pos);
            if (pos.X < game.boundaryLeft && velocity.X < 0) { velocity.X *= -0.5f; }
            if (pos.X > game.boundaryRight && velocity.X > 0) { velocity.X *= -0.5f; }
            if (pos.Y > game.boundaryTop) { LoseGame(); }

            if (pos.Y < game.boundaryBottom) { LoseGame(); }
        }

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.Exit();


        }
        public void LoseGame()
        { game.mainPage.Restart(); }

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