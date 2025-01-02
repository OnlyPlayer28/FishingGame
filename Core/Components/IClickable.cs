using Core.Components;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    public abstract class ICLickable : IActive,ILayerable,IHoverable,IComponent
    {
        public  abstract Vector2 position { get; set; }
        public abstract Vector2 size { get; set; }

        public  abstract bool isActive { get; set; }
        public  abstract float layer { get; set; }
        public ICLickable(Vector2 position, Vector2 size, bool isActive)
        {
            this.position = position;
            this.size = size;
            this.isActive = isActive;
        }
        public virtual IActive SetActive(bool active)
        {
            this.isActive = active;
            return this;
        }
        public abstract void OnMouseClick();


        public abstract void SetPositionAndBoundingBox();

        public abstract void OnMouseOver(object sender, EventArgs e);
        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
