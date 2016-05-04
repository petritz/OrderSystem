using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Data;

namespace OrderSystem.Models
{
    public class MenuRegistry
    {
        private List<AbstractMenuItem> list;

        public MenuRegistry()
        {
            list = new List<AbstractMenuItem>();
            //TODO add items
        }

        public List<AbstractMenuItem> List
        {
            get { return list; }
        }
    }
}
