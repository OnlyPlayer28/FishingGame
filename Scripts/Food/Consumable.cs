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
        public Consumable(int id, string name, Sprite sprite, float rarity,FoodState foodState,int price)
            : base(id, name, sprite, rarity)
        {
            this.foodState = foodState;
                this.price = price;
        }
        public Consumable(Consumable consumable,int price)
            : base(consumable.ID, consumable.name, consumable.sprite, consumable.rarity)
        {
            this.foodState = consumable.foodState;
            this.price = price;
        }


        public override IAddableToInventory Clone()
        {
            return new Consumable(ID,name,sprite,rarity,foodState,price);
        }

        public int price { get ; set; }
    }
}
