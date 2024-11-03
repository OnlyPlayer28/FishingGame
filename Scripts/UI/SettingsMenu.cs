using Core;
using Core.Debug;
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
    internal class SettingsMenu : IMenu
    {
        private int currentPage = 0;
        private Rect background { get; set; }
        public SettingsMenu(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            background = new Rect(position, size, Helper.HexToRgb("#542424"), true, layer - .00001f).SetFillColor(Helper.HexToRgb("#6e3b34"));
        }

        public override IMenu SetActive(bool active)
        {

            return base.SetActive(active);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isActive)
            {
                background.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        private void OnChangeMenuButtonsClick(Object o,ButtonEventArgs e)
        {

        }
        private void OnMenuButtonsClick(Object o,ButtonEventArgs e)
        {

        } 
    }
}
