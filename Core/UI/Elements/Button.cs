using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Core.UI.Elements
{
    public class Button : IUIElement
    {
        public bool isActive { get ; set ; }
        public string name { get ; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public void LoadContent(ContentManager contentManager)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
