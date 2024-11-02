using Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.InventoryManagement
{
    public class InventoryEventArgs : EventArgs
    {
        public int ID;
        public int totalItemAmount;
        public IAddableToInventory item;
    }
    public class Inventory :ITaggable
    {
        public string name { get; set ; }

        //Item1 = amount; Item2 = item
        public List<Tuple<int,IAddableToInventory>> inventory { get; set; }

        public int maxStackSize { get; private set; } 

        public EventHandler<InventoryEventArgs> OnInventoryModifyEvent { get; set; }
        public Inventory(string name = "defaultInventory",int maxStackSIze = 99)
        {
            this.name = name;
            this.maxStackSize = maxStackSIze;
            inventory = new List<Tuple<int, IAddableToInventory>> ();
            OnInventoryModifyEvent += OnInventoryModify;

        }
        public void OnInventoryModify(Object sender, InventoryEventArgs e)
        {

            if (e.totalItemAmount <= 0)
            {
                inventory.Remove(new Tuple<int,IAddableToInventory> (e.totalItemAmount,e.item));
            }

        }
        /// <summary> 
        /// Retrieves the first occurance of a specific item 
        /// </summary>
        /// <param name="ID"> ID of the desired item></param>
        /// <returns>A positive int if item was found, otherwise returns -1</returns>
        public int GetItemAmount(int ID)
        {
            return inventory.Any(p=>p.Item2.ID == ID)?inventory.Find(p=>p.Item2.ID ==ID).Item1:-1;

        }
        public void AddItem( IAddableToInventory @object,int amount=1)
        {
            if(inventory.Any(p=>p.Item2.ID==@object.ID&&amount<0&&p.Item1==0))
            {
                return;
            }
            if(inventory.All(p=>p.Item2.ID != @object.ID))
            {
                inventory.Add(new Tuple<int, IAddableToInventory>(amount, (IAddableToInventory)@object.Clone()));
                OnInventoryModifyEvent?.Invoke(this, new InventoryEventArgs { ID = inventory.Last().Item2.ID, totalItemAmount = inventory.Last().Item1 ,item = inventory.Last().Item2});
                return;
            }
            for (int i =0; i < inventory.Count;i++)
            {
                if (inventory[i].Item2.ID == @object.ID)
                {
                    inventory[i] = new Tuple<int, IAddableToInventory>(inventory[i].Item1 + amount, inventory[i].Item2);
                    OnInventoryModifyEvent?.Invoke(this, new InventoryEventArgs { ID = inventory[i].Item2.ID, totalItemAmount = inventory[i].Item1 ,item = inventory[i].Item2 });
                    return;
                }
            }


        }
        public static int GenerateLoot(params IAddableToInventory[] items)
        {
            float sumOfPropabilities = items.Sum(p => p.rarity)/**100*/;

            float randomValue = ReferenceHolder.random.Next(1, (int)(sumOfPropabilities));
            //randomValue /= 100;

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
