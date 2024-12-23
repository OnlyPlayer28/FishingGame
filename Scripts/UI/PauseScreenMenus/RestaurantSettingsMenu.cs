using Core;
using Core.Components;
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
    internal class RestaurantSettingsMenu : IMenu
    {
        private Text restaurantOpeningHoursText { get; set; }

        private ArrowButtons arrowButtons { get; set; }
        private ArrowButtons closingArrows { get; set; }
        public RestaurantSettingsMenu(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            restaurantOpeningHoursText = new Text(position + new Vector2(1, 1), "openingHours",Color.Wheat, Game1.Font_24, layer:layer);

            canvas.AddTextElement(restaurantOpeningHoursText);

            arrowButtons = new ArrowButtons(position + new Vector2(27, 2), 12, new Sprite(Vector2.Zero,new Rectangle(7,37,4,7),"Art/UI/UI","",layer), true,canvas, "click","openingArrows", Helper.HexToRgb("#a06f51")).SetValue(Game1.player.restaurantManager.openingHour).SetMinAndMaxValues(0,23);
            closingArrows = new ArrowButtons(position + new Vector2(56, 2), 12, new Sprite(Vector2.Zero, new Rectangle(7, 37, 4, 7), "Art/UI/UI", "", layer), true, canvas, "click", "closingArrows", Helper.HexToRgb("#a06f51")).SetValue(Game1.player.restaurantManager.closingHour).SetMinAndMaxValues(0, 23);
            canvas.AddUIELement(arrowButtons);
            canvas.AddUIELement(closingArrows);

            arrowButtons.OnValueChangedEvent += OnOpeningHoursChanged;
            closingArrows.OnValueChangedEvent += OnOpeningHoursChanged;
        }
        public override IMenu SetActive(bool active)
        {
            restaurantOpeningHoursText.isActive = active;
            arrowButtons.isActive = active;
            closingArrows.isActive = active;
            return base.SetActive(active);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        private void OnOpeningHoursChanged(object o, ArrowButtonsEventArgs e)
        {
            if (e.name == "openingArrows")
            {
                Game1.player.restaurantManager.SetOpeningHours((int)e.value, 0);
            }else
            {
                Game1.player.restaurantManager.SetClosingHours((int)e.value, 0);
            }
        }
        public override void Update(GameTime gameTime)
        {
            restaurantOpeningHoursText.text = $"open from:    {Game1.player.restaurantManager.openingHour.ToString().PadRight(3)}    to:{Game1.player.restaurantManager.closingHour.ToString().PadLeft(6)}";
            base.Update(gameTime);
        }
    }
}
