using Core.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Crafting
{
    public class CraftingRecipe
    {
        /// <summary>/// Key=item,Value=amount/// </summary>
        public Dictionary<int, int> input { get; set; }
        /// <summary>/// Key=item,Value=amount/// </summary>
        public Dictionary<int,int> output { get; set; }
        public int specialRequirements { get; set; } 

        public string name { get; set; }

        public CraftingRecipe(Dictionary<int,int> input,Dictionary<int,int> output,int specialRequirements,string name)
        {
            this.output = output;
            this.input = input;
            this.specialRequirements = specialRequirements;
            this.name = name;
        }

        public void Craft(Inventory inventory,int amount = 1)
        {
            foreach (KeyValuePair<int,int> item in input)
            {
                if (inventory.GetItemAmount(input[item.Key]) < item.Value)
                {
                    return;
                }
            }
            foreach (var item in output)
            {
                inventory.AddItem(Game1.GetItem(item.Key), item.Value);
            }
            foreach (var item in input)
            {
                inventory.AddItem(Game1.GetItem(item.Key), -item.Value);
            }

        }
        /// <summary>
        /// Checks if an item is craftable, if it is, returns true otherwise returns false
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="canCraft"></param>
        /// <param name="amount"></param>
        public void Craft(Inventory inventory,out bool canCraft, int amount = 1)
        {
            foreach (KeyValuePair<int, int> item in input)
            {
                if (inventory.GetItemAmount(input[item.Key]) < item.Value)
                {
                    canCraft = false;
                    return;
                }

            }
            canCraft = true;
        }
        


    }
}
