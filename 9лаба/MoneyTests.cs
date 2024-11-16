using Microsoft.VisualStudio.TestTools.UnitTesting;
using Лаба9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Лаба9.Tests
{
    [TestClass()]
    public class MoneyTests
    {
        [TestMethod()]
        public void Set_Positive_value_Rubles()
        {
            Money money = new Money();
            money.Rubles = 5;
            Assert.AreEqual(money.Rubles, 5);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Set_Negative_value_Rubles()
        {
            Money money = new Money();
            money.Rubles = -5;
        }
        [TestMethod()]
        public void Set_Positive_value_Kopeks()
        {
            Money money = new Money();
            money.Kopeks = 5;
            Assert.AreEqual(money.Kopeks, 5);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Set_Negative_value_Kopeks()
        {
            Money money = new Money();
            money.Kopeks = -5;
        }
        [TestMethod()]
        public void Set_BigValue_Kopeks()
        {
            Money money = new Money();
            money.Kopeks = 102;
            Assert.AreEqual(money.Kopeks, 2);
            Assert.AreEqual(money.Rubles, 1);
        }
        [TestMethod()]
        public void Set_Positive_value_ObjectCount()
        {
            int s1 = Money.ObjectCount;
            Money.ObjectCount = 1;
            int s2 = Money.ObjectCount;
            Assert.AreEqual(s1+1,s2);
        }
        [TestMethod()]
        public void Set_Negative_value_ObjectCount()
        {
            int s1 = Money.ObjectCount;
            Money.ObjectCount = -1;
            int s2 = Money.ObjectCount;
            Assert.AreEqual(s1-1,s2);
        }
        [TestMethod()]
        public void Default_constructor()
        {
            Money money = new Money();
            Assert.AreEqual(money.Kopeks, 0);
            Assert.AreEqual(money.Rubles, 0);
        }

        [TestMethod()]
        public void Parameters_Constructor()
        {
            Money money = new Money(1, 2);
            Assert.AreEqual(money.Kopeks, 2);
            Assert.AreEqual(money.Rubles, 1);
        }
        [TestMethod()]
        public void Get_ObjectCount()
        {
            int s1 = Money.ObjectCount;
            Money money1 = new Money();
            Money money2 = new Money();
            int s2 = Money.ObjectCount;
            Assert.AreEqual(s1+2,s2);
        }
        [TestMethod()]
        public void Zeroing_Values()
        {
            Money money = new Money(12, 123);
            money.Reset();
            Assert.AreEqual(money.Rubles, 0);
            Assert.AreEqual(money.Kopeks, 0);
        }
        [TestMethod()]
        public void Operator_Less_Test()
        {
            Money money1 = new Money(1, 2);
            Money money2 = new Money(2, 1);
            Assert.AreEqual(money1 < money2, true);
        }
        public void Operator_More_Test()
        {
            Money money1 = new Money(1, 2);
            Money money2 = new Money(2, 1);
            Assert.AreEqual(money1 > money2, false);
        }
        [TestMethod()]
        public void Operator_UnarMinus_Test()
        {
            Money money1 = new Money(2, 2);
            money1--;
            Assert.AreEqual(1,money1.Kopeks);
        }
        [TestMethod()]
        public void Operator_UnarPlus_Test()
        {
            Money money1 = new Money(2, 2);            
            money1++;
            Assert.AreEqual(money1.Rubles, 2);
            Assert.AreEqual(money1.Kopeks, 3);
        }
        [TestMethod()]
        public void Operator_UnarPlus_Limit_Test()
        { 
            Money money1 = new Money(2, 99);
            money1++;
            Assert.AreEqual(money1.Rubles, 3);
            Assert.AreEqual(money1.Kopeks, 0);
        }
        [TestMethod()]
        public void Operator_Implicit_Test()
        {
            Money money = new Money(2, 2);
            int rubles = money;
            Assert.AreEqual(rubles, 2);
        }
        [TestMethod()]
        public void Operator_Explicit_Test()
        {
            Money money = new Money(2, 2);
            Assert.AreEqual(money.Rubles, 2);
        }
        [TestMethod()]
        public void Operator_MinusLeft_Test()
        {
            Money money = new Money(2, 99);
            money = money - 75;
            Assert.AreEqual(money.Kopeks, 24);
        }
        [TestMethod()]
        public void Operator_MinusRight_Test()
        {
            Money money = new Money(2, 99);
            int r = 1000 - money;
            Assert.AreEqual(r, 701);
        }
    }
}