/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class Block : GameObject
    {
            public float blockWidth=1;
            public float blockHeight=1;
        public Block(LabGame game, int x, int y)
        {


            this.game = game;
            type = GameObjectType.Block;
            myModel = game.assets.GetModel("block", CreateBlockModel);
            myModel = game.assets.GetModel("block", CreateBlockModel);
            this.pos = new Vector3(x,y,0);
           // pos = new SharpDX.Vector3(x, 0, 0);
            GetParamsFromModel();
            /*
            basicEffect = new BasicEffect(game.GraphicsDevice);

            basicEffect.World = Matrix.Identity;
            basicEffect.View = game.camera.View;
            basicEffect.Projection = game.camera.Projection;

            // primitive color
            basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
            basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.Alpha = 1.0f;
            *//*
        }

        //***Need to insert the correct arguments****
        public MyModel CreateBlockModel()
        {
            return game.assets.CreateTexturedCube("brick.png", 1);
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            basicEffect.World = Matrix.Translation(pos);
        }

    }
}*/
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class Block : GameObject
    {
        public static float blockWidth = 3.5f;
        public static float blockHeight = 0.5f;
        public static float blockDepth = 1.0f;

        public Block(LabGame game, float x, float y)
        {
            this.game = game;
            type = GameObjectType.Block;
            myModel = game.assets.GetModel("block", CreateBlockModel);
            this.pos = new Vector3(x, y, 0);
            // pos = new SharpDX.Vector3(x, 0, 0);
            GetParamsFromModel();
            /*
            basicEffect = new BasicEffect(game.GraphicsDevice);

            basicEffect.World = Matrix.Identity;
            basicEffect.View = game.camera.View;
            basicEffect.Projection = game.camera.Projection;

            // primitive color
            basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
            basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.Alpha = 1.0f;
            */
        //}
/*
        //***Need to insert the correct arguments****
        public MyModel CreateBlockModel()
        {
            return game.assets.CreateTexturedCube("brick.png", new Vector3(blockWidth, blockHeight, blockDepth));
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            basicEffect.View = game.camera.View;
            basicEffect.World = Matrix.Translation(pos);
        }

    }
}*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class Block : GameObject
    {
        public static float blockWidth = 3.5f;
        public static float blockHeight = 0.5f;
        public static float blockDepth = 1.0f;
        public String texturePath;


        public Block(LabGame game, float x, float y)
        {
            this.game = game;
            type = GameObjectType.Block;
            if (game.score < 20)
            {
                texturePath = "brick.png";
                myModel = game.assets.GetModel("block", CreateBlockModel);
            }
            else if (game.score < 800)
            {
                texturePath = "brick2.jpg";
                myModel = game.assets.GetModel("block2", CreateBlockModel);
            }
            else
            {
                texturePath = "brick3.png";
                myModel = game.assets.GetModel("block3", CreateBlockModel);
            }
            this.pos = new Vector3(x, y, 0);
            // pos = new SharpDX.Vector3(x, 0, 0);
            GetParamsFromModel();
            /*
            basicEffect = new BasicEffect(game.GraphicsDevice);

            basicEffect.World = Matrix.Identity;
            basicEffect.View = game.camera.View;
            basicEffect.Projection = game.camera.Projection;

            // primitive color
            basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
            basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.Alpha = 1.0f;
            */
        }

        //***Need to insert the correct arguments****
        public MyModel CreateBlockModel()
        {
            return game.assets.CreateTexturedCube(texturePath, new Vector3(blockWidth, blockHeight, blockDepth));
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            basicEffect.View = game.camera.View;
            basicEffect.World = Matrix.Translation(pos);
        }

    }
}