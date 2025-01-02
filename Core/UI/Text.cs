using Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fishing;
using Core.Cameras;

namespace Core.UI
{
    public class Text : IComponent, INameable,IActive
    {
        public string name { get; set; }

        public Color color { get; set; }
        public string text { get; set; }

        public Vector2 position { get; private set; }

        private SpriteFont font { get; set; }
        private float layer { get; set; }
        public bool isActive { get ; set ; }

        public float scale { get; set; } = 1f;

        public Text(Vector2 position,string text,Color color, SpriteFont font ,string name="defaultText",float layer = .1f,bool isActive = false)
        {
            this.position = position*CameraManager.GetCurrentCamera().zoom;   
            this.text = text;
            this.color = color;
            this.name = name;
            this.layer = layer;
            this.font = font;
            this.isActive = isActive;
            LoadContent(Game1.contentManager);
        }
        public IActive SetActive(bool active)
        {
            this.isActive = active;
            return this;
        }
        public Text setPosition(Vector2 position)
        {
            this.position = position*CameraManager.GetCurrentCamera().zoom;
            return this;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color,0,Vector2.Zero,scale,SpriteEffects.None,layer);
        }

        public void LoadContent(ContentManager contentManager)
        {
           //font = contentManager.Load<SpriteFont>("Fonts/Tuna_24");
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
