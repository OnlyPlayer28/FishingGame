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

    }
}
