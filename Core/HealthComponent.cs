using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COre
{
    public class HealthComponent
    {

        public float health { get; private set; }
        public float maxHealth { get; private set; }

        public EventHandler OnEntityDeathEvent { get; private protected set; }
        public HealthComponent(float maxHealth)
        {
            this.health = maxHealth;
            this.maxHealth = maxHealth;
        }

        
        public void TakeDamage(float damage) 
        {
            health -= damage;
            if (health < 0)
            {
                OnEntityDeathEvent?.Invoke(this, EventArgs.Empty);

                OnDeath();
            }
        }
        public void AddHealth(float amount)
        {
            health = health+amount > maxHealth ? maxHealth : health+amount;
        }
        public virtual Rectangle GetCollision()
        {
            
            return new Rectangle();
        }
        public virtual void OnDeath()
        {

        }
    }
}
