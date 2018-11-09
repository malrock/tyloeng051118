using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using calculator;

namespace calculator_test
{
    [TestClass]
    public class BasicCalculatorTests
    {
        [TestMethod]
        public void TestAddition()
        {
            var a = 2.0f;
            var b = 1.5f;
            var res = a+b;
            Assert.AreEqual(BasicCalculator.Add(a,b),res);
        }
        [TestMethod]
        public void TestSubtraction()
        {
            var a = 5.0f;
            var b = 1.5f;
            var res = a - b;
            Assert.AreEqual(BasicCalculator.Subtract(a, b), res);
        }
        [TestMethod]
        public void TestMultiplication()
        {
            var a = 5.0f;
            var b = 5.5f;
            var res = a * b;
            Assert.AreEqual(BasicCalculator.Multiply(a, b), res);
        }
        [TestMethod]
        public void TestDivision()
        {
            var a = 5.0f;
            var b = 2.0f;
            var res = a / b;
            Assert.AreEqual(BasicCalculator.Divide(a, b), res);
        }
    }
}
