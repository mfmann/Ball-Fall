using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Project
{
    public class PathFinding
    {


        public class ArrayNodeController
        {
            int xSize, ySize;
            float xSpan, ySpan,xShift,yShift;
            ArrayNode[,] grid;

            public ArrayNodeController(int xSize,int ySize,float xSpan,float ySpan,float xShift,float yShift) 
                {this.xSize=xSize;
                this.ySize = ySize;
                this.xSpan = xSpan;
                this.ySpan = ySpan;
                this.xShift = xShift;
                this.yShift = yShift;

                grid=ArrayNode.generateNodeGrid(xSize,ySize,xSpan,ySpan,xShift,yShift);
                
            }
            public void updateBlocking(Block block) {
                for (int i = (int)(block.pos[0] - Block.blockWidth / 2); i <= (block.pos[0] + Block.blockWidth); i++)
                    for (int j = (int)(block.pos[1] - Block.blockHeight / 2); i <= (block.pos[1] + Block.blockHeight); i++)
                    { if (withinArrayBounds(i, j)) { grid[i,j].blockIfIntersecting(block); } }
            }
            Func<PathingNode, double> getDistanceEstimator(PathingNode endpoint)
            {
                Func<PathingNode, PathingNode, double> fn = distanceFinder;
                return startpos=>distanceFinder(startpos,endpoint); }

            double distanceFinder(PathingNode a, PathingNode b)
            { return Vector2.Distance(a.getRealPos(), b.getRealPos()); }

        public Path<PathingNode> getPath(Vector2 startPos,Vector2 endPos)
            {
                PathingNode startNode = insertNodeIntoArrayNodeGrid(startPos[0],startPos[1]);
                PathingNode endNode = insertNodeIntoArrayNodeGrid(endPos[0], endPos[1]);
            return FindPath(startNode,endNode,distanceFinder,getDistanceEstimator(endNode)); }
            public Boolean withinArrayBounds(float xPos,float yPos)
            {
                return ((xPos > xShift && xPos < xShift + xSpan) && (yPos > yShift && yPos < yShift + ySpan));
            }
            public Vector2 getArrayPos(float xPos,float yPos) 
                {
                xPos-=xShift;
                yPos -= yShift;
                xPos = xPos / xSpan / ((float)xSize / (xSize - 1)) * xSize;
                yPos = yPos/ySpan / ((float)ySize / (ySize - 1))*ySize;
                return new Vector2(xPos, yPos);
            
            }
            public Boolean withinArrayBounds(int xPos, int yPos)
            {
                return ((xPos >= 0 && xPos < xSize) && (yPos >=0 && yPos < ySize));
            }


            public void resetGrid() { foreach (ArrayNode i in grid) { i.resetNode(); } }


            public PathingNode insertNodeIntoArrayNodeGrid(float xPos, float yPos)
                {
                    PathingNode insertNode = new FreeNode(xPos, yPos);
                    Vector2 arrayPos=getArrayPos(xPos,yPos);
                List<int>xPosValues=new List<int>();
                List<int>yPosValues=new List<int>();

                if (arrayPos[0] <= 0) { xPosValues.Add(0); }
                else
                    if
                        (arrayPos[0] >= xSize - 1) { xPosValues.Add(xSize - 1); }
                    else 
                    {xPosValues.Add((int)arrayPos[0]);
                    xPosValues.Add((int)arrayPos[0]+1);
                    }
                if (arrayPos[1] <= 0) { yPosValues.Add(0); }
                else
                    if
                        (arrayPos[1] >= ySize - 1) { yPosValues.Add(ySize - 1); }
                    else
                    {
                        yPosValues.Add((int)arrayPos[1]);
                        yPosValues.Add((int)arrayPos[1] + 1);
                    }
                foreach (int i in xPosValues) foreach (int j in yPosValues)
                    {
                        PathingNode neighbour = grid[i, j];
                    neighbour.addNode(insertNode);
                    insertNode.addNode(neighbour);
                    }
                   


                    return insertNode;
            }

            public class FreeNode : PathingNode
            {
                float xPos, yPos;
                public Vector2 getRealPos() { return new Vector2(xPos, yPos); }
                

                public  Boolean isBlocked(){return true;}
                public FreeNode(float xPos,float yPos) 
                    {
                        this.xPos = xPos;
                        this.yPos = yPos;
                }
                private List<PathingNode> neighbours = new List<PathingNode>();
                public void addNode(PathingNode node) { neighbours.Add(node); }

                public void removeNode(PathingNode node) { neighbours.Add(node); }

                //public override IEnumerable<PathingNode> getNeighbours(){return neighbours;}
                public  IEnumerable<PathingNode> Neighbours { get { return neighbours; } }
                public IEnumerator<PathingNode> GetEnumerator()
                    {
                    return neighbours.GetEnumerator();
                }


                


            }

            public class ArrayNode : PathingNode
            {
                public  Boolean isBlocked() { return isBlockedVar; }
                List<PathingNode> neighbours = new List<PathingNode>();
                List<PathingNode> tempNeighbours = new List<PathingNode>();//neighbours that
                ArrayNode[,] nodeGrid;
                float x, y;
                int gridX, gridY;
                public Boolean isBlockedVar = false;
                public IEnumerable<PathingNode> Neighbours { get { return neighbours; } }
                public Vector2 getRealPos() { return new Vector2(x,y); }
                
                public ArrayNode(float x, float y, int gridX, int gridY, ArrayNode[,] nodeGrid)
                {
                    this.x = x;
                    this.y = y;
                    this.nodeGrid = nodeGrid;
                    this.gridX = gridX;
                    this.gridY = gridY;
                }


                public void initNeighbourList() {  
                for (int i = gridX - 1; i < gridX + 1; i++) {
                    for (int j = gridY - 1; j < gridY + 1; j++) { if (i != gridX || j != gridY)
                    { if (i >= 0 && i < nodeGrid.GetLength(0) && j >= 0 && j < nodeGrid.GetLength(1))
                    {
                        neighbours.Add(nodeGrid[i,j]);
                    } }
                    }
                
             } }

                public Boolean checkForIntersection(Block block) 
                    {
                        float xMin = block.pos[0] - Block.blockWidth / 2;
                        float xMax = block.pos[0] + Block.blockWidth / 2;
                        float yMin = block.pos[1] - Block.blockHeight / 2;
                        float yMax = block.pos[1] + Block.blockHeight / 2;
                    if ((x>xMin&&x<xMax)&&(y>yMin&&y<yMax)){return true;}
                    else {return false;}
                }
                public void blockIfIntersecting(Block block)
                { if (checkForIntersection(block)) { isBlockedVar = true; } }


                public void resetNode(){isBlockedVar=false;
                removeTempNeighbours();}

                public void addNode(PathingNode neighbour)
                {
                    tempNeighbours.Add(neighbour);
                neighbours.Add(neighbour);
                }

                public void removeTempNeighbours()
                {
                    if (tempNeighbours.Count > 0) { foreach (PathingNode i in tempNeighbours) { neighbours.Remove(i); } }
                }


                //size is the size of the grid dimension.  Span is the area the nodes cover
                public static ArrayNode[,] generateNodeGrid(int xSize,int ySize,float xSpan,float ySpan,float xShift,float yShift)
                    {
                        ArrayNode[,] returnArray = new ArrayNode[xSize, ySize];
                        for (int i = 0; i < xSize; i++)
                        { for (int j = 0; j < ySize; j++) 
                            {
                                float xPos = (i *xSpan) /xSize*((float)xSize/(xSize-1)) / xSize+xShift;
                                float yPos = (j * ySpan) / ySize * ((float)ySize / (ySize - 1)) / ySize+yShift;
                            returnArray[i,j]=new ArrayNode(xPos,yPos,i,j,returnArray);
                        
                        } }

                        foreach (ArrayNode a in returnArray)    
                            {a.initNeighbourList();}
                        return returnArray;
                }



                public IEnumerator<PathingNode> GetEnumerator()
                {
                    List<PathingNode> returnList = new List<PathingNode>();
                    foreach (PathingNode i in neighbours)
                    { 
                        if (!i.isBlocked())returnList.Add(i); }
                    
                    return returnList.GetEnumerator(); }

            }
            public interface PathingNode : IHasNeighbours<PathingNode>
            {

                Boolean isBlocked();
                void addNode(PathingNode neighbour);

                Vector2 getRealPos();
                
            };
             
            



        }








        //*********************************************************************************************************
        /*a* algorithm and related code taken from:
         * http://blogs.msdn.com/b/ericlippert/archive/2007/10/10/path-finding-using-a-in-c-3-0-part-four.aspx
         * http://blogs.msdn.com/b/ericlippert/archive/2007/10/04/path-finding-using-a-in-c-3-0-part-two.aspx
         * http://blogs.msdn.com/b/ericlippert/archive/2007/10/02/path-finding-using-a-in-c-3-0.aspx
         * http://blogs.msdn.com/b/ericlippert/archive/2007/10/08/path-finding-using-a-in-c-3-0-part-three.aspx
         * 
        */

        /*


        public class Path<Node> : IEnumerable<Node>
        {
            public Node LastStep { get; private set; }
            public Path<Node> PreviousSteps { get; private set; }
            public double TotalCost { get; private set; }
            private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
            {
                LastStep = lastStep;
                PreviousSteps = previousSteps;
                TotalCost = totalCost;
            }
            public Path(Node start) : this(start, null, 0) { }
            public Path<Node> AddStep(Node step, double stepCost)
            {
                return new Path<Node>(step, this, TotalCost + stepCost);
            }
            public IEnumerator<Node> GetEnumerator()
            {
                for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                    yield return p.LastStep;
            }
            IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }


        */
        public class Path<Node> : IEnumerable<Node>
        {
            public Node LastStep { get; private set; }
            public Path<Node> PreviousSteps { get; private set; }
            public double TotalCost { get; private set; }
            private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
            {
                LastStep = lastStep;
                PreviousSteps = previousSteps;
                TotalCost = totalCost;
            }
            public Path(Node start) : this(start, null, 0) { }
            public Path<Node> AddStep(Node step, double stepCost)
            {
                return new Path<Node>(step, this, TotalCost + stepCost);
            }

            /**public class PathEnumerator : IEnumerator<Node> 
                {

                Path<Node> currentP,startingP;

                public Node Current { get { return currentP.LastStep; } }
                public PathEnumerator(Path<Node> p) {
                    this.currentP = p;
                    this.startingP = p;
                }
                public void Reset() { }
                public boolean MoveNext() { }
                
            }*/
             System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            { return this.GetEnumerator(); }
            public IEnumerator<Node> GetEnumerator()
            {
                
                for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                    yield return p.LastStep;
            }
            IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }



















        public interface IHasNeighbours<N>
        {
            IEnumerable<N> Neighbours { get; }
        }
        static public Path<Node> FindPath<Node>(
    Node start,
    Node destination,
    Func<Node, Node, double> distance,
    Func<Node, double> estimate)
    where Node : IHasNeighbours<Node>
        {
            var closed = new HashSet<Node>();
            var queue = new PriorityQueue<double, Path<Node>>();
            queue.Enqueue(0, new Path<Node>(start));
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep))
                    continue;
                if (path.LastStep.Equals(destination))
                    return path;
                closed.Add(path.LastStep);
                foreach (Node n in path.LastStep.Neighbours)
                {
                    double d = distance(path.LastStep, n);
                    var newPath = path.AddStep(n, d);
                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }
            }
            return null;
        }




        public class PriorityQueue<P, V>
        {
            private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();
            public void Enqueue(P priority, V value)
            {
                Queue<V> q;
                if (!list.TryGetValue(priority, out q))
                {
                    q = new Queue<V>();
                    list.Add(priority, q);
                }
                q.Enqueue(value);
            }
            public V Dequeue()
            {
                // will throw if there isn’t any first element!
                var pair = list.First();
                var v = pair.Value.Dequeue();
                if (pair.Value.Count == 0) // nothing left of the top priority.
                    list.Remove(pair.Key);
                return v;
            }
            public bool IsEmpty
            {
                get { return !list.Any(); }
            }
        }







    }

}