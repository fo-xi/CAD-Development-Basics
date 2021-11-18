using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassesFrame;
using Inventor;

namespace InventorApi
{
    public class GlassesFrameBuilder
    {
        private InventorConnector _connector = new InventorConnector();

        private Point2d _centerPointFirstСircle;

        private Point2d _centerPointSecondCircle;

        public void Build(GlassesFrameParameters parameters)
        {
            _connector.CreateNewDocument();
            BuildLensFrame(50, 12, 4);
        }

        private void BuildLensFrame(double diameter, double bridgeLength, double endPieceLength)
        {
            _connector.Sketch = _connector.MakeNewSketch(0);

            var radius = diameter / 2;
            var bridgeWidth = 4;
            var heightLowerPartBridge = 2;

            //Строим первую внешнюю окружность
            _centerPointFirstСircle = _connector.TransientGeometry.CreatePoint2d(0, 0);
            _connector.DrawCircle(_centerPointFirstСircle, radius);

            var xCoordFirstPointTopBridge = 
                FindPointXCircleCenteredOrigin(radius, heightLowerPartBridge + bridgeWidth, 1);
            //Точка начала верхней части моста
            var firstPointTopBridge = 
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge,
                    heightLowerPartBridge + bridgeWidth);

            //Точка конца верхней части  моста
            var secondPointBeginninBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge + bridgeLength,
                    heightLowerPartBridge + bridgeWidth);

            //Строим вторую внешнюю окружность
            var xCoordCenter =
                FindPointX0Circle(secondPointBeginninBridge.X, secondPointBeginninBridge.Y, radius, 1);
            _centerPointSecondCircle =
                _connector.TransientGeometry.CreatePoint2d(xCoordCenter, 0);
            _connector.DrawCircle(_centerPointSecondCircle, radius);

