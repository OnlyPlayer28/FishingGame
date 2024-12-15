using Core;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI.PauseScreenMenus
{
    internal class RestaurantSettingsMenu : IMenu
    {
        private Text restaurantOpeningHoursText { get; set; }
        public RestaurantSettingsMenu(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            restaurantOpeningHoursText = new Text(position + new Vector2(1, 1), "openingHours",Color.Wheat, Game1.Font_24, layer:layer);

            canvas.AddTextElement(restaurantOpeningHoursText);
        }
        public override IMenu SetActive(bool active)
        {
            restaurantOpeningHoursText.isActive = active;
            return base.SetActive(active);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            restaurantOpeningHoursText.text = $"opening hours: {Game1.player.restaurantManager.openingHour}:{Game1.player.restaurantManager.openingMinute} - {Game1.player.restaurantManager.closingHour}:{Game1.player.restaurantManager.closingMinute} ";
            base.Update(gameTime);
        }
    }
}
