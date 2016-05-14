using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;
using OrderSystem.Exceptions;
using OrderSystem.Helper;

namespace OrderSystem.Database.Tests
{
    /// <summary>
    /// Tests for the QueryBuilder class.
    /// </summary>
    [TestClass()]
    public class QueryBuilderTests
    {
        [TestMethod()]
        public void SelectFromTest()
        {
            SelectQueryBuilder test = new SelectQueryBuilder("user");
            test.SelectDistinct();
            test.SelectColumn("test");

            Assert.AreEqual("SELECT DISTINCT `test` FROM `user` ", test.Statement);
        }

        [TestMethod()]
        [ExpectedException(typeof(QueryBuilderException))]
        public void SelectFromTestInvalid()
        {
            // This should fail
            SelectQueryBuilder test = new SelectQueryBuilder("yolo");
            test.SelectAll();
            test.SelectDistinct();
            test.SelectColumn("test");
            test.SelectAll();

            Console.WriteLine(test.Statement);
        }

        [TestMethod]
        public void SelectFromWhereTest()
        {
            SelectQueryBuilder test = new SelectQueryBuilder("user");
            test.SelectColumn("username");
            test.SelectColumn("password");
            test.Where("name", QueryBuilder.ValueWrap("Markus"), CompareType.Equal);
            test.Where("tries", 5, CompareType.LessThanOrEqual);

            Assert.AreEqual("SELECT `username`, `password` FROM `user` WHERE name = 'Markus' AND tries <= 5 ", test.Statement);
        }

        [TestMethod()]
        public void SelectFromOrderByTest()
        {
            SelectQueryBuilder test = new SelectQueryBuilder("user");
            test.SelectAll()
                .OrderBy("username", OrderType.Ascending)
                .OrderBy("birthday", OrderType.Descending);

            Assert.AreEqual("SELECT * FROM `user` ORDER BY username ASC, birthday DESC ", test.Statement);
        }

        [TestMethod()]
        public void SelectFromLimitTest()
        {
            SelectQueryBuilder test = new SelectQueryBuilder("user");
            test.SelectAll()
                .Limit(10, 5);

            Assert.AreEqual("SELECT * FROM `user` LIMIT 10 OFFSET 5 ", test.Statement);

            SelectQueryBuilder test2 = new SelectQueryBuilder("user");
            test2.SelectAll()
                .Limit(4);

            Assert.AreEqual("SELECT * FROM `user` LIMIT 4 ", test2.Statement);
        }

        [TestMethod()]
        public void SelectInSelectTest()
        {
            SelectQueryBuilder test1 = new SelectQueryBuilder("user");
            test1.SelectColumn("id");

            SelectQueryBuilder test2 = new SelectQueryBuilder("yolo");
            test2.Select(test1);

            Assert.AreEqual("SELECT ( SELECT `id` FROM `user` ) FROM `yolo` ", test2.Statement);
        }

        [TestMethod()]
        public void InsertTest()
        {
            InsertQueryBuilder test = new InsertQueryBuilder("user");
            test.Insert("id", 5);
            test.Insert("username", QueryBuilder.ValueWrap("mustermann"));
            test.Insert("modified", "NULL");

            Assert.AreEqual("INSERT INTO `user` (`id`, `username`, `modified`) VALUES (5, 'mustermann', NULL)", test.Statement);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            UpdateQueryBuilder test = new UpdateQueryBuilder("user");
            test.Update("modified", "NOW()");
            test.Update("ip", QueryBuilder.ValueWrap("localhost"));
            test.Where("id", 5);

            Assert.AreEqual("UPDATE `user` SET `modified` = NOW(), `ip` = 'localhost' WHERE id = 5 ", test.Statement);
        }

        [TestMethod()]
        public void SelectFromJoinTest()
        {
            SelectQueryBuilder test = new SelectQueryBuilder("user u", false);
            test.SelectAll();
            test.Join(JoinType.Inner, "yolo x", "u.id = x.uid");

            Assert.AreEqual("SELECT * FROM user u INNER JOIN yolo x ON (u.id = x.uid) ", test.Statement);
        }
    }
}