﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Components
{
    public  class Sprite :IComponent,IPosition
    {
        public string name { get; set; }
        public Rectangle spriteRect { get; set; }
        private Rectangle tilemapLocationRect;
        public Vector2 position { get; set; }
        public Vector2 size { get;  }
        private Texture2D texture;
        public Color color { get; private set; } = Color.White;

        private string texturePath { get; }
        private float transparancy=1f;
        public float rotation { get;  set ; }
        private float scale=1f;
        public float layer = 1f;

        private Vector2 origin { get; set; } = Vector2.Zero;

        [JsonConstructor]
        public Sprite(  Vector2 position, Vector2 size,Vector2 tilemapPosition,string texturePath,string name= "")
        {
            this.name = name;
            this.position = position;
            this.size = size;
            this.texturePath = texturePath;

            this.spriteRect = new Rectangle();
            tilemapLocationRect = new Rectangle((int)tilemapPosition.X, (int)tilemapPosition.Y, (int)this.size.X, (int)this.size.Y);
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
        public Vector2 getPosition()
        {
            return position + origin;
        }
        public  void LoadContent(ContentManager contentManager)
        {
            texture =contentManager.Load<Texture2D>(texturePath);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 subPixelOffset = position - new Vector2((int)position.X, (int)position.Y);
            spriteBatch.Draw(texture, position/*+subPixelOffset*/, tilemapLocationRect, color, rotation, origin, scale, SpriteEffects.None, layer);

        }

        public  void Update(GameTime gameTime)
        {
            spriteRect = new Rectangle((int)(position.X-origin.X), (int)(position.Y-origin.Y), (int)size.X, (int)size.Y);
        }
    }
}
