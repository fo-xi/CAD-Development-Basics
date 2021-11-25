using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlassesFrame;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace UnitTesting
{
    [TestClass]
    public class GlassesFrameParametersTest
    {
        [TestCase(TestName = "Позитивный тест геттера BridgeLength")]
        public void TestBridgeLengthGet_CorrectValue()
        {
            var expected = 12;
            var parameters = new GlassesFrameParameters()
            {
                BridgeLength = expected
            };
            var actual = parameters.BridgeLength;
            Assert.AreEqual(expected, actual, "Геттер BridgeLength возвращает " +
                                              "неправильное значение длины моста");
        }

        [TestCase(TestName = "Позитивный тест сеттера BridgeLength")]
        public void TestBridgeLengthSet_CorrectValue()
        {
            var expected = 12;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
            {
                parameters.BridgeLength = expected;
            }, "Сеттер BridgeLength принимает неправильное значение длины моста");
        }

        [TestCase(8, "Исключение может произойти " +
                     "если число не входит в диапазон [10, 16]",
            TestName = "Присвоение значения 8 в качестве значения длины моста")]
        [TestCase(18,
            "Исключение может произойти если число не входит в диапазон [10, 16]",
            TestName = "Присвоение значения 18 в качестве значения длины моста")]
        public void TestBridgeLength_InvalidBridgeLength(double wrongBridgeLength, string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.BridgeLength = wrongBridgeLength;
            }, message);
        }

        [TestCase(TestName = "Позитивный тест геттера EndPieceLength")]
        public void TestEndPieceLengthGet_CorrectValue()
        {
            var expected = 6;
            var parameters = new GlassesFrameParameters()
            {
                EndPieceLength = expected
            };
            var actual = parameters.EndPieceLength;
            Assert.AreEqual(expected, actual, "Геттер EndPieceLength возвращает " +
                                              "неправильное значение длины концевых элементов");
        }

        [TestCase(TestName = "Позитивный тест сеттера EndPieceLength")]
        public void TestEndPieceLengthSet_CorrectValue()
        {
            var expected = 6;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
            {
                parameters.EndPieceLength = expected;
            }, "Сеттер EndPieceLength принимает неправильное " +
               "значение длины концевых элементов");
        }

        [TestCase(2, "Исключение может произойти " +
                     "если число не входит в диапазон [4, 8]",
            TestName = "Присвоение значения 2 в качестве значения длины концевых элементов")]
        [TestCase(10,
            "Исключение может произойти если число не входит в диапазон [4, 8]",
            TestName = "Присвоение значения 10 в качестве значения длины концевых элементов")]
        public void TestEndPieceLength_InvalidEndPieceLength(double wrongEndPieceLength,
            string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.EndPieceLength = wrongEndPieceLength;
            }, message);
        }

        [TestCase(TestName = "Позитивный тест геттера FrameWidth")]
        public void TestFrameWidthGet_CorrectValue()
        {
            var expected = 4;
            var parameters = new GlassesFrameParameters()
            {
                FrameWidth = expected
            };
            var actual = parameters.FrameWidth;
            Assert.AreEqual(expected, actual, "Геттер FrameWidth возвращает " +
                                              "неправильное значение ширины оправы");
        }

        [TestCase(TestName = "Позитивный тест сеттера FrameWidth")]
        public void TestFrameWidthSet_CorrectValue()
        {
            var expected = 4;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() => 
            {
                parameters.FrameWidth = expected;
            }, "Сеттер FrameWidth принимает неправильное " +
                   "значение ширины оправы");
        }

        [TestCase(1, "Исключение может произойти " +
                     "если число не входит в диапазон [2, 5]",
            TestName = "Присвоение значения 1 в качестве значения ширины оправы")]
        [TestCase(10,
            "Исключение может произойти если число не входит в диапазон [2, 5]",
            TestName = "Присвоение значения 10 в качестве значения ширины оправы")]
        public void TestFrameWidth_InvalidFrameWidth(double wrongFrameWidth,
            string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.FrameWidth = wrongFrameWidth;
            }, message);
        }

        [TestCase(TestName = "Позитивный тест геттера LensFrameWidth")]
        public void TestLensFrameWidthGet_CorrectValue()
        {
            var expected = 56;
            var parameters = new GlassesFrameParameters()
            {
                LensFrameWidth = expected
            };
            var actual = parameters.LensFrameWidth;
            Assert.AreEqual(expected, actual, "Геттер LensFrameWidth возвращает " +
                                              "неправильное значение ширины рамы линзы");
        }

        [TestCase(TestName = "Позитивный тест сеттера LensFrameWidth")]
        public void TestLensFrameWidthSet_CorrectValue()
        {
            var expected = 56;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
                {
                    parameters.LensFrameWidth = expected;
                }, "Сеттер LensFrameWidth принимает неправильное " +
                   "значение ширины рамы линзы");
        }

        [TestCase(46, "Исключение может произойти " +
                     "если число не входит в диапазон [52, 58]",
            TestName = "Присвоение значения 46 в качестве значения ширины рамы линзы")]
        [TestCase(100,
            "Исключение может произойти если число не входит в диапазон [52, 58]",
            TestName = "Присвоение значения 100 в качестве значения ширины рамы линзы")]
        [TestCase(52,
            "Исключение может произойти если значение ширины линзы " +
            "больше значения ширины рамы линзы",
            TestName = "Присвоение значения 52 в качестве значения ширины линзы")]
        public void TestLensFrameWidth_InvalidLensFrameWidth(double wrongLensFrameWidth,
            string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.LensFrameWidth = 54;
                parameters.LensFrameWidth = wrongLensFrameWidth;
            }, message);
        }

        [TestCase(TestName = "Позитивный тест геттера LensWidth")]
        public void TestLensWidthGet_CorrectValue()
        {
            var expected = 50;
            var parameters = new GlassesFrameParameters()
            {
                LensWidth = expected
            };
            var actual = parameters.LensWidth;
            Assert.AreEqual(expected, actual, "Геттер LensWidth возвращает " +
                                              "неправильное значение ширины линзы");
        }

        [TestCase(TestName = "Позитивный тест сеттера LensWidth")]
        public void TestLensWidthSet_CorrectValue()
        {
            var expected = 50;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
                {
                    parameters.LensWidth = expected;
                }, "Сеттер LensWidth принимает неправильное " +
                   "значение ширины линзы");
        }

        [TestCase(36, "Исключение может произойти " +
                      "если число не входит в диапазон [48, 54]",
            TestName = "Присвоение значения 36 в качестве значения ширины линзы")]
        [TestCase(100,
            "Исключение может произойти если число не входит в диапазон [48, 54]",
            TestName = "Присвоение значения 100 в качестве значения ширины линзы")]
        [TestCase(54,
            "Исключение может произойти если значение ширины линзы " +
            "больше значения ширины рамы линзы",
            TestName = "Присвоение значения 54 в качестве значения ширины линзы")]
        public void TestLensWidth_InvalidLensWidth(double wrongLensWidth,
            string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.LensFrameWidth = 52;
                parameters.LensWidth = wrongLensWidth;
            }, message);
        }
    }
}
