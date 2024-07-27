using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts
{
    public class Player
    {
        public int fishingLevel { get; set; } = 0;

        public float fishHookReactionTime { get; private set; } = 1.5f;
        Random random = new Random();
        public Player()
        {

        }

        public float CalculateCurrentFishCatchTime(Vector2 minAndMax)
        {
            return (float)random.Next((int)minAndMax.X,(int)minAndMax.Y);
        }
    }
}
