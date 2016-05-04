using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    public class MenuItemGroup : AbstractMenuItem
    {
        private string name;
        private Label label;

        public MenuItemGroup(string name, Label label)
        {
            this.name = name;
            this.label = label;
        }

        public string Name
        {
            get { return name; }
        }

        public Label Label
        {
            get { return label; }
            set { label = value; }
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Group; }
        }
    }
}
