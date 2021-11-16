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

        private void BuildLensFrame(double diameter, double bridgeLength)
        {
            var bridgeWidth = 4;
            var heightLowerPartBridge = 2;

            //Строим первую внешнюю окружность
            _centerPointFirstСircle = _connector.TransientGeometry.CreatePoint2d();
            var radius = diameter / 2;
            _connector.DrawCircle(_centerPointFirstСircle, diameter);

            var xCoordOneBeginningBridge = FindPointXCircleCenteredOrigin(radius, heightLowerPartBridge + bridgeWidth);
            var xCoordOTwoBeginningBridge = FindPointXCircleCenteredOrigin(radius, heightLowerPartBridge);

            //Точка верхней части начала моста
            var pointOneBeginningBridge = _connector.TransientGeometry.CreatePoint2d(xCoordOneBeginningBridge, heightLowerPartBridge + bridgeWidth);
            //Точка верхней части начала моста
            var pointTwoBeginningBridge = _connector.TransientGeometry.CreatePoint2d(xCoordOTwoBeginningBridge, heightLowerPartBridge);

            var xCoordOneEndBridge = FindPointXCircleCenteredOrigin(radius,xCoordOneBeginningBridge + bridgeLength);
            var xCoordTwoEndBridge = FindPointXCircleCenteredOrigin(radius,xCoordOneBeginningBridge + bridgeLength);

            //Точка верхней части конца моста
            var pointOneEndBridge = _connector.TransientGeometry.CreatePoint2d(xCoordOneEndBridge, heightLowerPartBridge + bridgeWidth);
            //Точка нижней части конца моста
            var pointTwoEndBridge = _connector.TransientGeometry.CreatePoint2d(xCoordTwoEndBridge, heightLowerPartBridge + bridgeWidth);

            //Строим верхнюю линию моста
            _connector.DrawLine(pointOneBeginningBridge, pointOneEndBridge);
            //Строим нижнюю линию моста
            _connector.DrawLine(pointTwoBeginningBridge, pointTwoEndBridge);

            //Строим вторую внешнюю окружность
            var xCoordCenter = FindPointXCircle(pointOneEndBridge.X, pointOneEndBridge.Y, radius);
            _centerPointSecondCircle = _connector.TransientGeometry.CreatePoint2d(xCoordCenter, 0);
            _connector.DrawCircle(_centerPointSecondCircle, diameter);
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
