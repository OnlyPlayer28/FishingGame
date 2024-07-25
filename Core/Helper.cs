using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Core.Components;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooter.Core
{
    public static class Helper
    {
        /*public static Vector2 GetAnchorPosition(AnchorPoint anchorPoint)
        {
            Vector2 positon = Vector2.Zero;
            switch (anchorPoint)
            {
                case AnchorPoint.TopLeft:
                    positon = Vector2.Zero;
                    break;
                case AnchorPoint.Top:
                    positon = new Vector2(Game1.defaultResolution.X / 2, 0);
                    break;
                case AnchorPoint.TopRight:
                    positon = new Vector2(Game1.defaultResolution.X , 0);
                    break;
                case AnchorPoint.Right:
                    positon = new Vector2(0,Game1.defaultResolution.Y/2);
                    break;
                case AnchorPoint.Centered:
                    positon = new Vector2(Game1.defaultResolution.X/2, Game1.defaultResolution.Y / 2);
                    break;
                case AnchorPoint.Left:
                    positon = new Vector2(Game1.defaultResolution.X, Game1.defaultResolution.Y/2 );
                    break;
                case AnchorPoint.BottomLeft:
                    positon = new Vector2(0, Game1.defaultResolution.Y);
                    break;
                case AnchorPoint.Bottom:
                    positon = new Vector2(Game1.defaultResolution.X/2, Game1.defaultResolution.Y );
                    break;
                case AnchorPoint.BottomRight:
                    positon = new Vector2(Game1.defaultResolution.X, Game1.defaultResolution.Y );
                    break;
                default:
                    
                    break;
            }

            return positon/Game1.UIScaleFactor;
        }*/

       /* public static float RotateTowards(Vector2 positionToRotateTowards,Vector2 objectToRotate)
        {

           Vector2 direction = positionToRotateTowards - objectToRotate;
            return (float)Math.Atan2(direction.Y,direction.X);

        }

        public static Vector2 ScreenToWorldSpace(in Vector2 vectorToConvert)
        {
            Matrix inverseMatrix = Matrix.Invert(Game1.WorldMatrix);
            return Vector2.Transform(vectorToConvert, inverseMatrix);
        }
        public static Vector2 WorldToScreenSpace(in Vector2 vectorToConvert)
        {
            Matrix inverseMatrix = Matrix.Invert(Game1.WorldMatrix);
            Vector2 cameraSpacePosition = Vector2.Transform(vectorToConvert,inverseMatrix);
            Viewport viewport = new Viewport(0,0,(int)Game1.defaultResolution.X,(int)Game1.defaultResolution.Y);
            return new Vector2((cameraSpacePosition.X + 1) * viewport.Width / 2, (cameraSpacePosition.Y + 1) * viewport.Height / 2);
        }
        public static Vector2 RotateVector(Vector2 direction, float degrees)
        {
            // Convert degrees to radians
            float radians = MathHelper.ToRadians(degrees);

            // Calculate sine and cosine of the angle
            float cosTheta = (float)Math.Cos(radians);
            float sinTheta = (float)Math.Sin(radians);

            // Perform rotation
            float newX = direction.X * cosTheta - direction.Y * sinTheta;
            float newY = direction.X * sinTheta + direction.Y * cosTheta;

            // Return the rotated vector
            return new Vector2(newX, newY);
        }*/

        public static Vector2 MoveToward(Vector2 position, Vector2 positionToMoveTowards,float speed) 
        {
            Vector2 direction = positionToMoveTowards - position;
            direction.Normalize();
            return (float)(position - positionToMoveTowards).Length() <.2f? positionToMoveTowards:position+ (direction)*speed;
        }
    }
}
