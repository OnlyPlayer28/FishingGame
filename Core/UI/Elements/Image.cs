using Core.Components;
using Core.Debug;
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
            get { return drawDebugShape ? debugShape.position : image.position; } 
            set { image.position = value; }
        }
        public Vector2 size 
        {
            get { return drawDebugShape?debugShape.size: image.size; }
            set { _size = value; }
        }
        public bool isActive { get; set; }
        public string name { get; set; }
        public float layer 
        { 
            get { return drawDebugShape?debugShape.layer: image.layer; }
            set { image.layer = value; }
        }
        public bool ignoreMouseInput { get; set; }
        public Sprite image { get;set; }
        public DebugShape debugShape { get; set; }
        private bool drawDebugShape { get; set; }
        public Image(Sprite image, bool ignoreMouseInput = false,bool isActive = true)
        {
            this.image = image;
            this.isActive = isActive;
            this.ignoreMouseInput = ignoreMouseInput;
        }
        public Image(DebugShape image,bool ignoreMouseInput = false,bool isActive = true)
        {
            this.ignoreMouseInput=ignoreMouseInput;
            this.isActive=isActive;
            debugShape = image;
            drawDebugShape = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                if(!drawDebugShape)
                    image.Draw(spriteBatch);
                else
                    debugShape.Draw(spriteBatch);
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            if(drawDebugShape)
                debugShape.LoadContent(contentManager);
            else
                image.LoadContent(contentManager);
        }

        public virtual void OnMouseOver(object sender, EventArgs e)
        {
        }

        public void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (!drawDebugShape)
                    image.Update(gameTime);
                else
                    debugShape.Update(gameTime);
            }
        }

        public void OnMouseOver(object sender, MouseInputEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
