using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
namespace Project
{
    /*code is a modified version of the code at this site http://blog.andreaskahler.com/2009/06/creating-icosphere-mesh-in-code.html*/
    public class IcoSphereCreator
    {
        private struct TriangleIndices
        {
            public int v1;
            public int v2;
            public int v3;

            public TriangleIndices(int v1, int v2, int v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
            }
        }

       // private MeshGeometry3D geometry;
        private List<int> triangleIndicesGeometry;
        private List<Vector3> positionsGeometry;
        private int index;
        private Dictionary<Int64, int> middlePointIndexCache;

        // add vertex to mesh, fix position to be on unit sphere, return index
        private int addVertex(Vector3 p)
        {
            float length = (float)Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
            positionsGeometry.Add(new Vector3(p.X / length, p.Y / length, p.Z / length));//adjusts for size, adds to list
            return index++;
        }

        // return index of point in the middle of p1 and p2
        private int getMiddlePoint(int p1, int p2)
        {
            // first check if we have it already
            bool firstIsSmaller = p1 < p2;
            Int64 smallerIndex = firstIsSmaller ? p1 : p2;
            Int64 greaterIndex = firstIsSmaller ? p2 : p1;
            Int64 key = (smallerIndex << 32) + greaterIndex;

            int ret;
            if (this.middlePointIndexCache.TryGetValue(key, out ret))
            {
                return ret;
            }

            // not in cache, calculate it
            Vector3 point1 = positionsGeometry[p1];
            Vector3 point2 = positionsGeometry[p2];
            Vector3 middle = new Vector3(
                (float)((point1.X + point2.X) / 2.0),
                (float)((point1.Y + point2.Y) / 2.0),
               (float) ((point1.Z + point2.Z) / 2.0));

            // add vertex makes sure point is on unit sphere
            int i = addVertex(middle);

            // store it, return index
            this.middlePointIndexCache.Add(key, i);
            return i;
        }

        public Vector3[] Create(int recursionLevel)
        {
            positionsGeometry = new List<Vector3>();
            triangleIndicesGeometry = new List<int>();
            
            this.middlePointIndexCache = new Dictionary<long, int>();
            this.index = 0;

            // create 12 vertices of a icosahedron
            float t = (float)((1.0 + (float)Math.Sqrt(5.0)) / 2.0);

            addVertex(new Vector3(-1, t, 0));
            addVertex(new Vector3(1, t, 0));
            addVertex(new Vector3(-1, -t, 0));
            addVertex(new Vector3(1, -t, 0));

            addVertex(new Vector3(0, -1, t));
            addVertex(new Vector3(0, 1, t));
            addVertex(new Vector3(0, -1, -t));
            addVertex(new Vector3(0, 1, -t));

            addVertex(new Vector3(t, 0, -1));
            addVertex(new Vector3(t, 0, 1));
            addVertex(new Vector3(-t, 0, -1));
            addVertex(new Vector3(-t, 0, 1));


            // create 20 triangles of the icosahedron
            var faces = new List<TriangleIndices>();

            // 5 faces around point 0
            faces.Add(new TriangleIndices(0, 11, 5));
            faces.Add(new TriangleIndices(0, 5, 1));
            faces.Add(new TriangleIndices(0, 1, 7));
            faces.Add(new TriangleIndices(0, 7, 10));
            faces.Add(new TriangleIndices(0, 10, 11));

            // 5 adjacent faces 
            faces.Add(new TriangleIndices(1, 5, 9));
            faces.Add(new TriangleIndices(5, 11, 4));
            faces.Add(new TriangleIndices(11, 10, 2));
            faces.Add(new TriangleIndices(10, 7, 6));
            faces.Add(new TriangleIndices(7, 1, 8));

            // 5 faces around point 3
            faces.Add(new TriangleIndices(3, 9, 4));
            faces.Add(new TriangleIndices(3, 4, 2));
            faces.Add(new TriangleIndices(3, 2, 6));
            faces.Add(new TriangleIndices(3, 6, 8));
            faces.Add(new TriangleIndices(3, 8, 9));

            // 5 adjacent faces 
            faces.Add(new TriangleIndices(4, 9, 5));
            faces.Add(new TriangleIndices(2, 4, 11));
            faces.Add(new TriangleIndices(6, 2, 10));
            faces.Add(new TriangleIndices(8, 6, 7));
            faces.Add(new TriangleIndices(9, 8, 1));


            // refine triangles
            for (int i = 0; i < recursionLevel; i++)
            {
                var faces2 = new List<TriangleIndices>();
                foreach (var tri in faces)
                {
                    // replace triangle by 4 triangles
                    int a = getMiddlePoint(tri.v1, tri.v2);
                    int b = getMiddlePoint(tri.v2, tri.v3);
                    int c = getMiddlePoint(tri.v3, tri.v1);

                    faces2.Add(new TriangleIndices(tri.v1, a, c));
                    faces2.Add(new TriangleIndices(tri.v2, b, a));
                    faces2.Add(new TriangleIndices(tri.v3, c, b));
                    faces2.Add(new TriangleIndices(a, b, c));
                }
                faces = faces2;
            }

            // done, now add triangles to mesh
            foreach (var tri in faces)
            {
                this.triangleIndicesGeometry.Add(tri.v1);
                this.triangleIndicesGeometry.Add(tri.v2);
                this.triangleIndicesGeometry.Add(tri.v3);
            }

            return triangleArrayGenerator(triangleIndicesGeometry,positionsGeometry);
        }

      static Vector3[] triangleArrayGenerator(List<int> triangleIndicesGeometry,List<Vector3> positionsGeometry)
            {Vector3[] triangles=new Vector3[triangleIndicesGeometry.Count];
            for (int i = 0; i < triangleIndicesGeometry.Count; i++)
            { triangles[i] = positionsGeometry[triangleIndicesGeometry[i]]; }

                return triangles;
            }
        static VertexPositionNormalColor[] getVertexPositionColorArray(Vector3[] triangles, Color ballColor)
            {
            
                       VertexPositionNormalColor[] vertexList = new VertexPositionNormalColor[triangles.Length];
            Vector3 normal=Vector3.Zero;    
            for (int i=0;i<triangles.Length;i++)
                    {
                        if (i % 3 == 0)
                        {var lineA = triangles[i + 1] - triangles[i];
                        var lineB = triangles[i + 2] - triangles[i+1];
                            normal=Vector3.Normalize(Vector3.Cross(lineA,lineB));
                        }
                    vertexList[i]=new VertexPositionNormalColor(triangles[i],normal,ballColor);
                }
            return vertexList;
            
    }
        public static VertexPositionNormalColor[] getBallModel(int subdivisions,Color ballColor)
            {
                IcoSphereCreator generator = new IcoSphereCreator();
                return getVertexPositionColorArray(generator.Create(subdivisions),ballColor);

            }
        
    }

}
