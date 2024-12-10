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
    public class JetEngineTests
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

            var jetEngine = new JetEngine("JetModel", 1000, "M", 50);

            Assert.AreEqual("JetModel", jetEngine.Model);
            Assert.AreEqual(1000, jetEngine.Power);
            Assert.AreEqual("M", jetEngine.Gender);
            Assert.AreEqual(50, jetEngine.Thrust);
        }
        [TestMethod]
        public void JetEngine_CopyConstructor_ShouldCopyFieldsCorrectly()
        {
            var originalEngine = new JetEngine("Model1", 500, "M", 20000);
            var copiedEngine = new JetEngine(originalEngine);

            Assert.AreEqual(originalEngine.Model, copiedEngine.Model);
            Assert.AreEqual(originalEngine.Power, copiedEngine.Power);
            Assert.AreEqual(originalEngine.Gender, copiedEngine.Gender);
            Assert.AreEqual(originalEngine.Thrust, copiedEngine.Thrust);
        }
        [TestMethod]
        public void Test_Init_ShouldSetInvalidMaxTorqueFromInput()
        {
            var jetEngine = new JetEngine();
            string input = "TestModel\n300\nM\n-100\n100";
            using (var stringReader = new StringReader(input))
            {
                Console.SetIn(stringReader);

                jetEngine.Init();
            }
            Assert.AreEqual("TestModel", jetEngine.Model);
            Assert.AreEqual(300, jetEngine.Power);
            Assert.AreEqual("M", jetEngine.Gender);
            Assert.AreEqual(100, jetEngine.Thrust);
        }
        [TestMethod]
        public void Test_SetValidThrust_ShouldSetThrust()
        {
            var jetEngine = new JetEngine();
            jetEngine.Thrust = 120;

            Assert.AreEqual(120, jetEngine.Thrust);
        }

        [TestMethod]
        public void Test_SetInvalidThrust_ShouldThrowException()
        {
            var jetEngine = new JetEngine();
            stringWriter.Flush();
            jetEngine.Thrust = -10;
            string consoleOutput = outputStringBuilder.ToString().Trim();
            Assert.AreEqual("Ошибка: Заданный аргумент находится вне диапазона допустимых значений.", consoleOutput);
        }

        [TestMethod]
        public void Test_ShallowCopy_ShouldCreateShallowCopy()
        {
            var original = new JetEngine("EcoJet", 800, "W", 60);
            var copy = original.ShallowCopy() as JetEngine;

            Assert.IsNotNull(copy);
            Assert.AreEqual(original.Model, copy.Model);
            Assert.AreEqual(original.Power, copy.Power);
            Assert.AreEqual(original.Gender, copy.Gender);
            Assert.AreEqual(original.Thrust, copy.Thrust);
            Assert.AreNotSame(original, copy);
        }

        [TestMethod]
        public void Test_RandomInit_ShouldSetRandomValues()
        {
            var jetEngine = new JetEngine();
            jetEngine.RandomInit();

            Assert.IsNotNull(jetEngine.Model, "Model was not set.");
            Assert.IsTrue(jetEngine.Power > 0, "Power was not set or is invalid.");
            Assert.IsTrue(jetEngine.Gender == "M" || jetEngine.Gender == "W", "Gender was not set correctly.");
            Assert.IsTrue(jetEngine.Thrust >= 30 && jetEngine.Thrust <= 150, "Thrust was not set within the expected range.");
        }
        [TestMethod]
        public void Test_Show_ShouldPrintAllProperties()
        {
            var jeyingane = new JetEngine("EcoJet", 800, "W", 60);
            jeyingane.Show();
            stringWriter.Flush();
            string consoleOutput = outputStringBuilder.ToString();
            StringAssert.Contains(consoleOutput, "Model: EcoJet");
            StringAssert.Contains(consoleOutput, "Power: 800");
            StringAssert.Contains(consoleOutput, "Gender: W");
            StringAssert.Contains(consoleOutput, "Thrust: 60 kN");
        }

    }
}