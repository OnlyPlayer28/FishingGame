using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Restaurant
{
    public class RestaurantManager
    {
        public int openingHour { get; set; } 
        public int openingMinute { get; set; }

        public int closingHour { get; set; } 
        public int closingMinute { get; set; }

        public string restaurantName { get;private set; }
        public EventHandler OnRestaurantOpenEvent { get; set; }
        public EventHandler OnRestarantCloseEvent { get; set; }

        public bool isOpen { get;private set;}
        public RestaurantManager(string name)
        {
            restaurantName = name;
            DayNightSystem.OnHourPassEvent += OnHourPass;
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

        public void OnHourPass(Object o,DateEventArgs e)
        {
            if((openingHour < closingHour&&(e.date.hours>= openingHour && e.date.hours<closingHour))||(openingHour>closingHour&&(e.date.hours>=openingHour||e.date.hours<closingHour))) { isOpen = true; }
            else {  isOpen = false; }

        }
        public void Update(GameTime gameTime)
        {

        }

    }
}
