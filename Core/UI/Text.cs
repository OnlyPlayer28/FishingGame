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

namespace Core.UI
{
    internal class Text : IComponent, ITaggable
    {
        public string name { get; set; }

        public Color color { get; set; }
        public string text { get; set; }

        public Vector2 position { get; set; }

        private SpriteFont font { get; set; }
        private float layer { get; set; }


        public Text(Vector2 position,string text,Color color, SpriteFont font ,string name="defaultText",float layer = .1f)
        {
            this.position = position;   
            this.text = text;
            this.color = color;
            this.name = name;
            this.layer = layer;
            //this.font = font;
            LoadContent(Game1.contentManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color);
        }

        public void LoadContent(ContentManager contentManager)
        {
            font = contentManager.Load<SpriteFont>("Fonts/Font_Pixel");
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
