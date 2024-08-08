using Core.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Components;

namespace Fishing.Scripts.Food
{
    public enum FishSpecies
    {
        Squid,
        Tuna,
        Salmon
    }
    public class Fish : IAddableToInventory,ISellable
    {
        public int ID { get ; set ; }
        public string name { get ; set ; }
        public Sprite sprite { get; set; }
        public int price { get; set; }

        public FishSpecies species { get; set; }

        public Fish(int ID, string name, Sprite sprite, int price, FishSpecies species)
        {
            this.ID = ID;
            this.name = name;
            this.sprite = sprite;
            this.price = price;
            this.species = species;
        }
    }
}
