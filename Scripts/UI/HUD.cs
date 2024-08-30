using Core;
using Core.Components;
using Core.UI;
using Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI
{
    public  class HUD : IMenu
    {
        public Image moneyAndDayUI { get; set; }

        private Text moneyText { get; set; }
        private Text dateText { get; set; }
        public HUD(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            moneyAndDayUI = new Image(new Sprite(new Vector2(96,3),new Vector2(30,17),new Vector2(40,0),"Art/UI/UI",layer:.1f));
            moneyText = new Text(moneyAndDayUI.position+new Vector2(7,10f), "012345", Helper.HexToRgb("#edeff7"),Game1.Font_HUD, layer: 0.00009f);
            dateText = new Text(moneyAndDayUI.position + new Vector2(1), "", Helper.HexToRgb("#edeff7"),Game1.Font_24,layer: 0.00009f);
            LoadContent(Game1.contentManager);

            AddTextEleemnt(moneyText);
            AddMenuElement(moneyAndDayUI);
            AddTextEleemnt(dateText);
        }


        public override void LoadContent(ContentManager contentManager)
        {
            moneyAndDayUI.LoadContent(contentManager);

        }

        public override HUD SetActive(bool active)
        {
            isActive = active;
            moneyAndDayUI.isActive = active;
            moneyText.isActive = active;
            dateText.isActive = active;
            return this;
        }

        public override void Update(GameTime gameTime)
        {
            dateText.text = DayNightSystem.GetCurrentDate().ToString("EUDHR",CultureInfo.InvariantCulture);
            moneyText.text = Game1.player.money.ToString("D6");
        }

    }
}
