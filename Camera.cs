/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Project
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public Game game;
        public Vector3 pos;
        public Vector3 oldPos;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(Game game) {
            pos = new Vector3(0, 0, -10);
            View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update(GameTime gameTime)
        {
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
        }
    }
}
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Project
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public LabGame game;
        public Vector3 pos;
        public Vector3 oldPos;
        float accelerometerScaling = 7.0f;
        private Vector3 focalPoint = new Vector3(0, 0, 0);
        private float distFromFP = 10.0f;
        private float MaxY = 2.0f;
        private float MinY = -2.0f;
        private float MaxX = 4.0f;
        private float MinX = -4.0f;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(LabGame game)
        {
            pos = new Vector3(0, 0, focalPoint.Z - distFromFP);
            View = Matrix.LookAtLH(pos, focalPoint, Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update(GameTime gameTime)
        {
            var a = game.accelerometerReading;
            var b = a.AccelerationX;
            pos.X = (-(float)game.accelerometerReading.AccelerationX * accelerometerScaling + pos.X) / 2;
            if (pos.X > MaxX) pos.X = MaxX;
            if (pos.X < MinX) pos.X = MinX;
            pos.Y = (-(float)game.accelerometerReading.AccelerationZ * accelerometerScaling + pos.Y) / 2;
            if (pos.Y > MaxY) pos.Y = MaxY;
            if (pos.Y < MinY) pos.Y = MinY;
            pos.Z = -1 * (float)Math.Sqrt(Math.Pow(distFromFP, 2) - Math.Pow(pos.X, 2) - Math.Pow(pos.Y, 2)) + focalPoint.Z;

            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(pos, focalPoint, Vector3.UnitY);
        }
    }
}
