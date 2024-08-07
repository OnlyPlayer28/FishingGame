using Core.Components;
using Core.UI;
using Fishing;
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
    internal class Canvas:ITaggable,IComponent,IActive
    {
        public List<IUIElement> elements { get; set; }
        public List<ICLickable> clickableUI { get; set; }
        public List<Text> textElements { get; set; }
        public string name { get; set; }
        public bool isActive { get; set ; }
        public bool isDrawing { get; set ; }

        public Canvas( string name, bool isActive)
        {
            this.elements = new List<IUIElement>();
            textElements = new List<Text>();
            this.name = name;
            this.isActive = isActive;
        }

        public void OnMouseClick(Object sender,MouseInputEventArgs e)
        {
            if (InputManager.inputState == GameInputState.Gameplay) { return; }
            foreach (var item in clickableUI)
            {
                if(e.mouseRect.Intersects(new Rectangle((int)item.position.X, (int)item.position.Y, (int)item.size.X, (int)item.size.Y)))
                {
                    item.OnMouseClick();
                    return;
                }
            }
            
        }
        public void LoadContent(ContentManager contentManager)
        {
            elements.ForEach(p=>p.LoadContent(Game1.contentManager));
            textElements.ForEach(p => p.LoadContent(Game1.contentManager));
        }
        public void AddTextElement(Text texToAdd)
        {
            textElements.Add(texToAdd);
        }
        public void AddUIELement(IUIElement uIElement)
        {
            elements.Add(uIElement);
            elements.OrderBy(p => p.layer);
        }
        public void AddClickableElement (ICLickable clickable)
        {
            clickableUI.Add(clickable);
            clickableUI.OrderBy(p => p.layer);
        }
        public void Update(GameTime gameTime)
        {
            if(isActive)
            {
                //Add checking for mouseHover so that gameInput state can be set to semi UI.Also IClickable should register only if the input state is semiUI or UI.
                //So that should be implemented
                foreach (var item in elements)
                {
                    item.Update(gameTime);
                }
            }
        }
        public void DrawText(SpriteBatch spriteBatch)
        {
            if (isActive || isDrawing)
            {
                textElements.ForEach(p => p.Draw(spriteBatch));
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive||isDrawing)
            {
                elements.ForEach(p => p.Draw(spriteBatch));
            }
        }
    }
}
