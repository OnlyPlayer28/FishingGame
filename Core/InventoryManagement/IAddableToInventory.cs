using Core.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.InventoryManagement
{
    public class IAddableToInventory
    {
        public  int ID { get; set; }
        public string name { get; set; }
        public Sprite sprite { get; set; }
        [JsonProperty("spawnPropability")]
        public float rarity { get; set; }

        public virtual object Clone() { return null; }

    }
}
