using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Exceptions
{
    /// <summary>
    /// Base class for all KeyValueStorage/Reader/Writer Exceptions
    /// </summary>
    public class KeyValueException : Exception
    {
        public KeyValueException(string message) : base(message)
        {
        }
    }
}
