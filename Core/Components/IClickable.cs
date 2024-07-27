using Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Core.Components
{
    internal abstract class ICLickable : IActive
    {
        private Vector2 _position { get; set; }
        private Vector2 _size { get; set; }

        private Rectangle _bounds { get; set; }

        public ICLickable(Vector2 position, Vector2 size, bool isActive)
        {
            _position = position;
            _size = size;
            this.isActive = isActive;
        }

        public void ReceiveMouseClick()
        {

        }
        public abstract bool isActive { get ; set; }
        public Rectangle GetClickableBound()
        {
            return _bounds;
        }
        public virtual void SetPositionAndBoundingBox()
        {

        }
    }
}
