using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    public interface IActive
    {
        public bool isActive { get;set;}
        public IActive SetActive(bool active);
    }
}
