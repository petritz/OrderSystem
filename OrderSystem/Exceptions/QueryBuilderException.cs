using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Exceptions
{
    public class QueryBuilderException : Exception
    {
        public QueryBuilderException(string message) : base(message)
        {
        }
    }
}