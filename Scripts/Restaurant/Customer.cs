using Core;
using Core.Components;
using Core.Debug;
using Fishing.Scripts.Restaurant.Customers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Restaurant
{
    public class CustomerReportEventArgs : EventArgs
    {
        public float satisfaction;
        public int foodOrderedID;
    }
    internal class Customer : IComponent
    {
        private float waitTime { get; set; }
        private MenuItem foodWanted { get; set; }
        private Rect customerSprite;

        private CustomerPersonality personality;

        public EventHandler OnCustomerLeaveEvent { get; set; }
        private bool hasReceivedFood { get; set; } = false;
        public Customer(Vector2 customerSpawnPosition,MenuItem foodWanted)
        {
            customerSprite = new Rect(customerSpawnPosition, new Vector2(28, 29), Color.Red, true, Layer.Entity);
            this.foodWanted = foodWanted;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            customerSprite.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager contentManager)
        {
        }
        public void ReceiveOrderedFood()
        {
            hasReceivedFood = true;
            Pay();
            OnCustomerLeaveEvent?.Invoke(this, new CustomerReportEventArgs() { satisfaction = 1/*CALCULATE SATISFACTION PROPERLY*/,foodOrderedID = foodWanted.ID});
        }
        private void Pay()
        {

        }
        public void Update(GameTime gameTime)
        {
            waitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(!hasReceivedFood&&waitTime < 0)
            {
                OnCustomerLeaveEvent?.Invoke(this,new CustomerReportEventArgs() { satisfaction = 0,foodOrderedID = foodWanted.ID});
            }
        }
    }
}
