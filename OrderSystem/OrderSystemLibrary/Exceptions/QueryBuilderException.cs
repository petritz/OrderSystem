using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystemLibrary.Exceptions
{
    /// <summary>
    /// The exception class for all query builder exceptions.
    /// </summary>
    public class QueryBuilderException : Exception
    {
        public QueryBuilderException(string message) : base(message)
        {
        }
    }
}