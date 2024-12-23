using Core;
using Core.Debug;
using Core.UI;
using Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI.PauseScreenMenus
{
    internal class RestaurantMenuSettings : IMenu
    {
        private Image[] menuItemAreaOutlines { get; set; }
        public RestaurantMenuSettings(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            menuItemAreaOutlines = new Image[6];
            for (int i = 0; i < menuItemAreaOutlines.Length; i++)
            {
                menuItemAreaOutlines[i] = new Image(new Rect(position+new Vector2(0,1) + new Vector2(1, i * 16), new Vector2(size.X-2, 16), Helper.GetRandomColor(), false, layer ),true,false);
                canvas.AddUIELement(menuItemAreaOutlines[i]);
            }
        }

        public override IMenu SetActive(bool active)
        {
            for (int i = 0;i < menuItemAreaOutlines.Length;i++)
            {
                menuItemAreaOutlines[i].isActive = active;
            }
            return base.SetActive(active);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menuItemAreaOutlines.Length; i++)
            {
                menuItemAreaOutlines[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
