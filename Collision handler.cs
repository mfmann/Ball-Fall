using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    //position of the ball relative to the block when it is colliding

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    
    //if the vector is (0,0) there is no collision
    class BallBlockCollisionHandler
    {
        static float reboundPercent=0.3f;
        public static Vector2 adjustedVelocity(Vector2 ballCollisionDirection,Vector2 ballVelocity,Vector2 blockVelocity) 
            {

                if (ballCollisionDirection.Equals(Vector2.Zero)) 
                    {
 

                    return ballVelocity;
                }// if direction is (0,0), ball not in contact with block
              
                if (Vector2.Dot(ballVelocity,blockVelocity)>0)//does dot product of 2 vectors moving in opposite directions give a negative number? assuming yes
                {

                    return ballVelocity;}//if it is positive, ball moving away from platform
                return Rejection(ballVelocity - blockVelocity, ballCollisionDirection) + -reboundPercent * Projection(ballVelocity - blockVelocity, ballCollisionDirection);
        
        }

        public static Vector2 adjustedVelocity(Vector2 ballPosition, Vector2 ballVelocity, float ballRadius, Vector2 blockPosition, Vector2 blockVelocity, float blockWidth, float blockHeight)
        {
            return adjustedVelocity(collidingDirection( ballPosition, ballRadius,  blockPosition, blockWidth, blockHeight),ballVelocity,blockVelocity);
        }


       public static void printVector2(Vector2 velocity,String message)
            {
      //          System.Diagnostics.Debug.WriteLine(message+": X="+velocity.X+" ,Y="+velocity.Y);
        }
        public static Vector2 collidingDirection(Vector2 ballPosition,float ballRadius, Vector2 blockPosition,float blockWidth,float blockHeight) 
            {
                Vector2 relativeBPos =ballPosition - blockPosition;

                

            if (Math.Abs(
                    relativeBPos.X)<blockWidth/2) 
                    {                    
                    if (Math.Abs(relativeBPos.Y)>blockHeight/2+ballRadius){return Vector2.Zero;}
                    else
                    if (relativeBPos.Y>0)
                        {return new Vector2(0,1);
                    }
                    else{return new Vector2(0,-1);}
                }
                if (Math.Abs(
                    relativeBPos.Y)<blockHeight/2) 
                    {
                    if (Math.Abs(relativeBPos.X)>blockWidth/2+ballRadius){return Vector2.Zero;}
                    else
                    if (relativeBPos.X>0)
                        {return new Vector2(1,0);
                    }
                    else{return new Vector2(-1,0);}
                    }


            Vector2[] corners={new Vector2(blockWidth/2,blockHeight/2),new Vector2(blockWidth/2,blockHeight/2),new Vector2(blockWidth/2,blockHeight/2),new Vector2(blockWidth/2,blockHeight/2)};
            for (int i=0;i<corners.Length;i++)
                {
                //position of the ball relative to the corners
                corners[i]=relativeBPos-corners[i];
            
            }

            //get the closest corner
            Vector2 closestCorner=Vector2.Min(Vector2.Min(Vector2.Min(corners[0],corners[1]),corners[2]),corners[3]);
            //check if actuallly in contact will corner
            if (closestCorner.Length()>ballRadius){

                return new Vector2(0,0);}
            else {

                Vector2 returnVector;
                Vector2.Normalize(ref closestCorner,out returnVector);
                return returnVector;}
        }
    




static Vector2 Projection(Vector2 a,Vector2 b)
    {
    return (Vector2.Dot(a,b)/Vector2.Dot(b,b))*b;
}
static Vector2 Rejection(Vector2 a,Vector2 b)
    {
        return a - Projection(a, b);
}
        
    }
}
