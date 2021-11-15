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
                _bridgeLength = value;
                RaisePropertyChanged(nameof(BridgeLength));
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
                _endPieceLength = value;
                RaisePropertyChanged(nameof(EndPieceLength));
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
                _frameWigth = value;
                RaisePropertyChanged(nameof(FrameWigth));
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
                _lensFrameWidth = value;
                RaisePropertyChanged(nameof(LensFrameWidth));
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
                _lensWidth = value;
                RaisePropertyChanged(nameof(LensWidth));
            }
        }
    }
}
