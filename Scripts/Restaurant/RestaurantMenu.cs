using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Restaurant
{
    public struct MenuItem
    {
        public int ID;
        public int price;

        public override string ToString() => $"{Game1.GetItem(ID).name}: {price}";

    }
    internal class RestaurantMenu
    {
        public List<MenuItem> menuItems { get; private set; }

        private int maxSellableItems { get; set; }

        public RestaurantMenu(int maxSellableItems)
        {
            menuItems = new List<MenuItem>();
            this.maxSellableItems = maxSellableItems;
        }

        public RestaurantMenu AddMenuItem(int ID,int price = -1)
        {
            if(menuItems.Any(item => item.ID == ID)) { return this; }
            else
            {
                menuItems.Add(new MenuItem { ID = ID, price = price });
                return this;
            }
        }

        public void ModifyItemPrice(int ID,int newPrice)
        {
            MenuItem @object = menuItems.Where(p => p.ID == ID).FirstOrDefault();
            @object.price = newPrice;
        }

        public void PrintMenuToConsole()
        {
            string textToReturn=default;
            menuItems.ForEach(p => textToReturn += p.ToString());

            System.Diagnostics.Debug.WriteLine(textToReturn);
        }

        public MenuItem GetRandomMenuItem(int maxPrice = -1)
        {
            if (menuItems.Count == 0 || menuItems.All(p => p.price > maxPrice)) { return new MenuItem {ID=-1,price=-1 }; }
            else
            {
                MenuItem[] objects = menuItems.Where(p=>p.price <= maxPrice).ToArray();
                return objects[ReferenceHolder.random.Next(0,objects.Length)];
            }
        }
    }
}
