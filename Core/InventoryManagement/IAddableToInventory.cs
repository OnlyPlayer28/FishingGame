using Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.InventoryManagement
{
    public interface IAddableToInventory
    {
        public  int ID { get; set; }
        public string name { get; set; }
        public Sprite sprite { get; set; }
    }
}
