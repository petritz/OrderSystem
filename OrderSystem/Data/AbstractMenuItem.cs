using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    public abstract class AbstractMenuItem
    {
        public abstract MenuItemType Type { get; }
    }
}