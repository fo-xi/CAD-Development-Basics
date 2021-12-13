using System;
using GlassesFrame;
using Inventor;

namespace InventorApi
{
	/// <summary>
	/// Класс, отвечающий за создание оправы для очков.
	/// </summary>
	public class GlassesFrameBuilder
	{
		/// <summary>
		/// Высота моста.
		/// </summary>
		private const double BridgeWidth = 2;

		/// <summary>
		/// Расстояние между центром и нижней частью моста рамы с круглыми линзами.
		/// </summary>
		private const double HeightLowerPartBridge = 2;

		/// <summary>
		/// Ширина рамы c прямоугольными линзами.
		/// </summary>
		private const double RectangularLensFrameWidth = 3;

		/// <summary>
		/// Объект, содержащий в себе методы Inventor API.
		/// </summary>
		private InventorConnector _connector = new InventorConnector();

		/// <summary>
		/// Вторая точка верхней части моста.
		/// </summary>
		private Point2d _secondPointBeginninBridge;

		/// <summary>
		/// x координата центра окружности второй линзы.
		/// </summary>
		private double _xCoordCenter;

		/// <summary>
		/// Создает оправу для очков с круглыми линзами.
		/// </summary>
		/// <param name="parameters">Параметры оправы для очков.</param>
		public void BuilRoundGlassesFrame(GlassesFrameParameters parameters)
		{
			_connector.CreateNewDocument();
			_connector.Sketch = _connector.MakeNewSketch(0);
			double outerCircleRadius = parameters.LensFrameRadius / 2;
			BuildFirstRoundLensFrame(outerCircleRadius, parameters.LensRadius / 2);
			BuildeBridgeRoundLens(outerCircleRadius, parameters.BridgeLength);
			BuildSecondRoundLensFrame(outerCircleRadius, parameters.LensRadius / 2);
			BuildEndElementRoundLens(outerCircleRadius, parameters.EndPieceLength);
			_connector.Extrude(parameters.FrameWidth);
			_connector.FilletsRoundLens();
		}

		/// <summary>
		/// Создает оправу для очков с прямоугольными линзами.
		/// </summary>
		/// <param name="parameters">Параметры оправы для очков.</param>
		public void BuilRectangularGlassesFrame(GlassesFrameParameters parameters)
		{
			_connector.CreateNewDocument();
			_connector.Sketch = _connector.MakeNewSketch(0);
			BuildFirstRectangularLensFrame(parameters.LensFrameHeight, parameters.LensFrameWidth);
			BuildeBridgeRectangularLens(parameters.BridgeLength, parameters.LensFrameHeight,
				parameters.LensFrameWidth);
			BuildSecondRectangularLensFrame(parameters.LensFrameHeight, parameters.LensFrameWidth,
				parameters.BridgeLength);
			BuildEndElementRectangularLens(parameters.LensFrameHeight, parameters.LensFrameWidth,
				parameters.BridgeLength, parameters.EndPieceLength);
			_connector.Extrude(parameters.FrameWidth);
			_connector.FilletsRectangularLens();
		}


		/// <summary>
		/// Строит раму первой круглой линзы.
		/// </summary>
		/// <param name="outerCircleRadius">Радиус внешней окружности.</param>
		/// <param name="innerCircleRadius">Радиус внутренней окружности.</param>
		private void BuildFirstRoundLensFrame(double outerCircleRadius, double innerCircleRadius)
		{
			var centerPoint = 
				_connector.TransientGeometry.CreatePoint2d(0, 0);

			//Строим внешнюю окружность 
			_connector.DrawCircle(centerPoint, outerCircleRadius);

			//Строим внутреннюю окружность
			_connector.DrawCircle(centerPoint, innerCircleRadius);
		}

		/// <summary>
		/// Строит раму второй линзы.
		/// </summary>
		/// <param name="outerCircleRadius">Радиус внешней окружности.</param>
		/// <param name="innerCircleRadius">Радиус внутренней окружности.</param>
		private void BuildSecondRoundLensFrame(double outerCircleRadius, double innerCircleRadius)
		{
			//Строим внешнюю окружность
			_xCoordCenter =
				FindPointX0Circle(_secondPointBeginninBridge.X,
					_secondPointBeginninBridge.Y, outerCircleRadius, 1);

			var centerPoint =
				_connector.TransientGeometry.CreatePoint2d(_xCoordCenter, 0);

			//Строим внешнюю окружность
			_connector.DrawCircle(centerPoint, outerCircleRadius);

			//Строим внутреннюю окружность
			_connector.DrawCircle(centerPoint, innerCircleRadius);
		}

