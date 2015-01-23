// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.Devices.Sensors;

namespace Project
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class LabGame : Game
    {
        private BasicEffect basicEffect;
        private GraphicsDeviceManager graphicsDeviceManager;
        public List<GameObject> gameObjects;
        private Stack<GameObject> addedGameObjects;
        private Stack<GameObject> removedGameObjects;
        private KeyboardManager keyboardManager;
        public KeyboardState keyboardState;
        private Player player;
        public AccelerometerReading accelerometerReading; 
        public GameInput input;
        public double score;
        public MainPage mainPage;
        private Texture2D ballsTexture;
        private DateTime starttime;
        private Boolean hasSpawnedChaser = false,setStartTime=false;
        private Boolean hasSpawnedWorld = false;
       // private Texture2D background;
        //SpriteBatch spriteBatch;
        private static  double scoreScalingFactor = 0.005;

        // TASK 4: Use this to represent difficulty
        public float difficulty;

        // Represents the camera's position and orientation
        public Camera camera;

        // Graphics assets
        public Assets assets;

        // Random number generator
        public Random random;

        // World boundaries that indicate where the edge of the screen is for the camera.
        public float boundaryLeft;
        public float boundaryRight;
        public float boundaryTop;
        public float boundaryBottom;

        public bool started = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="LabGame" /> class.
        /// </summary>
        public LabGame(MainPage mainPage)
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
           
          

            // Create the keyboard manager
            keyboardManager = new KeyboardManager(this);
            assets = new Assets(this);
            random = new Random();
            input = new GameInput();

            // Set boundaries.
            boundaryLeft = -6.5f;
            boundaryRight = 6.5f;
            boundaryTop = 4;
            boundaryBottom = -4.5f;

            // Initialise event handling.
            input.gestureRecognizer.Tapped += Tapped;
            input.gestureRecognizer.ManipulationStarted += OnManipulationStarted;
            input.gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;
            input.gestureRecognizer.ManipulationCompleted += OnManipulationCompleted;

            this.mainPage = mainPage;

            score = 0;
            difficulty = 1;
        }

        protected override void LoadContent()
        {
            // Initialise game object containers.
            gameObjects = new List<GameObject>();
            addedGameObjects = new Stack<GameObject>();
            removedGameObjects = new Stack<GameObject>();

            // Create game objects.
            /*player = new Player(this);

            this.Add(player);
            this.Add(new ScrollingWall(this));
            this.Add(new BackWall(this, new Vector2(0, 0), 10000, 1));
            */
            // Create an input layout from the vertices
            //ballsTexture = Content.Load<Texture2D>("balls.dds");
           // background = Content.Load<Texture2D>("sky.jpg");

            

            //spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Ball Fall";
            camera = new Camera(this);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
        
            score+=this.difficulty*this.difficulty*scoreScalingFactor*gameTime.ElapsedGameTime.Milliseconds;
                            System.Diagnostics.Debug.WriteLine("update time start=" + DateTime.Now.Millisecond);
            if (started)
            {
                if (started && !setStartTime) { starttime = DateTime.Now; setStartTime = true; }
                if (!hasSpawnedWorld && (DateTime.Now - starttime).Milliseconds > 0)
                {
                    hasSpawnedWorld = true;
                    player = new Player(this);
                    this.Add(player);
                    this.Add(new ScrollingWall(this));
                    this.Add(new BackWall(this, new Vector2(0, 0), 30000, 1));

                }
                if (!hasSpawnedChaser  && (DateTime.Now - starttime).Seconds > 10)
                {
                    hasSpawnedChaser=true;
                    gameObjects.Add(new Chaser(this, new Vector3(0, this.boundaryBottom - 0.5f, 0)));
                }
                accelerometerReading = input.accelerometer.GetCurrentReading();
                System.Diagnostics.Debug.WriteLine("time 1=" + DateTime.Now.Millisecond);
                flushAddedAndRemovedGameObjects();
                System.Diagnostics.Debug.WriteLine("time 2=" + DateTime.Now.Millisecond);
                camera.Update(gameTime);
                System.Diagnostics.Debug.WriteLine("time 3=" + DateTime.Now.Millisecond);

                System.Diagnostics.Debug.WriteLine("time 4=" + DateTime.Now.Millisecond);
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Update(gameTime);
                }
                System.Diagnostics.Debug.WriteLine("time 5=" + DateTime.Now.Millisecond);
                mainPage.UpdateScore((int)score);

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                    this.Dispose();
                    App.Current.Exit();
                }
                // Handle base.Update
            }
            System.Diagnostics.Debug.WriteLine("time 6=" + DateTime.Now.Millisecond);
            base.Update(gameTime);
                       System.Diagnostics.Debug.WriteLine("update time end=" + DateTime.Now.Millisecond);
                       System.Diagnostics.Debug.WriteLine("time 7=" + DateTime.Now.Millisecond);
        }

        protected override void Draw(GameTime gameTime)
        {

            System.Diagnostics.Debug.WriteLine("draw time start=" + DateTime.Now.Millisecond);
            if (started)
            {
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(Color.Black);

               // spriteBatch.Begin();
               // spriteBatch.Draw(background, new Rectangle(0, 0, 1400, 850), Color.White);
               // spriteBatch.End();

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Draw(gameTime);
                }
            }
           
            // Handle base.Draw
            base.Draw(gameTime);
            System.Diagnostics.Debug.WriteLine("draw time end=" + DateTime.Now.Millisecond);
        }
        // Count the number of game objects for a certain type.
        public int Count(GameObjectType type)
        {
            int count = 0;
            foreach (var obj in gameObjects)
            {
                if (obj.type == type) { count++; }
            }
            return count;
        }

        // Add a new game object.
        public void Add(GameObject obj)
        {
            if (!gameObjects.Contains(obj) && !addedGameObjects.Contains(obj))
            {
                addedGameObjects.Push(obj);
            }
        }

        // Remove a game object.
        public void Remove(GameObject obj)
        {
            if (gameObjects.Contains(obj) && !removedGameObjects.Contains(obj))
            {
                removedGameObjects.Push(obj);
            }
        }

        // Process the buffers of game objects that need to be added/removed.
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedGameObjects.Count > 0) { gameObjects.Add(addedGameObjects.Pop()); }
            while (removedGameObjects.Count > 0) { gameObjects.Remove(removedGameObjects.Pop()); }
        }

        public void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            // Pass Manipulation events to the game objects.

        }

        public void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            // Pass Manipulation events to the game objects.
            foreach (var obj in gameObjects)
            {
                obj.Tapped(sender, args);
            }
        }

        public void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            camera.pos.Z = camera.pos.Z * args.Delta.Scale;
            // Update camera position for all game objects
            foreach (var obj in gameObjects)
            {
                if (obj.basicEffect != null) { obj.basicEffect.View = camera.View; }
                obj.OnManipulationUpdated(sender, args);
            }
        }

        public void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
        }

    }
}
