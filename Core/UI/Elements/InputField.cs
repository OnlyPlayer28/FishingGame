using Core.Components;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI.Elements
{
    public enum InputFieldState
    {
        ClickedOff,
        IsWriting

    }
    public class InputField : ICLickable,IActive
    {
        private Text inputText;
        private InputFieldState state;
        private string defaultText ="write here";

        public override Vector2 position { get ; set ; }
        public override Vector2 size { get ; set ; }
        public override bool isActive { get ; set; }
        public override float layer { get; set; }
        public InputField(Vector2 position, Vector2 size,float layer, Canvas canvas, bool isActive,SpriteFont font)
            : base(position, size, isActive)
        {
            this.layer = layer;
            this.isActive = isActive;
            inputText = new Text(position, defaultText, Color.Red, font, layer: this.layer,isActive:this.isActive);
            canvas.AddTextElement(inputText);

            InputManager.OnMouseClickEvent += OnMouseInput;
            InputManager.OnKeyboardPressEvent += OnKeyboardInput;
        }
        private void OnKeyboardInput(Object o,KeyboardInputEventArgs e)
        {
            string textToAdd = "";
            bool isShift = false;
            
            if(isActive&&state ==InputFieldState.IsWriting&&e.inputState!=GameInputState.Gameplay)
            {
                if( e.keys.Contains(Keys.LeftShift)||e.keys.Contains(Keys.RightShift)) { isShift = true; }
                else { isShift = false; }

                foreach (Keys item in e.keys)
                {
                    switch (item)
                    {
                        case Keys.LeftShift:
                        case Keys.RightShift:
                            break;
                        case Keys.D1:textToAdd = "1";break;
                        case Keys.D2:textToAdd = "2";break;
                        case Keys.D3:textToAdd = "3";break;
                        case Keys.D4:textToAdd = "4";break;
                        case Keys.D5:textToAdd = "5";break;
                        case Keys.D6:textToAdd = "6";break;
                        case Keys.D7:textToAdd = "7";break;
                        case Keys.D8:textToAdd = "8";break;
                        case Keys.D9:textToAdd = "9";break;
                        case Keys.D0:textToAdd = "0";break;
                        default:
                            if (item.ToString().Contains("Oem")) 
                            {
                                textToAdd= item.ToString().Replace("Oem", "").ToLower();
                            }
                            else if(item.ToString().Length == 1)
                            {
                                System.Diagnostics.Debug.WriteLine("added 1 char, isShift: "+isShift);
                                textToAdd = isShift?item.ToString():item.ToString().ToLower();
                            }
 
                            break;
                    }

                }
                inputText.text += textToAdd;
            }
        }
        private void OnMouseInput(Object o,MouseInputEventArgs e)
        {
            /*if (isActive&&!e.mouseRect.Intersects(Helper.GetRectFromVector2(position, size)))
            {
                state = InputFieldState.ClickedOff;
                if(inputText.text.Length == 0)
                {
                    inputText.text =defaultText;
                }
            }*/
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
        public override IActive SetActive(bool active)
        {
            this.isActive = active;
            inputText.SetActive(active);
            return this;
        }
        public override void LoadContent(ContentManager contentManager)
        {
        }

        public override void OnMouseClick()
        {
            state = InputFieldState.IsWriting;
            if(inputText.text == defaultText)
            {
                inputText.text = "";
            }
        }

        public override void OnMouseOver(object sender, EventArgs e)
        {
        }

        public override void SetPositionAndBoundingBox()
        {
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
