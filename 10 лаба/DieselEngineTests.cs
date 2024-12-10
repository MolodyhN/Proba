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
    public class DieselEngineTests
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
            var dieselEngine = new DieselEngine("DieselPro", 200, "M", 7.5, 400);

            Assert.AreEqual("DieselPro", dieselEngine.Model);
            Assert.AreEqual(200, dieselEngine.Power);
            Assert.AreEqual("M", dieselEngine.Gender);
            Assert.AreEqual(7.5, dieselEngine.FuelEfficiency);
            Assert.AreEqual(400, dieselEngine.MaxTorque);
        }

        [TestMethod]
        public void Test_SetValidMaxTorque_ShouldSetMaxTorque()
        {
            var dieselEngine = new DieselEngine();
            dieselEngine.MaxTorque = 500;
            Assert.AreEqual(500, dieselEngine.MaxTorque);
        }

        [TestMethod]
        public void Test_SetInvalidMaxTorque_ShouldThrowException()
        {
            var dieselEngine = new DieselEngine();
            stringWriter.Flush();
            dieselEngine.MaxTorque = -100;
            string consoleOutput = outputStringBuilder.ToString().Trim();
            Assert.AreEqual("Ошибка: Заданный аргумент находится вне диапазона допустимых значений.", consoleOutput);
        }

        [TestMethod]
        public void Test_ShallowCopy_ShouldCreateShallowCopy()
        {
            var original = new DieselEngine("DieselPro", 200, "M", 7.5, 400);
            var copy = original.ShallowCopy() as DieselEngine;

            Assert.IsNotNull(copy);
            Assert.AreEqual(original.Model, copy.Model);
            Assert.AreEqual(original.Power, copy.Power);
            Assert.AreEqual(original.Gender, copy.Gender);
            Assert.AreEqual(original.FuelEfficiency, copy.FuelEfficiency);
            Assert.AreEqual(original.MaxTorque, copy.MaxTorque);
            Assert.AreNotSame(original, copy);
        }

        [TestMethod]
        public void Test_Clone_ShouldCreateDeepCopy()
        {
            var original = new DieselEngine("DieselPro", 200, "M", 7.5, 400);
            var clone = original.Clone() as DieselEngine;

            Assert.AreEqual(original.Model, clone.Model);
            Assert.AreEqual(original.Power, clone.Power);
            Assert.AreEqual(original.Gender, clone.Gender);
            Assert.AreEqual(original.FuelEfficiency, clone.FuelEfficiency);
            Assert.AreEqual(original.MaxTorque, clone.MaxTorque);

        }
        [TestMethod]
        public void Test_Init_ShouldSetMaxTorqueFromInput()
        {
            var dieselEngine = new DieselEngine();
            string input = "TestModel\n300\nM\n15\n450\n"; 
            using (var stringReader = new StringReader(input))
            using (var stringWriter = new StringWriter())
            {
                Console.SetIn(stringReader);
                Console.SetOut(stringWriter);
                dieselEngine.Init();
            }
            Assert.AreEqual("TestModel", dieselEngine.Model);
            Assert.AreEqual(300, dieselEngine.Power);
            Assert.AreEqual("M", dieselEngine.Gender);
            Assert.AreEqual(15, dieselEngine.FuelEfficiency);
            Assert.AreEqual(450, dieselEngine.MaxTorque);
        }
        [TestMethod]
        public void Test_Init_ShouldSetInvalidMaxTorqueFromInput()
        {
            var dieselEngine = new DieselEngine();
            string input = "TestModel\n300\nM\n\n15\n-450\n450\n";
            using (var stringReader = new StringReader(input))
            {
                Console.SetIn(stringReader);
                dieselEngine.Init();
            }
            Assert.AreEqual("TestModel", dieselEngine.Model);
            Assert.AreEqual(300, dieselEngine.Power);
            Assert.AreEqual("M", dieselEngine.Gender);
            Assert.AreEqual(15, dieselEngine.FuelEfficiency);
            Assert.AreEqual(450, dieselEngine.MaxTorque);
        }

        [TestMethod]
        public void Test_RandomInit_ShouldSetRandomValues()
        {
            var dieselEngine = new DieselEngine();
            dieselEngine.RandomInit();

            Assert.IsNotNull(dieselEngine.Model);
            Assert.IsTrue(dieselEngine.Power > 0);
            Assert.IsTrue(dieselEngine.Gender == "M" || dieselEngine.Gender == "W");
            Assert.IsTrue(dieselEngine.FuelEfficiency >= 5 && dieselEngine.FuelEfficiency <= 15);
            Assert.IsTrue(dieselEngine.MaxTorque >= 300 && dieselEngine.MaxTorque <= 700);
        }
        [TestMethod]
        public void Test_Show_ShouldOutputCorrectValues()
        {
            Engine dieselEngine = new DieselEngine("DieselPro", 200, "M", 7.5, 400);
            dieselEngine.Show();
            stringWriter.Flush();
            string consoleOutput = outputStringBuilder.ToString();
            StringAssert.Contains(consoleOutput, "Model: DieselPro");
            StringAssert.Contains(consoleOutput, "Power: 200");
            StringAssert.Contains(consoleOutput, "Gender: M");
            StringAssert.Contains(consoleOutput, "Fuel Efficiency: 7,5");
            StringAssert.Contains(consoleOutput, "Max Torque: 400");
        }
    }
}