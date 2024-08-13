using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Components
{
    public class Sprite : IComponent, IPosition,ILayerable
    {
        public string name { get; set; }
        [JsonIgnore]
        public Rectangle spriteRect { get; set; }

        public Rectangle tilemapLocationRect { get; }
        public Vector2 position { get; set; }

        public Vector2 origin { get;private set; } = Vector2.Zero;
        public Vector2 size { get;  }

        [JsonIgnore]
        private Texture2D texture;
        public Color color { get; private set; } = Color.White;

        public string texturePath { get; private set; }
        private float transparancy=1f;
        public float rotation { get;  set ; }
        private float scale=1f;
        public float layer{get;set; }
        public Vector2 tilemapPosition { get; set; }

        [JsonConstructor]
        public Sprite(  Vector2 position, Vector2 size,Vector2 tilemapPosition,string texturePath,string name= "",float layer = 0f)
        {
            this.name = name;
            this.position = position;
            this.size = size;
            this.texturePath = texturePath;
            this.layer = layer;

            this.spriteRect = new Rectangle();
            tilemapLocationRect = new Rectangle((int)tilemapPosition.X, (int)tilemapPosition.Y, (int)this.size.X, (int)this.size.Y);
        }
        public Sprite(Vector2 position,Rectangle tilemapLocationRect,string texturePath,string name,float layer)
        {
            this.name = name;
            this.position = position;
            this.tilemapLocationRect = tilemapLocationRect;
            this.texturePath = texturePath;
            this.layer = layer;
            size = new Vector2(tilemapLocationRect.Width,tilemapLocationRect.Height);
        }
        /// <summary>
        /// Use only for testing/debugging purposes! Dont draw nor update this sprite otherwise the game's gonna crash!
        /// </summary>
        public Sprite()
        {
            
        }
        public Sprite SetColor(Color color)
        {
            this.color = color;
            return this;
        }
        public Sprite SetOriginToCenter()
        {
            origin = size / 2;
            return this;
        }


        public Sprite SetOrigin(Vector2 origin)
        {
            this.origin = origin;
            return this;
        }
        public Vector2 getPosition()
        {
            return position - origin;
        }
        public void setPosition(Vector2 position)
        {
            this.position = position-origin;
        }
        public  void LoadContent(ContentManager contentManager)
        {
            texture =contentManager.Load<Texture2D>(texturePath);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 subPixelOffset = position - new Vector2((int)position.X, (int)position.Y);
            spriteBatch.Draw(texture,position+origin, tilemapLocationRect, color, rotation, origin, scale, SpriteEffects.None, layer);

        }

        public  void Update(GameTime gameTime)
        {
            spriteRect = new Rectangle((int)(position.X-origin.X), (int)(position.Y-origin.Y), (int)size.X, (int)size.Y);
        }
    }
}
