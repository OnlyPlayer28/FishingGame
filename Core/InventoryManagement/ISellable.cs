using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.InventoryManagement
{
    public interface ISellable:IAddableToInventory
    {
        public int price { get; set; }

    }
}
