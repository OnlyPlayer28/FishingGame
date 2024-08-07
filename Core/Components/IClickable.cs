using Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    public abstract class ICLickable : IActive,ILayerable
    {
        public Vector2 position { get; set; }
        public Vector2 size { get; set; }

        private Rectangle _bounds { get; set; }
        public  bool isActive { get; set; }
        public  float layer { get; set; }
        public ICLickable(Vector2 position, Vector2 size, bool isActive)
        {
            this.position = position;
            this.size = size;
            this.isActive = isActive;
        }

        public abstract void OnMouseClick();


        public Rectangle GetClickableBound()
        {
            return _bounds;
        }
        public virtual void SetPositionAndBoundingBox()
        {

        }
    }
}
