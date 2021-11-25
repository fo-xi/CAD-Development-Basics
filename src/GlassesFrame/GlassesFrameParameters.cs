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
		/// Ширина рамы линзы.
		/// </summary>
		private double _lensFrameWidth;

		/// <summary>
		/// Ширина линзы.
		/// </summary>
		private double _lensWidth;

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
		/// Возвращает и задает ширину рамы линзы.
		/// </summary>
		public double LensFrameWidth
		{
			get => _lensFrameWidth;
			set
			{
				Validator.AssertValue(value, 52, 58, "Ширина рамы линзы");
				Validator.IsLensWidth(LensWidth, value);
				_lensFrameWidth = value;
			}
		}

		/// <summary>
		/// Возвращает и задает ширину линзы.
		/// </summary>
		public double LensWidth
		{
			get => _lensWidth;
			set
			{
				Validator.AssertValue(value, 48, 54, "Ширина линзы");
				Validator.IsLensWidth(value, LensFrameWidth);
                _lensWidth = value;
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
			LensFrameWidth = 56;
			LensWidth = 54;
		}
	}
}
