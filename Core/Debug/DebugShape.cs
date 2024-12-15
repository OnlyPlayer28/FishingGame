using Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Debug
{

    public class Line : DebugShape
    {

        public Line(Vector2 position, Vector2 size,  Color color, float layer = 0f) 
            : base(position, size,  color,layer)
        {

        }

        public Line(Vector2 startPosition, Vector2 endPosition, int width, Color color, float layer = 0f)
            : base(startPosition, new Vector2( width, (endPosition - startPosition).Length()), color, layer)
        {
            rotation =-MathHelper.ToDegrees((float)Math.Atan2((double)(endPosition-startPosition).Y,(double)(endPosition-startPosition).X));
            origin = new Vector2(1,0);
        }


    }


    public class Rect : DebugShape
    {
        private bool fillCenter { get; set; }

        public Line[] lines = new Line[9];

        public Rect(Vector2 position, Vector2 size, Color color,bool fillCenter = false,float layer =0f) 
            : base(position, size, color,layer)
        {
            this.fillCenter = fillCenter;

            SetSize(size);
        }

        public Rect SetFillColor(Color color)
        {
            lines[4].originalColor = color;
            return this;
        }
        public Rect SetSize(Vector2 size)
        {
            lines[0] = new Line(Vector2.Zero, new Vector2(1, 1), originalColor, layer);
            lines[1] = new Line(Vector2.Zero + new Vector2(1, 0), new Vector2(size.X - 2, 1), originalColor, layer);
            lines[2] = new Line(Vector2.Zero + new Vector2(size.X - 1, 0), new Vector2(1, 1), originalColor, layer);
            lines[3] = new Line(Vector2.Zero + new Vector2(0, 1), new Vector2(1, size.Y - 2), originalColor, layer);
            lines[4] = new Line(Vector2.Zero + new Vector2(1, 1), this.fillCenter ? new Vector2(size.X - 2, size.Y - 2) : Vector2.Zero, originalColor, layer);
            lines[5] = new Line(Vector2.Zero + new Vector2(size.X - 1, 1), new Vector2(1, size.Y - 2), originalColor, layer);
            lines[6] = new Line(Vector2.Zero + new Vector2(0, size.Y - 1), new Vector2(1, 1), originalColor, layer);
            lines[7] = new Line(Vector2.Zero + new Vector2(1, size.Y - 1), new Vector2(size.X - 2, 1), originalColor, layer);
            lines[8] = new Line(Vector2.Zero + new Vector2(size.X - 1, size.Y - 1), new Vector2(1, 1), originalColor, layer);
            return this;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in lines)
            {
                item.offset = position;
                item.Draw(spriteBatch);
            }
        }
        public void SetOverlayColor(Color color)
        {
            if (lines[0].color == color) { return; }
            foreach (var item in lines)
            {
                item.color = color;
            }
            this.color = color;
        }

    }
    public class DebugShape : IComponent
    {
        public Vector2 position { get; set; }
        public Vector2 size { get; set; }
        public Texture2D texture { get; set; }
        public Color color { get; set; } = Color.White;
        public Color originalColor { get; set; }

        public float rotation { get; set; }

        public float layer { get; set; } = 0f;
        public Vector2 offset { get; set; }
        public Vector2 origin { get; internal set; }
        public DebugShape(Vector2 position, Vector2 size, Color color, float layer = 0f)
        {
            this.position = position;
            this.texture = LineTool.pointTexture;
            this.originalColor = color;
            this.size = size;
            this.layer = layer;
        }

        public Rectangle GetCollision()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position+offset, new Rectangle(0,0,texture.Width,texture.Height), Helper.MultiplyColors(originalColor,color), rotation, origin, size, SpriteEffects.None, layer);
        }

        public void LoadContent(ContentManager contentManager)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
