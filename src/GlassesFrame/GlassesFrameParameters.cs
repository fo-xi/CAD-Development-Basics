using GalaSoft.MvvmLight;

namespace GlassesFrame
{
	/// <summary>
	/// Класс параметров.
	/// </summary>
	public class GlassesFrameParameters : ViewModelBase
	{
		/// <summary>
		/// Длина моста.
		/// </summary>
		private double _bridgeLength;

		/// <summary>
		/// Длина концевого элемента.
		/// </summary>
		private double _endPieceLength;

		/// <summary>
		/// Ширина оправы.
		/// </summary>
		private double _frameWidth;

		/// <summary>
		/// Радиус рамы линзы.
		/// </summary>
		private double _lensFrameRadius;

		/// <summary>
		/// Радиус линзы.
		/// </summary>
		private double _lensRadius;

		/// <summary>
		/// Ширина рамы линзы.
		/// </summary>
		private double _lensFrameWidth;

		/// <summary>
		/// Высота рамы линзы.
		/// </summary>
		private double _lensFrameHeight;

		/// <summary>
		/// Возвращает и задает длину моста.
		/// </summary>
		public double BridgeLength
		{
			get => _bridgeLength;
            set
			{
				Validator.AssertValue(value, 10, 16, "Длина моста");
				_bridgeLength = value;
			}
		}

		/// <summary>
		/// Вовзращает и задает длину концевого элемента.
		/// </summary>
		public double EndPieceLength
		{
			get => _endPieceLength;
			set
			{
				Validator.AssertValue(value, 6, 12,
					"Длина концевого элемента");
				_endPieceLength = value;
			}
		}

		/// <summary>
		/// Возвращает и задает ширину оправы.
		/// </summary>
		public double FrameWidth
		{
			get => _frameWidth;
			set
			{
				Validator.AssertValue(value, 2, 5, "Ширина оправы");
				_frameWidth = value;
			}
		}

		/// <summary>
		/// Возвращает и задает радиус рамы линзы.
		/// </summary>
		public double LensFrameRadius
		{
			get => _lensFrameRadius;
			set
			{
				Validator.AssertValue(value, 52, 58, "Радиус рамы линзы");
				Validator.IsLensWidth(LensRadius, value);
				_lensFrameRadius = value;
			}
		}

		/// <summary>
		/// Возвращает и задает радиус линзы.
		/// </summary>
		public double LensRadius
		{
			get => _lensRadius;
			set
			{
				Validator.AssertValue(value, 48, 54, "Радиус линзы");
				Validator.IsLensWidth(value, LensFrameRadius);
                _lensRadius = value;
			}
		}

		/// <summary>
		/// Возвращает и задает ширину рамы линзы.
		/// </summary>
		public double LensFrameWidth
		{
			get => _lensFrameWidth;
			set
			{
				Validator.AssertValue(value, 48, 56, "Ширина рамы линзы");
				_lensFrameWidth = value;
			}
		}

		/// <summary>
		/// Возвращает и задает высоту рамы линзы.
		/// </summary>
		public double LensFrameHeight
		{
			get => _lensFrameHeight;
			set
			{
				Validator.AssertValue(value, 30, 45, "Высота рамы линзы");
				_lensFrameHeight = value;
			}
		}

		/// <summary>
		/// Задает параметры детали.
		/// </summary>
		public GlassesFrameParameters()
		{
			BridgeLength = 12;
			EndPieceLength = 8;
			FrameWidth = 3;
			LensFrameRadius = 56;
			LensRadius = 54;
			LensFrameWidth = 52;
			LensFrameHeight = 35;
		}
	}
}
