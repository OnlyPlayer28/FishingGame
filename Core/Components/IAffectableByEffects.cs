using Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    public interface IAffectableByEffects:IPosition
    {
        public float scale { get; set; }
        public float rotation { get; set; }
        public float transparency { get; set; }
    }
}
