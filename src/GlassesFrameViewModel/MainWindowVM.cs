using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GlassesFrame;
using GlassesFrameViewModel.Service;
using InventorApi;

namespace GlassesFrameViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private GlassesFrameParameters _glassesFrameParameters;

        private GlassesFrameBuilder _glassesFrameBuilder = new GlassesFrameBuilder();

        private IMessageBoxService _messageBoxService;

        private string _bridgeLength;

        private string _endPieceLength;

        private string _frameWigth;

        private string _lensFrameWidth;

        private string _lensWidth;

        public string BridgeLength
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

        public string EndPieceLength
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

        public string FrameWigth
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

        public string LensFrameWidth
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

        public string LensWidth
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

        public GlassesFrameParameters GlassesFrameParameters
        {
            get
            {
                return _glassesFrameParameters;
            }
            set
            {
                _glassesFrameParameters = value;
                RaisePropertyChanged(nameof(GlassesFrameParameters));
            }
        }

        public GlassesFrameBuilder GlassesFrameBuilder
        {
            get
            {
                return _glassesFrameBuilder;
            }
            set
            {
                _glassesFrameBuilder = value;
                RaisePropertyChanged(nameof(GlassesFrameBuilder));
            }
        }

        public RelayCommand ApplyCommand { get; set; }

        public MainWindowVM(IMessageBoxService messageBoxService)
        {
            _messageBoxService = messageBoxService;
            ApplyCommand = new RelayCommand(Apply);
        }

        private void Apply()
        {
            _glassesFrameBuilder.Build(_glassesFrameParameters);
        }

        private bool IsNumber(string value, out double result, out string message)
        {
            message = string.Empty;

            if (double.TryParse(value, out result))
            {
                return true;
            }

            message = "Можно вводить только числовые значения";
            return false;
        }
    }
}
