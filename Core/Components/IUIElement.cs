using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    public enum AnchorPoint
    {
        TopLeft,
        Top,
        TopRight,
        Right,
        Centered,
        Left,
        BottomLeft,
        Bottom,
        BottomRight,
        None
    }
    internal interface IUIElement
    {
         public bool isHoveredOverByUser { get; set; }
        public AnchorPoint anchorPoint { get; set; }
        public Vector2 anchorOffset { get; set; }

        public void DrawUI(SpriteBatch spriteBatch);
    }
}
