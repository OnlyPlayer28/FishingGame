using Core.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class KeyboardInputEventArgs : EventArgs
    {
        public GameInputState inputState;
        public Keys[] keys;
    }
    public class MouseInputEventArgs : EventArgs
    {
        public GameInputState inputState;
        public Rectangle mouseRect;
        public MouseButton mouseButton;
    }
    public enum GameInputState
    {
        Gameplay,
        UI,
        SemiUI
    }
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

        public static EventHandler<KeyboardInputEventArgs> OnKeyboardPressEvent;
        public static EventHandler<MouseInputEventArgs> OnMouseClickEvent;
        public static EventHandler<MouseInputEventArgs> OnMouseDownEvent;

        public static GameInputState inputState { get; set; }

        public static void Update(GameTime gameTime,bool isFocused)
        {
            if (isFocused) 
            {
                if (IsMouseButtonPressed(0) || IsMouseButtonPressed(MouseButton.Right))
                {
                    MouseButton button = IsMouseButtonPressed(0) ? MouseButton.Left : MouseButton.Right;

                    OnMouseClickEvent?.Invoke(null, new MouseInputEventArgs { inputState = inputState, mouseRect = new Rectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1, 1), mouseButton = button });
                }
                if (Keyboard.GetState().GetPressedKeyCount() > 0)
                {
                    OnKeyboardPressEvent?.Invoke(null, new KeyboardInputEventArgs { inputState = inputState, keys = Keyboard.GetState().GetPressedKeys() });
                }
            }
            previousMouseState = Mouse.GetState();
            previousKeyobardState = Keyboard.GetState();
        }

        /// <summary>
        /// Don't use for checking input!Subscribe to the events instead.
        /// </summary>
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

        public static Rectangle GetMouseRect()
        {

            return new Rectangle((int)(Mouse.GetState().Position.X/CameraManager.GetCurrentCamera().zoom), (int)(Mouse.GetState().Position.Y/ CameraManager.GetCurrentCamera().zoom), 1, 1);
        }
        /// <summary>
        /// Don't use for checking input!Subscribe to the events instead.
        /// </summary>
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
        /// <summary>
        /// Don't use for checking input!Subscribe to the events instead.
        /// </summary>
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
