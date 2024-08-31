using Core.Animations;
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
    public enum Origin
    {
        BottomCenter,
        Center,
        BottomLeft,
        BottomRight,
        CenterLeft,
        CenterRight,
        TopCenter,
        TopRight,
        TopLeft
    }
    public class Sprite : IComponent, IPosition, ILayerable,ICloneable
    {
        public string name { get; set; }
        [JsonIgnore]
        public Rectangle spriteRect { get; set; }

        public Rectangle tilemapLocationRect { get; set; }
        public Vector2 position { get; set; }


        public Vector2 origin { get; private set; } = Vector2.Zero;

        public Vector2 size { get; set;  }

        [JsonIgnore]
        public Texture2D texture { get; private set; }
        public Color color { get; set; } = Color.White;

        public string texturePath { get; private set; }
        private float transparancy = 1f;
        public float rotation { get; set; }
        [JsonIgnore]
        public float scale { get; set; } = 1f;
        public float layer { get; set; }
        public Vector2 tilemapPosition { get; set; }

        public SpriteEffects spriteEffects { get; set; }

        public List<Animation> animations { get;private set; } = new List<Animation> ();

        public Sprite(Vector2 position, Vector2 size, Vector2 tilemapPosition, string texturePath, string name = "", float layer = 0f)
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
        public Sprite(Vector2 position, Rectangle tilemapLocationRect, string texturePath, string name, float layer, params Animation[] animations)
        {
            this.name = name;
            this.position = position;
            this.tilemapLocationRect = tilemapLocationRect;
            this.texturePath = texturePath;
            this.layer = layer;
            this.size = new Vector2(this.tilemapLocationRect.Width, this.tilemapLocationRect.Height);
            if (animations != null)
                this.animations = animations.ToList();
            else
                this.animations = new List<Animation>();
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
        /// <summary>
        /// Defines a new texturePath and textureCoordinates for the sprite.Using "default" as path resets the sprite to default.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tilemapLocationRect"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Sprite SetNewPathAndLocationRect(string path, Rectangle tilemapLocationRect,ContentManager content)
        {
            if(path == "default")
            {
                this.tilemapLocationRect = new Rectangle(0, 0, 0, 0);
                return this;
            }

            this.tilemapLocationRect = tilemapLocationRect;
            if(texturePath!= path)
            {
                this.texturePath = path;
                LoadContent(content);
            }

            return this;
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
                case Origin.Center:
                    this.origin = new Vector2((int)(size.X / 2), (int)(size.Y / 2));
                    break;
                case Origin.BottomLeft:
                    this.origin = new Vector2(size.X, (int)(size.Y));
                    break;
                case Origin.BottomRight:
                    this.origin = new Vector2((int)(size.X ), (int)(size.Y));
                    break;
                case Origin.CenterLeft:
                    this.origin = new Vector2((int)(size.X), (int)(size.Y / 2));
                    break;
                case Origin.CenterRight:
                    this.origin = new Vector2((int)(size.X), (int)(size.Y / 2));
                    break;
                case Origin.TopCenter:
                    this.origin = new Vector2((int)(size.X / 2),0);
                    break;
                case Origin.TopRight:
                    this.origin = new Vector2((int)(size.X ), 0);
                    break;
                case Origin.TopLeft:
                    this.origin = new Vector2(0, 0);
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
        public Sprite PlayAnimation(string name,bool looping = true)
        {
            if(animations.Any(p=>p.name == name))
            {
                foreach (var item in animations)
                {
                    if(item.name == name)
                    {
                        item.Play().SetLooping(looping);
                    }
                    else
                    {
                        item.Stop();
                    }
                }
            }
            return this;
        }
        public void StopAllAnimation()
        {
            animations.ForEach(p => p.Stop());
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                if(animations.Any(p=>p.state == AnimationState.Playing))
                {
                    spriteBatch.Draw(texture, position,animations.Where(p=>p.state==AnimationState.Playing).First().GetCurrentFrameRect(), color * transparancy, rotation, origin, scale, spriteEffects, layer);
                }
                else
                {
                    spriteBatch.Draw(texture, position, tilemapLocationRect, color * transparancy, rotation, origin, scale, spriteEffects, layer);
                }
            }catch(Exception e)
            {

            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in animations)
            {
                if(item.state == AnimationState.Playing)
                {
                    item.Update(gameTime);
                }
            }
            spriteRect = new Rectangle((int)(position.X-origin.X), (int)(position.Y-origin.Y), (int)size.X, (int)size.Y);
        }

        public object Clone()
        {
            return new Sprite(this);
        }
    }
}
