using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Database.Tests
{
    [TestClass()]
    public class QueryBuilderTests
    {
        [TestMethod()]
        public void QueryBuilderTest()
        {
            SelectQueryBuilder test1 = new SelectQueryBuilder("user");
            test1.SelectDistinct();
            test1.SelectColumn("test");

            Assert.AreEqual("SELECT DISTINCT `test` FROM `user` ", test1.Statement);
        }
    }
}