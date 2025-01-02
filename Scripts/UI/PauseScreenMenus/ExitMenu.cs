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
    public class ExitMenu : IMenu
    {
        private Button exitButton;
        public ExitMenu(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            exitButton = new Button((position+(size/2))-new Vector2(10,10), new Vector2(20, 10), layer, "exitButton", false, "click").SetSimpleSprite(Helper.HexToRgb("#542424"),Helper.HexToRgb("#843630"));
            exitButton.SetButtonText("Exit", Color.White, Game1.Font_24);

            canvas.AddClickableElement(exitButton);
            exitButton.onButtonClickAction = () =>  Game1.exitGameEvent?.Invoke(this, EventArgs.Empty); 

           
        }

        public override IActive SetActive(bool active)
        {
            exitButton.isActive = active;
            return base.SetActive(active);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
