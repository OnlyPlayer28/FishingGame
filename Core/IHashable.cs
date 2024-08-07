using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class IHashable 
    {
        public int hash { get;private set; }
        public IHashable()
        {
            hash = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
        }
    }
}
