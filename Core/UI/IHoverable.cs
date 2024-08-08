using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI
{
    public interface IHoverable
    {
        public void OnMouseOver(Object sender, EventArgs e);
    }
}
