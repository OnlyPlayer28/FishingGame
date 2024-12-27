using Core;
using Fishing.Scripts.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Restaurant
{
    public enum RestaurantState
    {
        NoCustomer,
        CustomerWaiting
    }
    public class RestaurantManager
    {
        [JsonIgnore]
        public int openingHour { get; set; }
        [JsonIgnore]
        public int openingMinute { get; set; }
        [JsonIgnore]
        public int closingHour { get; set; }
        [JsonIgnore]
        public int closingMinute { get; set; }
        public int peakHour { get; private set; } = 7;
        [JsonIgnore]
        public string restaurantName { get; private set; }
        [JsonIgnore]
        public EventHandler OnRestaurantOpenEvent { get;private set; }
        [JsonIgnore]
        public EventHandler OnRestarantCloseEvent { get; private set; }
        [JsonIgnore]
        public EventHandler OnCustomerSpawnEvent { get;private set; }
        [JsonIgnore]
        public bool isOpen { get; private set; } = true;
        [JsonRequired]
        private int maxCostumersPerHour { get; set; } = 5;
        private int currentCustomerCountPerHour { get; set; }
        private int totalCustumersPerDay { get; set; }

        public float peakHourMultiplier { get;private set; } = .2f;

        [JsonIgnore]
        public bool isPeakHour { get; private set; } = false;
        [JsonRequired]
        private float minTimeBetweenCustumers { get;  set; } = 9;
        private float customerTimer { get; set; }

        private Customer currentCustomer { get; set; }
        [JsonIgnore]
        public RestaurantMenu currentRestaurantMenu { get;private set; }
        [JsonRequired]
        private int maxItemsPerMenu { get; set; } = 6;
        [JsonIgnore]
        public RestaurantState restaurantState { get; set; }
        private Vector2 customerSpawnPosition { get; set; } = new Vector2(50, 45);
        public RestaurantManager(string name)
        {
            currentRestaurantMenu = new RestaurantMenu(maxItemsPerMenu);
            customerTimer = minTimeBetweenCustumers;
            restaurantName = name;
            DayNightSystem.OnHourPassEvent += OnHourPass;
            OnCustomerSpawnEvent+= OnCustomerSpawn;
        }
        public RestaurantManager SetRestaurantName (string name)
        {
            this.restaurantName = name;
            return this;
        }
        public RestaurantManager SetOpeningHours(int hour,int minute)
        {
            openingHour = hour;
            openingMinute = minute;
            return this;
        }
        public RestaurantManager SetClosingHours(int hour, int minute)
        {
            closingHour = hour;
            closingMinute = minute;
            return this;
        }
        private void OnCustomerSpawn(Object o,EventArgs e)
        {
            GenerateNextCustomerWaitTime();
            if(currentCustomerCountPerHour>=(isPeakHour?maxCostumersPerHour*(1+peakHourMultiplier):maxCostumersPerHour)) { return; }
            currentCustomerCountPerHour++;
            restaurantState = RestaurantState.CustomerWaiting;
            currentCustomer = new Customer(customerSpawnPosition, currentRestaurantMenu.GetRandomMenuItem());
        }
        private void GenerateNextCustomerWaitTime()
        {
            customerTimer = (isPeakHour?(1 - peakHourMultiplier):1) * (minTimeBetweenCustumers+( (float)currentCustomerCountPerHour/2)+ReferenceHolder.random.Next(1,4));
        }
        public void OnHourPass(Object o,DateEventArgs e)
        {
            totalCustumersPerDay += currentCustomerCountPerHour;
            currentCustomerCountPerHour = 0;
            if (e.date.hours == peakHour) { isPeakHour = true; } else { isPeakHour= false; }

            if((openingHour < closingHour&&(e.date.hours>= openingHour && e.date.hours<closingHour))||(openingHour>closingHour&&(e.date.hours>=openingHour||e.date.hours<closingHour))) { isOpen = true; }
            else {  isOpen = false; }

        }
        public void Update(GameTime gameTime)
        {
            if (Game1.stateManager.GetActiveGameState().GetType() == typeof(RestaurantScene)&&isOpen) 
            {
                if (restaurantState == RestaurantState.NoCustomer)
                {
                    customerTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (customerTimer <= 0)
                    {
                        customerTimer = 100;
                        OnCustomerSpawnEvent?.Invoke(this, EventArgs.Empty);
                    }
                }else
                {
                    currentCustomer.Update(gameTime);
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (restaurantState == RestaurantState.CustomerWaiting&& Game1.stateManager.GetActiveGameState().GetType() == typeof(RestaurantScene) && isOpen)
            {
                currentCustomer.Draw(spriteBatch);
            }
        }

    }
}
