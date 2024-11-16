using Microsoft.VisualStudio.TestTools.UnitTesting;
using Лаба9;
using System;


namespace Лаба9.Tests
{
    [TestClass()]
    public class MoneyArrayTests
    {
        [TestMethod()]
        public void Default_constructor()
        {
            MoneyArray a = new MoneyArray();
            Assert.AreEqual(0, a.Size());
        }
        [TestMethod()]
        public void Parameters_Constructor()
        {
            MoneyArray a = new MoneyArray(5);
            Assert.AreEqual(5, a.Size());
        }

        [TestMethod()]
        public void Params_Copy_Constructor()
        {
            MoneyArray original = new MoneyArray(5);
            MoneyArray newArray = new MoneyArray(5, original);
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(newArray[i].Rubles, original[i].Rubles);
                Assert.AreEqual(newArray[i].Kopeks, original[i].Kopeks);
            }
        }

        [TestMethod()]
        public void Params_Copy_Constructor_Arr()
        {
            Money[] original = new Money[3];
            for (int i = 0; i < 3; i++)
            {
                original[i] = new Money(i, i);
            }
            MoneyArray newArray = new MoneyArray(3, original);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(newArray[i].Rubles, original[i].Rubles);
                Assert.AreEqual(newArray[i].Kopeks, original[i].Kopeks);
            }
        }
        [TestMethod()]
        public void Indexer_Get_ValidIndex_ReturnsCorrectValue()
        {
            var moneyArray = new MoneyArray(3);
            moneyArray[1] = new Money(10, 20);
            Assert.AreEqual(20, moneyArray[1].Kopeks);
            Assert.AreEqual(10, moneyArray[1].Rubles);
        }
        
        [TestMethod()]
        public void Indexer_Set_ValidIndex_SetsCorrectValue()
        {
            var moneyArray = new MoneyArray(3);
            moneyArray[1] = new Money(20, 30);
            moneyArray[1] = new Money(50, 20);
            Assert.AreEqual(20, moneyArray[1].Kopeks);
            Assert.AreEqual(50, moneyArray[1].Rubles);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Indexer_ValidIndex_ReturnsError()
        {
            var moneyArray = new MoneyArray(3);
            moneyArray[6] = new Money(10, 20);
        }
        [TestMethod()]
        public void SizeTest()
        {
            var moneyArray = new MoneyArray(3);
            Assert.AreEqual(3, moneyArray.Size());
        }
    }
}