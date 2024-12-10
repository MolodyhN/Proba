using Microsoft.VisualStudio.TestTools.UnitTesting;
using lib10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib10.Tests
{
    [TestClass()]
    public class ManufacturerComparerTests
    {
        [TestMethod]
        public void Compare_ShouldReturnZero_WhenModelsAreEqual()
        {
            var engine1 = new Engine { Model = "ModelA", Power = 100, Gender = "M" };
            var engine2 = new Engine { Model = "ModelA", Power = 150, Gender = "W" };
            var comparer = new ManufacturerComparer();
            var result = comparer.Compare(engine1, engine2);
            Assert.AreEqual(0, result); 
        }

        [TestMethod]
        public void Compare_ShouldReturnNegative_WhenFirstModelIsLessThanSecond()
        {
            var engine1 = new Engine { Model = "ModelA", Power = 100, Gender = "M" };
            var engine2 = new Engine { Model = "ModelB", Power = 150, Gender = "W" };
            var comparer = new ManufacturerComparer();
            var result = comparer.Compare(engine1, engine2);
            Assert.IsTrue(result < 0); 
        }

        [TestMethod]
        public void Compare_ShouldReturnPositive_WhenFirstModelIsGreaterThanSecond()
        {
            var engine1 = new Engine { Model = "ModelB", Power = 150, Gender = "W" };
            var engine2 = new Engine { Model = "ModelA", Power = 100, Gender = "M" };
            var comparer = new ManufacturerComparer();
            var result = comparer.Compare(engine1, engine2);
            Assert.IsTrue(result > 0); 
        }

        [TestMethod]
        public void Compare_ShouldReturnZero_WhenBothModelsAreNull()
        {
            Engine engine1 = null;
            Engine engine2 = null;
            var comparer = new ManufacturerComparer();
            var result = comparer.Compare(engine1, engine2);
            Assert.AreEqual(0, result); 
        }
    }

}
