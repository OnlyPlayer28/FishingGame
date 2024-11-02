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

        internal EventHandler OnLoopEvent { get; set; }

        internal float startDelay = 0f;
        internal float length = 1f;

        internal float timer = 0;
        internal float? delayTimer = 0f;
        internal bool isLooping = false;

        internal IAffectableByEffects itemAffectedByEffectRef {  get; set; }
        internal IAffectableByEffects originalItemState { get; private set; }

        public IEffect(float length,IAffectableByEffects itemAffectableByEffectRef, float startDelay = 0f,bool isLooping= false) 
        {
            this.length = length;
            this.startDelay = startDelay;
            this.isLooping = isLooping;
            delayTimer = this.startDelay;
            this.itemAffectedByEffectRef = itemAffectableByEffectRef;
            OnDestroyEvent+= this.OnDestroy;
            OnLoopEvent += OnLoop;
            timer = this.length;

            originalItemState =  (IAffectableByEffects)this.itemAffectedByEffectRef.Clone();

        }
        internal virtual void OnDestroy(Object o,EventArgs e)
        {
            //RESET ALL THE FIELDS BACK TO THEIR ORIGINAL VALUES-> WHEN EFFECT ENDS ALL ITS 
        }

        internal virtual void OnLoop(Object o,EventArgs e)
        {

        }
        public virtual void Update(GameTime gameTime)
        {
            if(delayTimer != null&&delayTimer >= 0) {  delayTimer-= (float)gameTime.ElapsedGameTime.TotalSeconds; return; }
            else 
            { 
                 delayTimer = null;
            }

            if (timer > 0)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (isLooping)
                {
                    OnLoopEvent?.Invoke(this, EventArgs.Empty);
                    timer = length;
                }else
                {

                    OnDestroyEvent?.Invoke(this,EventArgs.Empty);
                }
            }
        }
    }
}
