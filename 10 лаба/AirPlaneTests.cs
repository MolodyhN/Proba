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
    public class AirPlaneTests
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
        public void Test_SetValidModel_ShouldSetModel()
        {
            var airplane = new AirPlane();
            string expectedModel = "Boeing 737";
            airplane.Model = expectedModel;

            Assert.AreEqual(expectedModel, airplane.Model);
        }
        [TestMethod]
        public void Test_SetInit()
        {
            var airplane = new AirPlane();
            string input = "TestBrand\nTestModel\n";
            using (var inputReader = new StringReader(input))
            {
                Console.SetIn(inputReader);
                airplane.Init();
                Assert.AreEqual("TestBrand", airplane.Brand);
                Assert.AreEqual("TestModel", airplane.Model);
            }
        }

        [TestMethod]
        public void Test_SetInvalidModel_ShouldThrowException()
        {
            var airplane = new AirPlane();
            stringWriter.Flush();
            airplane.Model = "";
            string consoleOutput = outputStringBuilder.ToString().Trim();
            Assert.AreEqual("Ошибка: Заданный аргумент находится вне диапазона допустимых значений.", consoleOutput);
        }

        [TestMethod]
        public void Test_SetValidBrand_ShouldSetBrand()
        {
            var airplane = new AirPlane();
            string expectedBrand = "Airbus";
            airplane.Brand = expectedBrand;

            Assert.AreEqual(expectedBrand, airplane.Brand);
        }

        [TestMethod]
        public void Test_SetInvalidBrand_ShouldThrowException()
        {
            var airplane = new AirPlane();
            stringWriter.Flush();
            airplane.Brand = "";
            string consoleOutput = outputStringBuilder.ToString().Trim();
            Assert.AreEqual("Ошибка: Заданный аргумент находится вне диапазона допустимых значений.", consoleOutput);

        }

        [TestMethod]
        public void Test_RandomInit_ShouldSetRandomValues()
        {
            var airplane = new AirPlane();

            airplane.RandomInit();
            Assert.IsNotNull(airplane.Brand);
            Assert.IsNotNull(airplane.Model);
            Assert.IsTrue(airplane.Brand.StartsWith("Brand"));
            Assert.IsTrue(airplane.Model.StartsWith("Model"));
        }
        [TestMethod]
        public void Test_Show_ShouldOutputCorrectValues()
        {
            var airplane = new AirPlane();
            airplane.Brand ="TestBrand" ;
            airplane.Model = "TestModel";
            airplane.Show();
            stringWriter.Flush();
            string consoleOutput = outputStringBuilder.ToString();
            StringAssert.Contains(consoleOutput, "Model: TestModel");
            StringAssert.Contains(consoleOutput, "Brand: TestBrand");
        }
    }
}