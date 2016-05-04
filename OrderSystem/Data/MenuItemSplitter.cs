using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    public class MenuItemSplitter : AbstractMenuItem
    {
        private Separator seperator;

        public MenuItemSplitter(Separator seperator)
        {
            this.seperator = seperator;
        }

        public Separator Seperator
        {
            get { return seperator; }
            set { seperator = value; }
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Splitter; }
        }
    }
}
