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
    public enum Origin
    {
        BottomCenter
    }
    public class Sprite : IComponent, IPosition, ILayerable
    {
        public string name { get; set; }
        [JsonIgnore]
        public Rectangle spriteRect { get; set; }

        public Rectangle tilemapLocationRect { get; }
        public Vector2 position { get; set; }
   

        public Vector2 origin { get; private set; } = Vector2.Zero;

        public Vector2 size { get; }

        [JsonIgnore]
        private Texture2D texture;
        public Color color { get; set; } = Color.White;

        public string texturePath { get; private set; }
        private float transparancy = 1f;
        public float rotation { get; set; }
        [JsonIgnore]
        public float scale { get;  set; } = 1f;
        public float layer{get;set; }
        public Vector2 tilemapPosition { get; set; }

        public SpriteEffects spriteEffects { get; set; }


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
        [JsonConstructor]
        public Sprite(Vector2 position,Rectangle tilemapLocationRect,string texturePath,string name,float layer)
        {
            this.name = name;
            this.position = position;
            this.tilemapLocationRect = tilemapLocationRect;
            this.texturePath = texturePath;
            this.layer = layer;
            this.size = new Vector2(this.tilemapLocationRect.Width,this.tilemapLocationRect.Height);
        }
        public Sprite(Sprite sprite)
        {
            name = sprite.name;
            this.position = sprite.position;
            this.size = sprite.size;
            this.texturePath = sprite.texturePath;
            this.layer = sprite.layer;
            this.spriteRect = new Rectangle();
            this.tilemapLocationRect = sprite.tilemapLocationRect;

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
        public Sprite SetTransparency(float transparency)
        {
            this.transparancy = transparency;
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

        public Sprite SetOrigin(Origin origin)
        {
            switch (origin)
            {
                case Origin.BottomCenter:
                    this.origin = new Vector2((int)(size.X / 2), size.Y);
                    break;
                default:
                    break;
            }
            return this;
        }
        public Vector2 getPosition()
        {
            return position - origin;
        }
        public Sprite setPosition(Vector2 position)
        {
            this.position = position-origin;
            return this;
        }
        public  void LoadContent(ContentManager contentManager)
        {
            texture =contentManager.Load<Texture2D>(texturePath);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Vector2 subPixelOffset = position - new Vector2((int)position.X, (int)position.Y);
            spriteBatch.Draw(texture,position, tilemapLocationRect, color*transparancy, rotation, origin, scale,spriteEffects, layer);

        }

        public  void Update(GameTime gameTime)
        {
            spriteRect = new Rectangle((int)(position.X-origin.X), (int)(position.Y-origin.Y), (int)size.X, (int)size.Y);
        }
    }
}
