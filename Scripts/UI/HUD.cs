using Core;
using Core.Components;
using Core.UI;
using Core.UI.Elements;
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
    public class HUD : IMenu
    {
        public Image moneyAndDayUI { get; set; }
        public Button goToRestaurantButton { get; set; }

        private Text moneyText { get; set; }
        public HUD(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            moneyAndDayUI = new Image(new Sprite(new Vector2(96,3),new Vector2(30,17),new Vector2(40,0),"Art/UI/UI",layer:.1f));
            moneyText = new Text(moneyAndDayUI.position+new Vector2(7,10f), "012345", Helper.HexToRgb("#edeff7"),Game1.Font_HUD, layer: 0.00009f);
            goToRestaurantButton = new Button(new Vector2(116, 116), new Vector2(10, 10),.1f, isActive: true)
                .SetButtonSprite(new Sprite(new Vector2(116,116),new Vector2(10,10),new Vector2(18,19),"Art/UI/UI",layer:.1f),Game1.contentManager)
                .SetOnButtonCLickAction(OnRestaurantButtonClick);
            LoadContent(Game1.contentManager);

            AddTextEleemnt(moneyText);
            AddMenuElement(moneyAndDayUI);
            canvas.AddClickableElement(goToRestaurantButton);
        }

        public void OnRestaurantButtonClick()
        {
            Game1.stateManager.SetActive(true, "restaurantScene");
           // Game1.player.money++;
        }
        public override void LoadContent(ContentManager contentManager)
        {
            moneyAndDayUI.LoadContent(contentManager);
            goToRestaurantButton.LoadContent(contentManager);
        }

        public override HUD SetActive(bool active)
        {
            isActive = active;
            moneyAndDayUI.isActive = active;
            moneyText.isActive = active;
            goToRestaurantButton.isActive = active;
            return this;
        }

        public override void Update(GameTime gameTime)
        {
            moneyText.text = Game1.player.money.ToString("D6");
        }

    }
}
