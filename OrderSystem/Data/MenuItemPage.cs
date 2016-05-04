using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    public class MenuItemPage : AbstractMenuItem
    {
        private string name;
        private string imageResource;
        private Button button;

        public MenuItemPage(string name, string imageResource, Button button)
        {
            this.name = name;
            this.imageResource = imageResource;
            this.button = button;
        }

        public string Name
        {
            get { return name; }
        }

        public string ImageResource
        {
            get { return imageResource; }
        }

        public Button Button
        {
            get { return button; }
            set { button = value; }
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Page; }
        }
    }
}