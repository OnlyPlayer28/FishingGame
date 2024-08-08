using Core;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI
{
    public class FishingResultsScreen : IUIElement
    {
        public Vector2 position { get; set ; }
        public Vector2 size { get ; set ; }
        public bool isActive { get; set; }
        public string name { get; set; }
        public float layer { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public void LoadContent(ContentManager contentManager)
        {
        }

        public void OnMouseOver(object sender, EventArgs  e)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
