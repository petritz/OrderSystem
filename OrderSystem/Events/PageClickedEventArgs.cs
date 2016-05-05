using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Events
{
    public class PageClickedEventArgs : MainEventArgs
    {
        private PageIdentifier pageIdentifier;

        public PageClickedEventArgs(PageIdentifier pageIdentifier) : base("menu_page_clicked")
        {
            this.pageIdentifier = pageIdentifier;
        }

        public PageIdentifier PageIdentifier
        {
            get { return pageIdentifier; }
        }
    }
}