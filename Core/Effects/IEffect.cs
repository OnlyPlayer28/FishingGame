using Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Effects
{
    public abstract class IEffect
    {
        public EventHandler OnDestroyEvent { get; set; }

        private float startDelay = 0f;
        private float length = 1f;

        private float timer = 0;
        private float? delayTimer = 0f;
        private bool isLooping = false;

        public IAffectableByEffects itemAffectedByEffectRef {  get; set; }
        public IEffect(float length,IAffectableByEffects itemAffectableByEffectRef, float startDelay = 0f,bool isLooping= false) 
        {
            this.length = length;
            this.startDelay = startDelay;
            this.isLooping = isLooping;
            delayTimer = this.startDelay;
            this.itemAffectedByEffectRef = itemAffectableByEffectRef;
            OnDestroyEvent+= this.OnDestroy;
        }
        internal virtual void OnDestroy(Object o,EventArgs e)
        {
            //RESET ALL THE FIELDS BACK TO THEIR ORIGINAL VALUES-> WHEN EFFECT ENDS ALL ITS 
        }
        public virtual void Update(GameTime gameTime)
        {
            if(delayTimer != null&&delayTimer >= 0) {  delayTimer-= (float)gameTime.ElapsedGameTime.TotalSeconds; return; }
            else { timer = length; delayTimer = null; }

            if (timer >= 0)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (isLooping)
                {
                    timer = length;
                }else
                {
                    OnDestroyEvent?.Invoke(this,EventArgs.Empty);
                }
            }
        }
    }
}
