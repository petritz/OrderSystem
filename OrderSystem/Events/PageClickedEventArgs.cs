using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Events
{
    /// <summary>
    /// The event arguments for the page clicked event.
    /// </summary>
    public class PageClickedEventArgs : MainEventArgs
    {
        private PageIdentifier pageIdentifier;

        public PageClickedEventArgs(PageIdentifier pageIdentifier) : base("menu_page_clicked")
        {
            this.pageIdentifier = pageIdentifier;
        }

        /// <summary>
        /// The page identifier that triggered this event
        /// </summary>
        public PageIdentifier PageIdentifier
        {
            get { return pageIdentifier; }
        }
    }
}