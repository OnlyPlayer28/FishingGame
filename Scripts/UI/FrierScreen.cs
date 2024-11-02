using Core.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI
{
    internal class FrierScreen : IMenu
    {
        public FrierScreen(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {

        }

        public override IMenu SetActive(bool active)
        {
            return base.SetActive(active);
        }
    }
}
