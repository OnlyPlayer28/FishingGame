using Core.Components;
using Fishing.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Core.InventoryManagement
{
    public class IAddableToInventory : ITaggable
    {
        public  int ID { get; set; }
        public string name { get; set; }
        public Sprite sprite { get; set; }
        /*[JsonProperty("spawnPropability")]*/
        public float rarity { get; set; }
        public Tags tags { get ; set; }

        public IAddableToInventory(int id, string name, Sprite sprite,float rarity)
        {
            this.name = name;
            this.sprite = new Sprite(sprite);
            this.rarity = rarity;
            this.ID = id;
        }
        public virtual IAddableToInventory Clone() { return null; }

    }
}
