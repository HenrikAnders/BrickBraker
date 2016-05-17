using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockBreaker
{
    [TestClass
    ]
    class Test
    {
        [TestMethod]
        public void testList(List<Blocks.Block> list)
        {
            if (list.Count == 0)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void testInitLive(int live)
        {
            if (live != 3)
            {
                Assert.Fail();
            }
        }
    }
}
