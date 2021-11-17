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
            ApplyCommand = new RelayCommand(Apply);
        }

        private void Apply()
        {
            _glassesFrameBuilder.Build(_glassesFrameParameters);
        }
    }
}
