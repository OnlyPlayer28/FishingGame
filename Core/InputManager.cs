using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

    public enum MouseButton
    {
        Left = 0, 
        Right = 1
    }
    public static class InputManager
    {
        private static MouseState previousMouseState;
        public static MouseState currentMouseState;

        private static KeyboardState previousKeyobardState;
        public static KeyboardState currentKeyobardState;

        public static void Update(GameTime gameTime)
        {
            previousMouseState = Mouse.GetState();
            previousKeyobardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentKeyobardState = Keyboard.GetState();
        }

        public static bool IsMouseButtonPressed(MouseButton mouseButton)
        {
            switch ((int)mouseButton)
            {
                case 0:
                    if(Mouse.GetState().LeftButton == ButtonState.Pressed&&previousMouseState.LeftButton == ButtonState.Released) 
                    { return true; }
                    break;
                case 1:
                    if (Mouse.GetState().RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
                    { return true; }
                    break;
                default:
                    return false;
            }
            return false;
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(Mouse.GetState().X,Mouse.GetState().Y);
        }
       /* public static Rectangle GetMouseRectWorld()
        {
            Matrix inverseMatrix = Matrix.Invert(Game1.WorldMatrix);
            Vector2 position =  Vector2.Transform(new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y),inverseMatrix);
            return new Rectangle((int)position.X, (int)position.Y, 1, 1);
        }
        public static Rectangle GetMouseRectUI()
        {
            return new Rectangle(
                (int)(Mouse.GetState().Position.X / Game1.UIScaleFactor), (int)(Mouse.GetState().Position.Y / Game1.UIScaleFactor), 1, 1);
        }*/

        public static bool AreKeysBeingPressedDown(bool allHaveToBePressed = false, params Keys[] keys)
        {
            
            foreach (Keys item in keys)
            {
                if (Keyboard.GetState().IsKeyDown(item) && previousKeyobardState.IsKeyUp(item))
                {
                    if(allHaveToBePressed)
                    {
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AreKeysBeingHeldDown(Keys[] keys)
        {

            foreach (Keys item in keys)
            {
                if (currentKeyobardState.IsKeyDown(item) )
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
