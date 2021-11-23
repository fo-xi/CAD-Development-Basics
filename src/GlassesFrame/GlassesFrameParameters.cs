using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace GlassesFrame
{
	public class GlassesFrameParameters : ViewModelBase
	{
		private double _bridgeLength;

		private double _endPieceLength;

		private double _frameWigth;

		private double _lensFrameWidth;

		private double _lensWidth;

		public double BridgeLength
		{
			get
			{
				return _bridgeLength;
			}
			set
			{
				Validator.AssertValue(value, 10, 16, "Длина моста");
				_bridgeLength = value;
			}
		}

		public double EndPieceLength
		{
			get
			{
				return _endPieceLength;
			}
			set
			{
				Validator.AssertValue(value, 4, 8,
					"Длина концевого элемента");
				_endPieceLength = value;
			}
		}

		public double FrameWigth
		{
			get
			{
				return _frameWigth;
			}
			set
			{
				Validator.AssertValue(value, 2, 5, "Ширина оправы");
				_frameWigth = value;
			}
		}

		public double LensFrameWidth
		{
			get
			{
				return _lensFrameWidth;
			}
			set
			{
				Validator.AssertValue(value, 52, 58, "Ширина рамы линзы");
				Validator.IsLensWidth(LensWidth, value);
				_lensFrameWidth = value;
			}
		}

		public double LensWidth
		{
			get
			{
				return _lensWidth;
			}
			set
			{
				Validator.AssertValue(value, 48, 54, "Ширина линзы");
				Validator.IsLensWidth(value, LensFrameWidth);
				_lensWidth = value;
			}
		}

		public GlassesFrameParameters()
		{
			BridgeLength = 12;
			EndPieceLength = 6;
			FrameWigth = 3;
			LensFrameWidth = 56;
			LensWidth = 54;
		}
	}
}
