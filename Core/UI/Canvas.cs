﻿using Core.Audio;
using Core.Components;
using Core.UI;
using Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI
{
    public class Canvas:INameable,IComponent,IActive
    {
        public List<IUIElement> elements { get; set; }
        public List<ICLickable> clickableUI { get; set; }
        public List<Text> textElements { get; set; }
        public string name { get; set; }
        public bool isActive { get; set ; }
        public bool isDrawing { get; set ; }

        public Canvas( string name, bool isActive)
        {
            InputManager.OnMouseClickEvent += OnMouseClick;
            this.elements = new List<IUIElement>();
            textElements = new List<Text>();
            clickableUI = new List<ICLickable>();
            this.name = name;
            this.isActive = isActive;
        }
        public IActive SetActive(bool active)
        {
            this.isActive = active;
            return this;
        }
        public void OnMouseClick(Object sender,MouseInputEventArgs e)
        {

            if (InputManager.inputState == GameInputState.Gameplay||!isActive) {  return; }
            foreach (var item in clickableUI)
            {
                if(item.isActive&&InputManager.GetMouseRect().Intersects(Helper.GetRectFromVector2(item.position,item.size)))
                {
                    if(elements.Any(p=>p.isActive&& !p.ignoreMouseInput&&p!=item&&p.layer < item.layer && Helper.GetRectFromVector2(p.position, p.size).Intersects(InputManager.GetMouseRect())))
                    {
                        break;
                    }
                    item.OnMouseClick();
                    //InputManager.inputState = GameInputState.UI;
                    break;
                }
            }

        }

        public void LoadContent(ContentManager contentManager)
        {
            elements.ForEach(p=>p.LoadContent(contentManager));
            clickableUI.ForEach(p => p.LoadContent(contentManager));
            textElements.ForEach(p => p.LoadContent(contentManager));
        }
        public void AddTextElement(Text texToAdd)
        {
            textElements.Add(texToAdd);
        }
        public void AddUIELement(IUIElement uIElement)
        {
            elements.Add(uIElement);
            elements = elements.OrderBy(p => p.layer).ToList();
        }
        public void AddClickableElement (ICLickable clickable)
        {
            clickableUI.Add(clickable);
           clickableUI =  clickableUI.OrderBy(p => p.layer).ToList();
        }


        public void Update(GameTime gameTime)
        {
            if(isActive)
            {
                //Add checking for mouseHover so that gameInput state can be set to semi UI.Also IClickable should register only if the input state is semiUI or UI.
                //So that should be implemented
                foreach (var item in elements)
                {
                    if (item.isActive)
                    {
                        item.Update(gameTime);
                    }
                }
                foreach (var item in clickableUI)
                {
                    if (item.isActive)
                    {
                        item.Update(gameTime);
                    }

                    if (item.isActive &&InputManager.GetMouseRect().Intersects(Helper.GetRectFromVector2(item.position, item.size)))
                    {
                        if (clickableUI.Any(p => p!= item&&p.isActive && p.layer < item.layer && Helper.GetRectFromVector2(p.position, p.size).Intersects(InputManager.GetMouseRect())))
                        {

                            break;
                        }
                        else
                        {
                            
                            item.OnMouseOver(this, EventArgs.Empty);
                        }
                            break;
                    }
                }

                if(InputManager.inputState != GameInputState.UI&&(elements.Any(p=>p.isActive&&InputManager.GetMouseRect().Intersects(Helper.GetRectFromVector2(p.position,p.size)))|| clickableUI.Any(p =>p.isActive&&InputManager.GetMouseRect().Intersects(Helper.GetRectFromVector2(p.position, p.size)))))
                {
                    InputManager.inputState = GameInputState.SemiUI;
                }
                else
                {
                    InputManager.inputState = GameInputState.Gameplay;
                }

            }
        }
        public void DrawText(SpriteBatch spriteBatch)
        {
            if (isActive || isDrawing)
            {
                foreach (var item in textElements)
                {
                    if (item.isActive)
                    {
                        item.Draw(spriteBatch);
                    }
                }
                foreach (var item in clickableUI)
                {
                    if(item.isActive&&item is Button button)
                    {
                        button.DrawText(spriteBatch);
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive||isDrawing)
            {
                foreach (var item in elements)
                {
                    if (item.isActive) { item.Draw(spriteBatch); }
                }
                foreach (var item in clickableUI)
                {
                    if (item.isActive) { item.Draw(spriteBatch); }
                }
            }
        }
    }
}
