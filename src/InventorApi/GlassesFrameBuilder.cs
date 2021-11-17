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
        private InventorConnector _connector;

        private Point2d _centerPointFirstСircle;

        private Point2d _centerPointSecondCircle;

        public void Build(GlassesFrameParameters parameters)
        {

        }

        private void BuildLensFrame(double diameter, double bridgeLength, double endPieceLength)
        {
            var bridgeWidth = 4;
            var heightLowerPartBridge = 2;

            //Строим первую внешнюю окружность
            _centerPointFirstСircle = _connector.TransientGeometry.CreatePoint2d();
            var radius = diameter / 2;
            _connector.DrawCircle(_centerPointFirstСircle, diameter);

            var xCoordFirstPointBeginningBridge = 
                FindPointXCircleCenteredOrigin(radius, heightLowerPartBridge + bridgeWidth);
            //Точка начала верхней части моста
            var firstPointBeginningBridge = 
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointBeginningBridge,
                    heightLowerPartBridge + bridgeWidth);

            var xCoordSecondPointBeginningBridge =
                FindPointXCircleCenteredOrigin(radius, xCoordFirstPointBeginningBridge + bridgeLength);
            //Точка конца верхней части  моста
            var secondPointBeginningBridge =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBeginningBridge,
                    heightLowerPartBridge + bridgeWidth);


            var xCoordFirstPointEndBridge =
                FindPointXCircleCenteredOrigin(radius, heightLowerPartBridge);
            //Точка начала нижней части моста
            var firstPointEndBridge = 
                _connector.TransientGeometry.CreatePoint2d(xCoordFirstPointEndBridge,
                    heightLowerPartBridge);

            
            var xCoordSecondPointEndBridge = 
                FindPointXCircleCenteredOrigin(radius, xCoordFirstPointBeginningBridge + bridgeLength);
            //Точка конца нижней части моста
            var secondPointEndBridge = 
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointEndBridge,
                    heightLowerPartBridge + bridgeWidth);

            //Строим верхнюю линию моста
            _connector.DrawLine(firstPointBeginningBridge, secondPointBeginningBridge);
            //Строим нижнюю линию моста
            _connector.DrawLine(firstPointEndBridge, secondPointEndBridge);

            //Строим вторую внешнюю окружность
            var xCoordCenter = 
                FindPointXCircle(secondPointBeginningBridge.X, secondPointEndBridge.Y, radius);
            _centerPointSecondCircle = 
                _connector.TransientGeometry.CreatePoint2d(xCoordCenter, 0);
            _connector.DrawCircle(_centerPointSecondCircle, diameter);

            //Строим концевой элемент
            var endPieceWidth = 3;
            var xCoordSecondPointTopElement = 
                FindPointXCircleCenteredOrigin(radius, endPieceWidth / 2);
            //Точка конца верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var secondPointTopElement = 
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopElement,
                    endPieceWidth / 2);

            //Точка начала верхней части концевого элемента окружности с началом координат в точке (0, 0)
            var firstPointTopElement =
                _connector.TransientGeometry.CreatePoint2d(xCoordSecondPointTopElement - endPieceLength,
                    endPieceWidth / 2);


            //Строим верхнюю линию концевого элемента
            _connector.DrawLine(firstPointTopElement, secondPointTopElement);
        }

        /// <summary>
        /// Поиск координаты х точки на окружности с центром в начале координат.
        /// </summary>
        /// <param name="radius">Радиус окружности.</param>
        /// <param name="y">Коррдината у.</param>
        /// <returns>х координата точки на окружности с центром в начале координат.</returns>
        private double FindPointXCircleCenteredOrigin(double radius, double y)
        {
            return (Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2)));
        }

        /// <summary>
        /// Поиск х координаты точки центра окружности.
        /// </summary>
        /// <param name="x">Коррдината x.</param>
        /// <param name="y">Коррдината у.</param>
        /// <param name="radius">Радиус окружности.</param>
        /// <returns>х координата точки центра окружности.</returns>
        private double FindPointXCircle(double x, double y, double radius)
        {
            return (x - (Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2))));
        }

        private void BuildEndElement()
        {
           
        }

        private void BuildBridge()
        {

        }
    }
}
