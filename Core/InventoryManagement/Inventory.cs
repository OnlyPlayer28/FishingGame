using Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.InventoryManagement
{
    public class Inventory :ITaggable
    {
        public string name { get; set ; }

        public List<Tuple<int,IAddableToInventory>> inventory { get; set; }

        public int maxStackSize { get; private set; } 

        public Inventory(string name = "defaultInventory",int maxStackSIze = 99)
        {
            this.name = name;
            this.maxStackSize = maxStackSIze;
            inventory = new List<Tuple<int, IAddableToInventory>> ();

        }
        /// <summary> 
        /// Retrieves the first occurance of a specific item 
        /// </summary>
        /// <param name="ID"> ID of the desired item></param>
        /// <returns>A positive int if item was found, otherwise returns -1</returns>
        public int GetItemAmount(int ID)
        {
            return inventory.Exists(p=>p.Item2.ID == ID)?inventory.Find(p=>p.Item2.ID ==ID).Item1:-1;

        }

        public static int GenerateLoot(params IAddableToInventory[] items)
        {
            float sumOfPropabilities = items.Sum(p => p.rarity)*100;
            float randomValue = ReferenceHolder.random.Next(1, (int)(sumOfPropabilities));
            randomValue /= 100;

            int IDToReturn = -1;
            items = items.OrderBy(p => p.rarity).ToArray();
            float currentRarity = 0;
            for (int i = 0; i < items.Length; i++)
            {
                currentRarity += items[i].rarity;
                if (currentRarity >= randomValue) { IDToReturn = items[i].ID; break; }
            }
            return IDToReturn;

        }
    }
}
