using Core;
using Core.Components;
using Core.UI;
using Fishing.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI.PauseScreenMenus
{
    internal class SettingsMenu : IMenu
    {
        private Text controls { get; set; }
        public SettingsMenu(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu")
            : base(position, size, layer, canvas, name)
        {
            controls = new Text(position, "                     -Controls-\n", Color.Wheat, Game1.Font_24, layer: layer);
            foreach (var item in ControlsManager.inputKeys)
            {
                controls.text += $"{item.Key.Replace('_',' ').ToLower()}: {item.Value.ToString()}\n";
            }
            controls.text += "                      -Audio-";
            canvas.AddTextElement(controls);
        }

        public override IActive SetActive(bool active)
        {
            controls.isActive = active;
            return base.SetActive(active);
        }
    }
}
