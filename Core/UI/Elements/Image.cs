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

namespace Core.UI.Elements
{
    public class Image : IUIElement
    {
        private Vector2 _size;
        public Vector2 position 
        {
            get { return image.position; } 
            set { image.position = value; }
        }
        public Vector2 size 
        {
            get { return image.size; }
            set { _size = value; }
        }
        public bool isActive { get; set; }
        public string name { get; set; }
        public float layer 
        { 
            get { return image.layer; }
            set { image.layer = value; }
        }

        public Sprite image { get;set; }

        public Image(Sprite image,bool isActive = true)
        {
            this.image = image;
            this.isActive = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(isActive)
                image.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager contentManager)
        {
            image.LoadContent(contentManager);
        }

        public virtual void OnMouseOver(object sender, EventArgs e)
        {
        }

        public void Update(GameTime gameTime)
        {
            if(isActive)
                image.Update(gameTime);
        }

        public void OnMouseOver(object sender, MouseInputEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