		/// <summary>
		/// Строит мост.
		/// </summary>
		/// <param name="radius">Радиус внешней окружности.</param>
		/// <param name="bridgeLength">Длина моста.</param>
		private void BuildeBridgeRoundLens(double radius, double bridgeLength)
		{
			var xCoordFirstPointTopBridge =
				FindPointXCircleCenteredOrigin(radius, 
					HeightLowerPartBridge + BridgeWidth, 1);
			//Точка начала верхней части моста
			var firstPointTopBridge =
				_connector.TransientGeometry.CreatePoint2d(xCoordFirstPointTopBridge,
					HeightLowerPartBridge + BridgeWidth);

			//Точка конца верхней части моста
			_secondPointBeginninBridge =
				_connector.TransientGeometry.CreatePoint2d
				(xCoordFirstPointTopBridge + bridgeLength, 
					HeightLowerPartBridge + BridgeWidth);

			var secondPointBottomBridge =
				_connector.TransientGeometry.CreatePoint2d
				(xCoordFirstPointTopBridge + bridgeLength,
					HeightLowerPartBridge);

			//Строим верхнюю линию моста
			_connector.DrawRectangle(firstPointTopBridge,
				secondPointBottomBridge);
		}

		/// <summary>
		/// Строит концевые элементы.
		/// </summary>
		/// <param name="radius">Радиус внешней окружности.</param>
		/// <param name="endPieceLength">Длина концевых элементов.</param>
		private void BuildEndElementRoundLens(double radius, double endPieceLength)
		{
			//Ширина концевого элемента
			var endPieceWidth = 4;

			var xCoordSecondPointTopEndPiece1 =
				FindPointXCircleCenteredOrigin(radius, endPieceWidth / 2, -1);

			//Точка начала верхней части концевого элемента второй окружности
			var firstPointTopEndPiece1 =
				_connector.TransientGeometry.CreatePoint2d
				(xCoordSecondPointTopEndPiece1 - endPieceLength, endPieceWidth / 2);

			var xCoordSecondPointBottomEndPiece1 =
				FindPointXCircleCenteredOrigin(radius, -(endPieceWidth / 2), -1);
			//Точка конца нижней части концевого элемента окружности
			//с началом координат в точке (0, 0)
			var secondPointBottomEndPiece1 =
				_connector.TransientGeometry.CreatePoint2d(xCoordSecondPointBottomEndPiece1,
					-(endPieceWidth / 2));

			_connector.DrawRectangle(firstPointTopEndPiece1,
				secondPointBottomEndPiece1);

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
				_connector.TransientGeometry.CreatePoint2d
				(xCoordSecondPointBottomEndPiece2 + endPieceLength,
					-(endPieceWidth / 2));

			_connector.DrawRectangle(firstPointTopEndPiece2,
				secondPointBottomEndPiece2);
		}

