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

    public   class Fade : IEffect
    {
        private float fadeIncrement = 0f;

        private float originalTransparency;
        public Fade(float length, IAffectableByEffects itemAffectableByEffectRef, float startDelay = 0, bool isLooping = false) 
            : base(length, itemAffectableByEffectRef, startDelay, isLooping)
        {
            fadeIncrement = 1/length;
            originalTransparency = base.itemAffectedByEffectRef.transparency;
        }
        internal override void OnLoop(object o, EventArgs e)
        {
            itemAffectedByEffectRef.transparency = originalTransparency;
        }
        public override void Update(GameTime gameTime)
        {
            if (base.delayTimer == null)
            {
                itemAffectedByEffectRef.transparency -= fadeIncrement * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gameTime);

        }
    }

    public class Shake : IEffect
    {
        private Random random = new Random();
        private float intensity;

    
        public Shake(float intensity,float length, IAffectableByEffects itemAffectableByEffectRef, float startDelay = 0, bool isLooping = false) 
            : base(length, itemAffectableByEffectRef, startDelay, isLooping)
        {
            this.intensity = intensity;
        }
        internal override void OnLoop(object o, EventArgs e)
        {
            itemAffectedByEffectRef.posOffset = originalItemState.posOffset;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            itemAffectedByEffectRef.posOffset = new Vector2(random.Next(-100,100)/10,random.Next(-100,100)/10)*intensity;
        }
    }

    public class Scale
    {

    }
}
