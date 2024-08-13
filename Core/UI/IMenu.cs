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
        public bool isActive { get; set; }
        public string name { get; set; }
        public float layer { get; set; }

        private Canvas canvas;

        private List<IUIElement> menuElements { get; set; }

        public IMenu(Vector2 position, Vector2 size, float layer,string name = "defaultMenu")
        {
            menuElements = new List<IUIElement>();
            this.position = position;
            this.size = size;
            this.name = name;
            this.layer = layer;
        }
        public IMenu AddMenuElement(IUIElement uIElement)
        {
            menuElements.Add(uIElement);
            menuElements.Last().layer += this.layer;
            menuElements.Last().position += this.position;
            return this;
        }
        public IMenu AddTextEleemnt(Text element)
        {
            canvas.AddTextElement(element);
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
    }
}
