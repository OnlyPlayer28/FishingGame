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

    internal static class LineTool
    {
        public static Texture2D pointTexture { get; set; }
        private static Texture2D rectangleTexture { get; set; }

        private static List<DebugShape> shapes { get; set; }
        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            shapes = new List<DebugShape>();
            pointTexture = new Texture2D(graphicsDevice, 1, 1);
            rectangleTexture = new Texture2D(graphicsDevice, 3, 3);

            pointTexture.SetData(new Color[1] {Color.White});
            rectangleTexture.SetData(new Color[9] { Color.White, Color.White, Color.White, Color.White, Color.Transparent, Color.White, Color.White, Color.White, Color.White });
        }
            
            
        public static void AddLine(Vector2 position,Vector2 size,Color color)
        {
            shapes.Add(new Line(position,size, color));
        }
        public static void AddRectangle(Vector2 position, Vector2 size, Color color)
        {
            shapes.Add(new Rect(position, size,  color));
        }
        public static void Draw(SpriteBatch spriteBatch)
        {

            shapes.ForEach(p=>p.Draw(spriteBatch));
        }

        public static void LoadContent(ContentManager contentManager)
        {

        }

        public static void Update(GameTime gameTime)
        {

        }
    }
}
