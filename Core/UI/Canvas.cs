using Core.Components;
using Core.UI;
using Fishing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        }
        public void Update(GameTime gameTime)
        {
            if(isActive)
            {
                elements.ForEach(p=>p.Update(gameTime));
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
