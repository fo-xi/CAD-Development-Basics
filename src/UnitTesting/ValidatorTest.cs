using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassesFrame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace UnitTesting
{
    [TestClass]
    public class ValidatorTest
    {
        private const double MinValue = 10;

        private const double MaxValue = 16;

        private const double LensWidth = 50;

        private const double LensFrameWidth = 52;

        [TestCase(TestName = "Положительный тест метода AssertValue")]
        public void TestAssertValue_CorrectValue()
        {
            var value = 12;
            Assert.DoesNotThrow(() =>
            {
                Validator.AssertValue(value, MinValue, MaxValue, "Длина моста");
            }, "Исключение возникнет, если значение не входит в указанный диапазон");
        }

        [TestCase(1, "Исключение возникнет, " +
                     "если значение не входит в диапазон [10, 16]",
            TestName = "Проверка значения 1 принадлежности диапазону [10, 16]")]
        [TestCase(100, "Исключение возникнет, " +
                       "если значение не входит в диапазон [10, 16]",
            TestName = "Проверка значения 100 принадлежности диапазону [10, 16]")]
        public void TestAssertValue_IncorrectValue(double wrongValue, string message)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.AssertValue(wrongValue, MinValue, MaxValue, "Длина моста");
            }, message);
        }

        [TestCase(TestName = "Положительный тест метода IsLensWidth")]
        public void TestIsLensWidth_CorrectValue()
        {
            Assert.DoesNotThrow(() =>
            {
                Validator.IsLensWidth(LensWidth, LensFrameWidth);
            }, "Исключение возникнет, если значение ширины линзы " +
               "больше значения ширины рамы линзы");
        }

        [TestCase(54, "Исключение возникнет, " +
                      "если значение ширины линзы больше значения ширины рамы линзы",
            TestName = "Присвоение значения 54 в качестве значения ширины линзы, " +
                       "что больше значения ширины рамы линзы")]
        public void TestIsLensWidth_IncorrectValue(double wrongValueLensWidth, string message)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Validator.IsLensWidth(wrongValueLensWidth, LensFrameWidth);
            }, message);
        }
    }
}
