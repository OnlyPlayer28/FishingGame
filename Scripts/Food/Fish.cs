using Core.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Components;
using Microsoft.Xna.Framework;

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

        public Fish(int ID, string name, Sprite sprite, int price, FishSpecies species,float rarity,float difficulty,Vector2 minAndMaxDepth)
        {
            this.ID = ID;
            this.name = name;
            this.sprite = sprite;
            this.price = price;
            this.species = species;
            this.rarity = rarity;
            this.difficulty = difficulty;
            this.minAndMaxDepth = minAndMaxDepth;
        }

        public override object Clone()
        {
            return new Fish(this.ID, this.name, this.sprite, this.price, this.species, this.rarity, this.difficulty, this.minAndMaxDepth);
        }
    }
}
