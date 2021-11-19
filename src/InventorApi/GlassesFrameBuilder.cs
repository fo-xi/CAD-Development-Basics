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
        private const double BridgeWidth = 2;

        private const double HeightLowerPartBridge = 2;

        private InventorConnector _connector = new InventorConnector();

        private Point2d _secondPointBeginninBridge;

        private double _xCoordCenter;

        public void Build(GlassesFrameParameters parameters)
        {
            _connector.CreateNewDocument();
            _connector.Sketch = _connector.MakeNewSketch(0);
            double outerCircleRadius = 56 / 2;
            BuildFirstLensFrame(outerCircleRadius, 50 / 2);
            BuildTopLineBridge(outerCircleRadius, 12);
            BuildSecondLensFrame(outerCircleRadius, 50 / 2);
            BuildEndElement(outerCircleRadius, 10);
            _connector.Extrude(3);
            _connector.Fillet();
        }

        private void BuildFirstLensFrame(double outerCircleRadius, double innerCircleRadius)
        {
            var _centerPoint = _connector.TransientGeometry.CreatePoint2d(0, 0);

            //Строим внешнюю окружность 
            _connector.DrawCircle(_centerPoint, outerCircleRadius);

            //Строим внутреннюю окружность
            _connector.DrawCircle(_centerPoint, innerCircleRadius);
        }

        private void BuildSecondLensFrame(double outerCircleRadius, double innerCircleRadius)
        {
            //Строим внешнюю окружность
            _xCoordCenter =
                FindPointX0Circle(_secondPointBeginninBridge.X, _secondPointBeginninBridge.Y, outerCircleRadius, 1);
            var _centerPoint =
                _connector.TransientGeometry.CreatePoint2d(_xCoordCenter, 0);

            //Строим внешнюю окружность
            _connector.DrawCircle(_centerPoint, outerCircleRadius);

            //Строим внутреннюю окружность
            _connector.DrawCircle(_centerPoint, innerCircleRadius);
        }

        private void BuildTopLineBridge(double radius, double bridgeLength)
        {
            var xCoordFirstPointTopBridge =
                FindPointXCircleCenteredOrigin(radius, HeightLowerPartBridge + BridgeWidth, 1);
            //Точка начала верхней части моста
            var firstPointTopBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge,
                    HeightLowerPartBridge + BridgeWidth);

            //Точка конца верхней части моста
            _secondPointBeginninBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge + bridgeLength,
                    HeightLowerPartBridge + BridgeWidth);

            var secondPointBottomBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge + bridgeLength,
                    HeightLowerPartBridge);

            //Строим верхнюю линию моста
            _connector.DrawRectangle(firstPointTopBridge, secondPointBottomBridge);
        }

        private void BuildEndElement(double radius, double endPieceLength)
        {
            //Строим концевой элемент
            var endPieceWidth = 4;

            var xCoordSecondPointTopEndPiece1 =
                FindPointXCircleCenteredOrigin(radius, endPieceWidth / 2, -1);

            //Точка начала верхней части концевого элемента второй окружности
            var firstPointTopEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece1 - endPieceLength,
                    endPieceWidth / 2);

            var xCoordSecondPointBottomEndPiece1 =
                FindPointXCircleCenteredOrigin(radius, -(endPieceWidth / 2), -1);
            //Точка конца нижней части концевого элемента окружности с началом координат в точке (0, 0)
            var secondPointBottomEndPiece1 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece1,
                    -(endPieceWidth / 2));

            _connector.DrawRectangle(firstPointTopEndPiece1, secondPointBottomEndPiece1);

            var xCoordSecondPointTopEndPiece2 =
                FindPointXCircle(_xCoordCenter, endPieceWidth / 2, radius, 1);

            //Точка начала верхней части концевого элемента второй окружности
            var firstPointTopEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopEndPiece2,
                    endPieceWidth / 2);

            var xCoordSecondPointBottomEndPiece2 =
                FindPointXCircle(_xCoordCenter, -(endPieceWidth / 2), radius, 1);

            //Точка конца нижней части концевого элемента второй окружности
            var secondPointBottomEndPiece2 =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece2 + endPieceLength,
                    -(endPieceWidth / 2));

            _connector.DrawRectangle(firstPointTopEndPiece2, secondPointBottomEndPiece2);
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
    }
}
