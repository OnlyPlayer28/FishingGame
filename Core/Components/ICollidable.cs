using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    public interface ICollidable
    {
        //public bool isSolid { get;  set; }
        public  Rectangle boundingBox { get;   set; }

    }
}
