using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lib10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace lib10.Tests
{
    [TestClass]
    public class ICEngineTests
    {
        private StringBuilder outputStringBuilder;
        private StringWriter stringWriter;
        [TestInitialize]
        public void TestInitialize()
        {
            outputStringBuilder = new StringBuilder();
            stringWriter = new StringWriter(outputStringBuilder);
            Console.SetOut(stringWriter);
        }
        [TestMethod]
        public void Test_Constructor_ShouldInitializeProperties()
        {
            var icEngine = new ICEngine("EcoEngine", 150, "M", 8.5);

            Assert.AreEqual("EcoEngine", icEngine.Model);
            Assert.AreEqual(150, icEngine.Power);
            Assert.AreEqual("M", icEngine.Gender);
            Assert.AreEqual(8.5, icEngine.FuelEfficiency);
        }

        [TestMethod]
        public void Test_SetValidFuelEfficiency_ShouldSetFuelEfficiency()
        {
            var icEngine = new ICEngine();
            icEngine.FuelEfficiency = 10;

            Assert.AreEqual(10, icEngine.FuelEfficiency);
        }

        [TestMethod]
        public void Test_SetInvalidFuelEfficiency_ShouldThrowException()
        {
            var icEngine = new ICEngine();
            stringWriter.Flush();
            icEngine.FuelEfficiency = -10;
            string consoleOutput = outputStringBuilder.ToString().Trim();

            Assert.AreEqual("Ошибка: Заданный аргумент находится вне диапазона допустимых значений.", consoleOutput);
        }

        [TestMethod]
        public void Test_ShallowCopy_ShouldCreateShallowCopy()
        {
            var original = new ICEngine("EcoEngine", 150, "M", 8.5);
            var copy = original.ShallowCopy() as ICEngine;

            Assert.IsNotNull(copy);
            Assert.AreEqual(original.Model, copy.Model);
            Assert.AreEqual(original.Power, copy.Power);
            Assert.AreEqual(original.Gender, copy.Gender);
            Assert.AreEqual(original.FuelEfficiency, copy.FuelEfficiency);
            Assert.AreNotSame(original, copy);
        }

        [TestMethod]
        public void Test_Clone_ShouldCreateDeepCopy()
        {
            var original = new ICEngine("EcoEngine", 150, "M", 8.5);
            var clone = original.Clone() as ICEngine;

            Assert.AreEqual(original.Model, clone.Model);
            Assert.AreEqual(original.Power, clone.Power);
            Assert.AreEqual(original.Gender, clone.Gender);
            Assert.AreEqual(original.FuelEfficiency, clone.FuelEfficiency);
        }

        [TestMethod]
        public void Test_RandomInit_ShouldSetRandomValues()
        {
            var icEngine = new ICEngine();
            icEngine.RandomInit();

            Assert.IsNotNull(icEngine.Model);
            Assert.IsTrue(icEngine.Power > 0);
            Assert.IsTrue(icEngine.Gender == "M" || icEngine.Gender == "W");
            Assert.IsTrue(icEngine.FuelEfficiency >= 5 && icEngine.FuelEfficiency <= 15);
        }
        [TestMethod]
        public void Test_Show_ShouldPrintAllProperties()
        {
            var icEngine = new ICEngine("EcoEngine", 150, "M", 8.5);
            icEngine.Show();
            stringWriter.Flush();
            string consoleOutput = outputStringBuilder.ToString().Trim();

            StringAssert.Contains(consoleOutput, "Model: EcoEngine");
            StringAssert.Contains(consoleOutput, "Power: 150 HP");
            StringAssert.Contains(consoleOutput, "Gender: M");
            StringAssert.Contains(consoleOutput, "Fuel Efficiency: 8,5 L/100km");

        }
    }
}