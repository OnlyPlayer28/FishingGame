using Core;
using Core.Components;
using Core.Debug;
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
    public class InventoryScreen : IMenu
    {

        private Rect background { get; set; }
        public List<Sprite> icons { get; set; }
        public InventoryScreen(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            background = new Rect(position , size, Helper.HexToRgb("#542424"), true, layer).SetFillColor(Helper.HexToRgb("#6e3b34"));
        }

        public override void LoadContent(ContentManager contentManager)
        {

            base.LoadContent(contentManager);
        }

        public override IMenu SetActive(bool active)
        {
            
            return base.SetActive(active);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                background.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }
    }
}
