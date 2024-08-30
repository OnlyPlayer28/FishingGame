using Core.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Components;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

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
        public int price { get; set; }
        public Vector2 minAndMaxDepth { get; set; }

        public float difficulty { get; set; }
        public FishSpecies species { get; set; }

        [JsonConstructor]
        public Fish(int ID, string name, Sprite sprite, int price, FishSpecies species,float rarity,float difficulty,Vector2 minAndMaxDepth)
            :base(ID, name,sprite,rarity)
        {
            this.price = price;
            this.species = species;
            this.difficulty = difficulty;
            this.minAndMaxDepth = minAndMaxDepth;
        }

        public Fish(Fish fish) 
            : base(fish.ID,fish.name,fish.sprite,fish.rarity)
        {
            this.price = fish.price;
            this.species = fish.species;
            this.difficulty = fish.difficulty;
            this.minAndMaxDepth = fish.minAndMaxDepth;
        }

        public override Fish Clone()
        {
            return new Fish(this.ID, this.name, this.sprite, this.price, this.species, this.rarity, this.difficulty, this.minAndMaxDepth);
        }
    }
}
