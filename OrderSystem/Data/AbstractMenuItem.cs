using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    /// <summary>
    /// Abstract class for all menu items
    /// </summary>
    public abstract class AbstractMenuItem
    {
        /// <summary>
        /// Get the type of the menu item
        /// </summary>
        public abstract MenuItemType Type { get; }
    }
}