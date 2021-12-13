using System;
using GlassesFrame;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace UnitTesting
{
    [TestFixture]
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

        [TestCase(8, "Исключение возникнет, " +
                     "если значение не входит в диапазон [10, 16]",
            TestName = "Присвоение значения 8 в качестве значения длины моста")]
        [TestCase(18,
            "Исключение возникнет, если значение не входит в диапазон [10, 16]",
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
            var expected = 8;
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
            var expected = 8;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
            {
                parameters.EndPieceLength = expected;
            }, "Сеттер EndPieceLength принимает неправильное " +
               "значение длины концевых элементов");
        }

        [TestCase(1, "Исключение возникнет, " +
                     "если значение не входит в диапазон [6, 12]",
            TestName = "Присвоение значения 1 в качестве значения длины концевых элементов")]
        [TestCase(100,
            "Исключение возникнет, если значение не входит в диапазон [6, 12]",
            TestName = "Присвоение значения 100 в качестве значения длины концевых элементов")]
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

        [TestCase(1, "Исключение возникнет, " +
                     "если значение не входит в диапазон [2, 5]",
            TestName = "Присвоение значения 1 в качестве значения ширины оправы")]
        [TestCase(10,
            "Исключение возникнет, если значение не входит в диапазон [2, 5]",
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

        [TestCase(TestName = "Позитивный тест геттера LensFrameRadius")]
        public void TestLensFrameRadiusGet_CorrectValue()
        {
            var expected = 56;
            var parameters = new GlassesFrameParameters()
            {
                LensFrameRadius = expected
            };
            var actual = parameters.LensFrameRadius;
            Assert.AreEqual(expected, actual, "Геттер LensFrameRadius возвращает " +
                                              "неправильное значение радиуса рамы линзы");
        }

        [TestCase(TestName = "Позитивный тест сеттера LensFrameRadius")]
        public void TestLensFrameRadiusSet_CorrectValue()
        {
            var expected = 56;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() => 
            {
                parameters.LensFrameRadius = expected;
            }, "Сеттер LensFrameRadius принимает неправильное " +
                   "значение радиуса рамы линзы");
        }

        [TestCase(46, "Исключение возникнет, " +
                     "если значение не входит в диапазон [52, 58]",
            TestName = "Присвоение значения 46 в качестве значения радиуса рамы линзы")]
        [TestCase(100,
            "Исключение возникнет, если значение не входит в диапазон [52, 58]",
            TestName = "Присвоение значения 100 в качестве значения радиуса рамы линзы")]
        [TestCase(52,
            "Исключение возникнет, если значение радиуса линзы " +
            "больше значения радиуса рамы линзы",
            TestName = "Присвоение значения 52 в качестве значения радиуса рамы линзы, " +
                       "что меньше значения радиуса линзы")]
        public void TestLensFrameRadius_InvalidLensFrameWidth(double wrongLensFrameWidth,
            string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.LensRadius = 54;
                parameters.LensFrameRadius = wrongLensFrameWidth;
            }, message);
        }

        [TestCase(TestName = "Позитивный тест геттера LensRadius")]
        public void TestLensRadiusGet_CorrectValue()
        {
            var expected = 50;
            var parameters = new GlassesFrameParameters()
            {
                LensRadius = expected
            };
            var actual = parameters.LensRadius;
            Assert.AreEqual(expected, actual, "Геттер LensRadius возвращает " +
                                              "неправильное значение радиуса линзы");
        }

        [TestCase(TestName = "Позитивный тест сеттера LensRadius")]
        public void TestLensRadiusSet_CorrectValue()
        {
            var expected = 50;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
            {
                parameters.LensRadius = expected;
            }, "Сеттер LensRadius принимает неправильное " +
                   "значение радиуса линзы");
        }

        [TestCase(36, "Исключение возникнет, " +
                      "если значение не входит в диапазон [48, 54]",
            TestName = "Присвоение значения 36 в качестве значения радиуса линзы")]
        [TestCase(100,
            "Исключение возникнет, если значение не входит в диапазон [48, 54]",
            TestName = "Присвоение значения 100 в качестве значения радиуса линзы")]
        [TestCase(54,
            "Исключение возникнет, если значение радиуса линзы " +
            "больше значения радиуса рамы линзы",
            TestName = "Присвоение значения 54 в качестве значения радиуса линзы, " +
                       "что больше значения радиуса рамы линзы")]
        public void TestLensRadius_InvalidLensWidth(double wrongLensWidth,
            string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.LensFrameRadius = 52;
                parameters.LensRadius = wrongLensWidth;
            }, message);
        }

        [TestCase(TestName = "Позитивный тест геттера LensFrameWidth")]
        public void TestLensFrameWidthGet_CorrectValue()
        {
            var expected = 52;
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
            var expected = 52;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
            {
                parameters.LensFrameWidth = expected;
            }, "Сеттер LensFrameWidth принимает неправильное " +
                   "значение ширины рамы линзы");
        }

        [TestCase(1, "Исключение возникнет, " +
                     "если значение не входит в диапазон [48, 56]",
            TestName = "Присвоение значения 1 в качестве значения ширины рамы линзы")]
        [TestCase(100,
            "Исключение возникнет, если значение не входит в диапазон [48, 56]",
            TestName = "Присвоение значения 100 в качестве значения ширины рамы линзы")]
        public void TestLensFrameWidth_InvalidLensFrameWidth(double wrongLensFrameWidth,
            string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.LensFrameWidth = wrongLensFrameWidth;
            }, message);
        }

        [TestCase(TestName = "Позитивный тест геттера LensFrameHeight")]
        public void TestLensFrameHeighthGet_CorrectValue()
        {
            var expected = 40;
            var parameters = new GlassesFrameParameters()
            {
                LensFrameHeight = expected
            };
            var actual = parameters.LensFrameHeight;
            Assert.AreEqual(expected, actual, "Геттер LensFrameHeight возвращает " +
                                              "неправильное значение высоты рамы линзы");
        }

        [TestCase(TestName = "Позитивный тест сеттера LensFrameHeight")]
        public void TestLensFrameHeightSet_CorrectValue()
        {
            var expected = 40;
            var parameters = new GlassesFrameParameters();
            Assert.DoesNotThrow(() =>
            {
                parameters.LensFrameHeight = expected;
            }, "Сеттер LensFrameHeight принимает неправильное " +
                   "значение высоты рамы линзы");
        }

        [TestCase(1, "Исключение возникнет, " +
             "если значение не входит в диапазон [30, 45]",
            TestName = "Присвоение значения 1 в качестве значения высоты рамы линзы")]
        [TestCase(100,
    "Исключение возникнет, если значение не входит в диапазон [30, 45]",
            TestName = "Присвоение значения 100 в качестве значения высоты рамы линзы")]
        public void TestLensFrameHeight_InvalidLensFrameWidth(double wrongLensFrameHeight,
    string message)
        {
            var parameters = new GlassesFrameParameters();
            Assert.Throws<ArgumentException>(() =>
            {
                parameters.LensFrameHeight = wrongLensFrameHeight;
            }, message);
        }
    }
}
