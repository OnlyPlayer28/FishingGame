using Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Effects
{
    public class DefaultEffects
    {
    }

    public class Fade : IEffect
    {
        float fadeIncrement = 0f;
        public Fade(float length, IAffectableByEffects itemAffectableByEffectRef, float startDelay = 0, bool isLooping = false) 
            : base(length, itemAffectableByEffectRef, startDelay, isLooping)
        {
            fadeIncrement = 1/length;
        }
        public override void Update(GameTime gameTime)
        {itemAffectedByEffectRef.transparency -= fadeIncrement*(float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

        }
    }
}
