using Core.Components;
using Core.InventoryManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.Food
{
    public enum FoodState
    {
        Raw,
        Grilled,
        Fried,
        Cooked
    }
    public class Consumable : IAddableToInventory, ISellable
    {
        public FoodState foodState { get; set; }
        [JsonConstructor]
        public Consumable(int id, string name, Sprite sprite, float rarity,FoodState foodState)
            : base(id, name, sprite, rarity)
        {
            this.foodState = foodState;
        }
        public Consumable(Consumable consumable)
            : base(consumable.ID, consumable.name, consumable.sprite, consumable.rarity)
        {
            this.foodState = consumable.foodState;
        }


        public override IAddableToInventory Clone()
        {
            return new Consumable(this);
        }

        public int price { get ; set; }
    }
}
