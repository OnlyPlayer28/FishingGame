using Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI
{
    internal interface IUIElement : IComponent, IActive, ITaggable,ILayerable,IHoverable
    {
        public Vector2 position { get; set; }
        public Vector2 size { get; set; }
    }
}