            var xCoordFirstPointBottomBridge =
                FindPointXCircleCenteredOrigin(radius, heightLowerPartBridge, 1);
            //Точка начала нижней части моста
            var firstPointBottomBridge = 
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointBottomBridge,
                    heightLowerPartBridge);

            var xCoordSecondPointBottomBridge =
                FindPointXCircle(xCoordCenter, firstPointBottomBridge.Y, radius, -1);
            //Точка конца нижней части моста
            var secondPointBottomBridge = 
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomBridge,
                    heightLowerPartBridge);

            //Строим верхнюю линию моста
            _connector.DrawLine(firstPointTopBridge, secondPointBeginninBridge);
            //Строим нижнюю линию моста
            _connector.DrawLine(firstPointBottomBridge, secondPointBottomBridge);

            //Строим концевой элемент
            var endPieceWidth = 3;

            //Строим верхнюю линию концевого элемента окружности с началом координат в точке (0, 0)
            var xCoordSecondPointTopEndPiece1 = 
                FindPointXCircleCenteredOrigin(radius, endPieceWidth / 2, -1);
            //Точка конца верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var secondPointTopEndPiece1 = 
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece1,
                    endPieceWidth / 2);

            //Точка начала верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var firstPointTopEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece1 - endPieceLength,
                    endPieceWidth / 2);

            //Строим верхнюю линию концевого элемента
            _connector.DrawLine(firstPointTopEndPiece1, secondPointTopEndPiece1);

            //Строим нижнюю линию концевого элемента окружности с началом координат в точке (0, 0)
            var xCoordSecondPointBottomEndPiece1 =
                FindPointXCircleCenteredOrigin(radius, -(endPieceWidth / 2), -1);
            //Точка конца верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var secondPointBottomEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece1,
                    -(endPieceWidth / 2));

            //Точка начала верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var firstPointBottomEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece1 - endPieceLength,
                    -(endPieceWidth / 2));

            //Строим нижнюю линию концевого элемента
            _connector.DrawLine(firstPointBottomEndPiece1, secondPointBottomEndPiece1);

            //Соединяем линии концевых элементов
            _connector.DrawLine(firstPointTopEndPiece1, firstPointBottomEndPiece1);

            //Строим верхнюю линию концевого элемента второй окружности
            var xCoordSecondPointTopEndPiece2 =
                FindPointXCircle(xCoordCenter, endPieceWidth / 2, radius, 1);

            //Точка начала верхней части концевого элемента второй окружности
            var firstPointTopEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece2,
                    endPieceWidth / 2);

            //Точка конца верхней части концевого элемента второй окружности
            var secondPointTopEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece2 + endPieceLength,
                    endPieceWidth / 2);

            //Строим верхнюю линию концевого элемента
            _connector.DrawLine(firstPointTopEndPiece2, secondPointTopEndPiece2);

            //Строим нижнюю линию концевого элемента второй окружности
            var xCoordSecondPointBottomEndPiece2 =
                FindPointXCircle(xCoordCenter, -(endPieceWidth / 2), radius, 1);

            //Точка начала верхней части концевого элемента второй окружности
            var firstPointBottomEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece2,
                    -(endPieceWidth / 2));

            //Точка конца верхней части концевого элемента второй окружности
            var secondPointBottomEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece2 + endPieceLength,
                    -(endPieceWidth / 2));

            //Строим нижнюю линию концевого элемента
            _connector.DrawLine(firstPointBottomEndPiece2, secondPointBottomEndPiece2);

            //Соединяем линии концевых элементов
            _connector.DrawLine(secondPointTopEndPiece2, secondPointBottomEndPiece2);
        }

        /// <summary>
        /// Поиск координаты х точки на окружности с центром в начале координат.
        /// </summary>
        /// <param name="radius">Радиус окружности.</param>
        /// <param name="y">Коррдината у.</param>
        /// <returns>х координата точки на окружности с центром в начале координат.</returns>
        private double FindPointXCircleCenteredOrigin(double radius, double y, int sing)
        {
            return sing * Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));
        }

        /// <summary>
        /// Поиск х0 координаты точки центра окружности.
        /// </summary>
        /// <param name="x">Коррдината x.</param>
        /// <param name="y">Коррдината у.</param>
        /// <param name="radius">Радиус окружности.</param>
        /// <returns>х0 координата точки центра окружности.</returns>
        private double FindPointX0Circle(double x, double y, double radius, int sign)
        {
            return x + sign * Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));
        }

        /// <summary>
        /// Поиск х координаты точки на окружности.
        /// </summary>
        /// <param name="x0">Коррдината x.</param>
        /// <param name="y">Коррдината у.</param>
        /// <param name="radius">Радиус окружности.</param>
        /// <returns>х координата точки центра окружности.</returns>
        private double FindPointXCircle(double x0, double y, double radius, int sign)
        {
            return x0 + sign * Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));
        }

        private void BuildEndElement()
        {
            //Строим концевой элемент
            var endPieceWidth = 3;

            //Строим верхнюю линию концевого элемента окружности с началом координат в точке (0, 0)
            var xCoordSecondPointTopEndPiece1 =
                FindPointXCircleCenteredOrigin(radius, endPieceWidth / 2, -1);
            //Точка конца верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var secondPointTopEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece1,
                    endPieceWidth / 2);

            //Точка начала верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var firstPointTopEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece1 - endPieceLength,
                    endPieceWidth / 2);

            //Строим верхнюю линию концевого элемента
            _connector.DrawLine(firstPointTopEndPiece1, secondPointTopEndPiece1);

            //Строим нижнюю линию концевого элемента окружности с началом координат в точке (0, 0)
            var xCoordSecondPointBottomEndPiece1 =
                FindPointXCircleCenteredOrigin(radius, -(endPieceWidth / 2), -1);
            //Точка конца верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var secondPointBottomEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece1,
                    -(endPieceWidth / 2));

            //Точка начала верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var firstPointBottomEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece1 - endPieceLength,
                    -(endPieceWidth / 2));

            //Строим нижнюю линию концевого элемента
            _connector.DrawLine(firstPointBottomEndPiece1, secondPointBottomEndPiece1);

            //Соединяем линии концевых элементов
            _connector.DrawLine(firstPointTopEndPiece1, firstPointBottomEndPiece1);

            //Строим верхнюю линию концевого элемента второй окружности
            var xCoordSecondPointTopEndPiece2 =
                FindPointXCircle(xCoordCenter, endPieceWidth / 2, radius, 1);

            //Точка начала верхней части концевого элемента второй окружности
            var firstPointTopEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece2,
                    endPieceWidth / 2);

            //Точка конца верхней части концевого элемента второй окружности
            var secondPointTopEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece2 + endPieceLength,
                    endPieceWidth / 2);

            //Строим верхнюю линию концевого элемента
            _connector.DrawLine(firstPointTopEndPiece2, secondPointTopEndPiece2);

            //Строим нижнюю линию концевого элемента второй окружности
            var xCoordSecondPointBottomEndPiece2 =
                FindPointXCircle(xCoordCenter, -(endPieceWidth / 2), radius, 1);

            //Точка начала верхней части концевого элемента второй окружности
            var firstPointBottomEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece2,
                    -(endPieceWidth / 2));

            //Точка конца верхней части концевого элемента второй окружности
            var secondPointBottomEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece2 + endPieceLength,
                    -(endPieceWidth / 2));

            //Строим нижнюю линию концевого элемента
            _connector.DrawLine(firstPointBottomEndPiece2, secondPointBottomEndPiece2);

            //Соединяем линии концевых элементов
            _connector.DrawLine(secondPointTopEndPiece2, secondPointBottomEndPiece2);
        }

        private void BuildBridge()
        {
            var xCoordFirstPointTopBridge =
                FindPointXCircleCenteredOrigin(radius, heightLowerPartBridge + bridgeWidth, 1);
            //Точка начала верхней части моста
            var firstPointTopBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge,
                    heightLowerPartBridge + bridgeWidth);

            //Точка конца верхней части  моста
            var secondPointBeginninBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge + bridgeLength,
                    heightLowerPartBridge + bridgeWidth);

            //Точка начала нижней части моста
            var firstPointBottomBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointBottomBridge,
                    heightLowerPartBridge);

            var xCoordSecondPointBottomBridge =
                FindPointXCircle(xCoordCenter, firstPointBottomBridge.Y, radius, -1);
            //Точка конца нижней части моста
            var secondPointBottomBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomBridge,
                    heightLowerPartBridge);

            //Строим верхнюю линию моста
            _connector.DrawLine(firstPointTopBridge, secondPointBeginninBridge);
            //Строим нижнюю линию моста
            _connector.DrawLine(firstPointBottomBridge, secondPointBottomBridge);
        }
    }
}
