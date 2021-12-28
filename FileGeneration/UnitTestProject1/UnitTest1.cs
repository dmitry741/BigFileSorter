﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using RecordLib;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Record r1 = new Record("415. Apple");
            Record r2 = new Record("1. Apple");

            Assert.IsTrue(r1.CompareTo(r2) == 1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            const string line1 = "415. Apple";
            const string line2 = "30432. Something something something";
            const string line3 = "32. Cherry is the best";
            const string line4 = "2. Banana is yellow";
            const string line5 = "1. Apple";

            List<Record> records = new List<Record>
            {
                new Record(line1),
                new Record(line2),
                new Record(line3),
                new Record(line4),
                new Record(line5)
            };

            records.Sort();

            Assert.IsTrue(records[0].Line == line5);
            Assert.IsTrue(records[1].Line == line1);
            Assert.IsTrue(records[2].Line == line4);
            Assert.IsTrue(records[3].Line == line3);
            Assert.IsTrue(records[4].Line == line2);
        }
    }
}
