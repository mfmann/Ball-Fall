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
    public class Assets
    {
        LabGame game;
        //private GraphicsDeviceManager graphicsDeviceManager;
       

        public Assets(LabGame game)
        {
            this.game = game;
           
        }



        // Dictionary of currently loaded models.
        // New/existing models are loaded by calling GetModel(modelName, modelMaker).
        public Dictionary<String, MyModel> modelDict = new Dictionary<String, MyModel>();

        // Load a model from the model dictionary.
        // If the model name hasn't been loaded before then modelMaker will be called to generate the model.
        public delegate MyModel ModelMaker();
        public MyModel GetModel(String modelName, ModelMaker modelMaker)
        {
            if (!modelDict.ContainsKey(modelName))
            {
                modelDict[modelName] = modelMaker();
            }
            return modelDict[modelName];
        }

        // Create a cube with one texture for all faces.
        public MyModel CreateTexturedCube(String textureName, float size)
        {
            return CreateTexturedCube(textureName, new Vector3(size, size, size));
        }

        public MyModel CreateBall(Vector3 size,Color color) 
            {
                var vertices=Project.IcoSphereCreator.getBallModel(5,color);
                for (int i = 0; i < vertices.Length; i++)
                { vertices[i].Position.X = vertices[i].Position.X * size.X;
                vertices[i].Position.Y = vertices[i].Position.Y * size.Y;
                vertices[i].Position.Z = vertices[i].Position.Z * size.Z;
                }
                    return new MyModel(game, vertices, null, Math.Max(Math.Max(size.Y, size.Z), size.X));
        }

        public MyModel CreateTexturedWall(String texturePath, Vector2 size)
        {
            Vector3 frontNormal = new Vector3(0.0f, 0.0f, -1.0f);

            VertexPositionNormalTexture[] shapeArray = new VertexPositionNormalTexture[]{
                new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), frontNormal, new Vector2(0.0f, 1.0f)), // Front
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), frontNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), frontNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), frontNormal, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), frontNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), frontNormal, new Vector2(1.0f, 1.0f))
            };

            for (int i = 0; i < shapeArray.Length; i++)
            {
                shapeArray[i].Position.X *= size.X / 2;
                shapeArray[i].Position.Y *= size.Y / 2;
                shapeArray[i].Position.Z = 1.0f;
            }

            float collisionRadius = 0;
            return new MyModel(game, shapeArray, texturePath, collisionRadius);
        }
        public MyModel CreateTexturedCube(String texturePath, Vector3 size)
        {

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 backNormal = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            VertexPositionNormalTexture[] shapeArray = new VertexPositionNormalTexture[]{
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), frontNormal, new Vector2(0.0f, 1.0f)), // Front
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), frontNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), frontNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), frontNormal, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), frontNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), frontNormal, new Vector2(1.0f, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), backNormal, new Vector2(1.0f, 1.0f)), // BACK
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), backNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), backNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), backNormal, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), backNormal, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), backNormal, new Vector2(0.0f, 0.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), topNormal, new Vector2(0.0f, 1.0f)), // Top
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), topNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), topNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), topNormal, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), topNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), topNormal, new Vector2(1.0f, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), bottomNormal, new Vector2(0.0f, 0.0f)), // Bottom
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), bottomNormal, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), bottomNormal, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), bottomNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), bottomNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), bottomNormal, new Vector2(1.0f, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), leftNormal, new Vector2(1.0f, 1.0f)), // Left
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), leftNormal, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), leftNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), leftNormal, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), leftNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), leftNormal, new Vector2(1.0f, 0.0f)),

            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), rightNormal, new Vector2(0.0f, 1.0f)), // Right
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), rightNormal, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), rightNormal, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), rightNormal, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), rightNormal, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), rightNormal, new Vector2(1.0f, 0.0f)),
            };

            for (int i = 0; i < shapeArray.Length; i++)
            {
                shapeArray[i].Position.X *= size.X / 2;
                shapeArray[i].Position.Y *= size.Y / 2;
                shapeArray[i].Position.Z *= size.Z / 2;
            }

            float collisionRadius = (size.X + size.Y + size.Z) / 6;
            return new MyModel(game, shapeArray, texturePath, collisionRadius);
        }
    }
}
