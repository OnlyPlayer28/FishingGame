using Core.UI;
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
    public abstract class IMenu : IUIElement
    {
        public Vector2 position { get; set ; }
        public Vector2 size { get; set; }
        public bool isActive { get; set; } = false; 
        public string name { get; set; }
        public float layer { get; set; }

        private Canvas canvas;

        private List<IUIElement> menuElements { get; set; }
        public bool ignoreMouseInput { get ; set; }

        public IMenu(Vector2 position, Vector2 size, float layer,Canvas canvas,string name = "defaultMenu")
        {
            menuElements = new List<IUIElement>();
            this.position = position;
            this.size = size;
            this.name = name;
            this.layer = layer;
            this.canvas = canvas;
        }
        public IMenu AddMenuElement(IUIElement uIElement)
        {
            menuElements.Add(uIElement);
            menuElements.Last().layer += this.layer;
            menuElements.Last().position += this.position;
            canvas.AddUIELement(menuElements.Last());
            return this;
        }
        public IMenu AddTextEleemnt(Text element)
        {
            canvas.AddTextElement(element);
            return this;
        }
        public IMenu RemoveTextElement(Text element)
        {
            canvas.textElements.Remove(element);
            return this;
        }
        public virtual IMenu SetActive(bool active)
        {
            this.isActive = active;
            return this;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                menuElements.ForEach(p => p.Draw(spriteBatch));
            }
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            menuElements.ForEach(p => p.LoadContent(contentManager));
        }

        public virtual void OnMouseOver(object sender, EventArgs e)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void OnMouseOver(object sender, MouseInputEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