		/// <summary>
		/// Поиск координаты х точки на окружности с центром в начале координат.
		/// </summary>
		/// <param name="radius">Радиус окружности.</param>
		/// <param name="y">Координата у.</param>
		/// <param name="sign">Знак.</param>
		/// <returns>х координата точки на окружности с центром в начале координат.</returns>
		private double FindPointXCircleCenteredOrigin(double radius, double y, int sign)
		{
			return sign * Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));
		}

		/// <summary>
		/// Поиск х0 координаты точки центра окружности.
		/// </summary>
		/// <param name="x">Координата x.</param>
		/// <param name="y">Координата у.</param>
		/// <param name="radius">Радиус окружности.</param>
		/// <param name="sign">Знак.</param>
		/// <returns>х0 координата точки центра окружности.</returns>
		private double FindPointX0Circle(double x, double y, double radius, int sign)
		{
			return x + sign * Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));
		}

		/// <summary>
		/// Поиск х координаты точки на окружности.
		/// </summary>
		/// <param name="x0">Координата x.</param>
		/// <param name="y">Координата у.</param>
		/// <param name="radius">Радиус окружности.</param>
		/// <param name="sign">Знак.</param>
		/// <returns>х координата точки центра окружности.</returns>
		private double FindPointXCircle(double x0, double y, double radius, int sign)
		{
			return x0 + sign * Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(y, 2));
		}

		/// <summary>
		/// Строит раму первой прямоугольной линзы.
		/// </summary>
		/// <param name="height">Высота рамы.</param>
		/// <param name="width">Ширина рамы.</param>
		private void BuildFirstRectangularLensFrame(double height, double width)
		{
			var firstPointOuterRectangle =
				_connector.TransientGeometry.CreatePoint2d(0, 0);

			var secondPointOuterRectangle = 
				_connector.TransientGeometry.CreatePoint2d(width, height);

			var firstPointInnerRectangle = 
				_connector.TransientGeometry.CreatePoint2d
				(RectangularLensFrameWidth, RectangularLensFrameWidth);

			var secondPointInnerRectangle = 
				_connector.TransientGeometry.CreatePoint2d
				(width - RectangularLensFrameWidth, height - RectangularLensFrameWidth);
			//Строим внешний прямоугольник 
			_connector.DrawRectangle(firstPointOuterRectangle, secondPointOuterRectangle);

			//Строим внутренний прямоугольник
			_connector.DrawRectangle(firstPointInnerRectangle, secondPointInnerRectangle);
		}

		/// <summary>
		/// Строит раму второй прямоугольной линзы.
		/// </summary>
		/// <param name="height">Высота рамы.</param>
		/// <param name="width">Ширина рамы.</param>
		/// <param name="bridgeLength">Длина моста.</param>
		private void BuildSecondRectangularLensFrame(double height, 
			double width, double bridgeLength)
		{
			var distanceSecondLens = width + bridgeLength;

			var firstPointOuterRectangle =
				_connector.TransientGeometry.CreatePoint2d(distanceSecondLens, 0);

			var secondPointOuterRectangle =
				_connector.TransientGeometry.CreatePoint2d((width * 2) + bridgeLength, height);

			var firstPointInnerRectangle =
				_connector.TransientGeometry.CreatePoint2d
				(distanceSecondLens + RectangularLensFrameWidth, RectangularLensFrameWidth);

			var secondPointInnerRectangle =
				_connector.TransientGeometry.CreatePoint2d
				(distanceSecondLens + (width - RectangularLensFrameWidth),
				height - RectangularLensFrameWidth);
			//Строим внешний прямоугольник 
			_connector.DrawRectangle(firstPointOuterRectangle, secondPointOuterRectangle);

			//Строим внутренний прямоугольник
			_connector.DrawRectangle(firstPointInnerRectangle, secondPointInnerRectangle);
		}

		/// <summary>
		/// Строит мост.
		/// </summary>
		/// <param name="bridgeLength">Длина моста.</param>
		/// <param name="height">Высота рамы линзы.</param>
		/// <param name="width">Ширина рамы линзы.</param>
		private void BuildeBridgeRectangularLens(double bridgeLength,
			double height, double width)
		{
			//Расстояние между центром и нижней частью моста
			var heightLowerPartBridge = 5;

			//Точка начала моста
			var firstPointTopBridge =
				_connector.TransientGeometry.CreatePoint2d(width,
					(height / 2) + heightLowerPartBridge);

			//Точка конца моста
			var secondPointBottomBridge =
				_connector.TransientGeometry.CreatePoint2d(width + bridgeLength,
				(height / 2) + heightLowerPartBridge + BridgeWidth);

			//Строим верхнюю линию моста
			_connector.DrawRectangle(firstPointTopBridge, secondPointBottomBridge);
		}

		/// <summary>
		/// Строит концевые элементы.
		/// </summary>
		/// <param name="height">Высота рамы.</param>
		/// <param name="width">Ширина рамы.</param>
		/// <param name="bridgeLength">Длина моста.</param>
		/// <param name="endPieceLength">Длина концевых элементов.</param>
		private void BuildEndElementRectangularLens(double height, double width,
			double bridgeLength, double endPieceLength)
		{
			//Ширина концевого элемента
			var endPieceWidth = 3.5;

			//Расстояние до второго концевого элемента
			var distanceSecondEndElement1 = (width * 2) + bridgeLength;

			//Вторая точка концевого элемента первой рамы линзы
			var secondPointEndElement1 = 
				_connector.TransientGeometry.CreatePoint2d(0, (height / 2) + (endPieceWidth / 2));

			//Первая точка концевого элемента первой рамы линзы
			var firstPointEndElement1 = 
				_connector.TransientGeometry.CreatePoint2d(-endPieceLength,
				(height / 2) - (endPieceWidth / 2));

			//Первая точка концевого элемента второй рамы линзы
			var firstPointEndElement2 =
				_connector.TransientGeometry.CreatePoint2d(distanceSecondEndElement1,
				(height / 2) - (endPieceWidth / 2));

			//Вторая точка концевого элемента второй рамы линзы
			var secondPointEndElement2 =
				_connector.TransientGeometry.CreatePoint2d
				(distanceSecondEndElement1 + endPieceLength,
				(height / 2) + (endPieceWidth / 2));

			//Строим концевой элемент первой рамы линзы
			_connector.DrawRectangle(secondPointEndElement1, firstPointEndElement1);

			//Строим концевой элемент второй рамы линзы
			_connector.DrawRectangle(firstPointEndElement2, secondPointEndElement2);
		}
	}
}
