using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderSystem.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Helper.Tests
{
    [TestClass()]
    public class QueryHelperTests
    {
        [TestMethod()]
        public void GetIpQueryTest()
        {
            Assert.AreEqual("SELECT `host` FROM information_schema.processlist WHERE ID = connection_id() ",
                QueryHelper.GetIpQuery().Statement);
        }
    }
}