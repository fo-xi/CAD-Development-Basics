using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
	public class MainWindowVM : ViewModelBase, INotifyDataErrorInfo
	{
		private GlassesFrameParameters _glassesFrameParameters = new GlassesFrameParameters();

		private GlassesFrameBuilder _glassesFrameBuilder = new GlassesFrameBuilder();

		private IMessageBoxService _messageBoxService;

		private Dictionary<Parameters, Action<double>> _parameters;

		private Dictionary<Parameters, string> _parametersName;

		private string _bridgeLength;

		private string _endPieceLength;

		private string _frameWigth;

		private string _lensFrameWidth;

		private string _lensWidth;

		protected readonly Dictionary<string, List<string>> _errorsByPropertyName
			= new Dictionary<string, List<string>>();

		public string Errors
		{
			get
			{
				return AllErrors();
			}
		}

		public string BridgeLength
		{
			get
			{
				return _bridgeLength;
			}
			set
			{
				Validate(value, Parameters.BridgeLength);
				_bridgeLength = value;
				RaisePropertyChanged(nameof(BridgeLength));
				RaisePropertyChanged(nameof(Errors));
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
				Validate(value, Parameters.EndPieceLength);
				_endPieceLength = value;
				RaisePropertyChanged(nameof(EndPieceLength));
				RaisePropertyChanged(nameof(Errors));
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
				Validate(value, Parameters.FrameWigth);
				_frameWigth = value;
				RaisePropertyChanged(nameof(FrameWigth));
				RaisePropertyChanged(nameof(Errors));
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
				Validate(value, Parameters.LensFrameWidth);
				_lensFrameWidth = value;
				RaisePropertyChanged(nameof(LensFrameWidth));
				RaisePropertyChanged(nameof(Errors));
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
				Validate(value, Parameters.LensWidth);
				_lensWidth = value;
				RaisePropertyChanged(nameof(LensWidth));
				RaisePropertyChanged(nameof(Errors));
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

			_parameters = new Dictionary<Parameters, Action<double>>
			{
				{ Parameters.BridgeLength, value => _glassesFrameParameters.BridgeLength = value },
				{ Parameters.EndPieceLength, value => _glassesFrameParameters.EndPieceLength = value },
				{ Parameters.FrameWigth, value => _glassesFrameParameters.FrameWigth = value },
				{ Parameters.LensFrameWidth, value => _glassesFrameParameters.LensFrameWidth = value },
				{ Parameters.LensWidth, value => _glassesFrameParameters.LensWidth = value }
			};

			_parametersName = new Dictionary<Parameters, string>
			{
				{ Parameters.BridgeLength, "Длина моста" },
				{ Parameters.EndPieceLength, "Длина концевого элемента" },
				{ Parameters.FrameWigth, "Ширина оправы" },
				{ Parameters.LensFrameWidth, "Ширина рамы линзы" },
				{ Parameters.LensWidth, "Ширина линзы" }
			};

			BridgeLength = _glassesFrameParameters.BridgeLength.ToString();
			EndPieceLength = _glassesFrameParameters.EndPieceLength.ToString();
			FrameWigth = _glassesFrameParameters.FrameWigth.ToString();
			LensFrameWidth = _glassesFrameParameters.LensFrameWidth.ToString();
			LensWidth = _glassesFrameParameters.LensWidth.ToString();

			ApplyCommand = new RelayCommand(Apply);
		}

		private void Apply()
		{
			if (HasErrors)
			{
				_messageBoxService.Show
					("Не получается построить модель. Посмотрите ошибки!");
			}
			else 
			{
				_glassesFrameBuilder.Build(_glassesFrameParameters);
			}
		}

		private bool IsNumber(string value, out double result, Parameters parameter)
		{
			if (double.TryParse(value, out result))
			{
				return true;
			}
			else
			{
				ClearErrors(parameter.ToString());
				if (string.IsNullOrEmpty(value))
				{
					AddError(parameter.ToString(),
						$"{_parametersName[parameter]}: cтрока не должна быть пустой!");
				}
				else
				{
					AddError(parameter.ToString(),
						$"{_parametersName[parameter]}: можно вводить только числовые значения!");
				}

				return false;
			}
		}

		private void Validate(string value, Parameters parameter)
		{
			if (!IsNumber(value, out double result, parameter))
			{
				return;
			} 

			if (string.IsNullOrEmpty(value))
			{
				AddError(parameter.ToString(),
					$"{_parametersName[parameter]}: cтрока не должна быть пустой!");
				return;
			}

			try
			{
				ClearErrors(parameter.ToString());
				_parameters[parameter](result);
			}
			catch (ArgumentException e)
			{
				AddError(parameter.ToString(), e.Message);
			}
		}

		public string AllErrors()
		{
			string errors = string.Empty;

			for (int i = 0; i < _errorsByPropertyName.Keys.Count; i++)
			{
				var value = _errorsByPropertyName.Values.ToArray()[i];
				foreach (var error in value)
				{
					errors += $"{error}\n";
				}
			}
			return errors;
		}

		/// <summary>
		/// Gets all error messages.
		/// </summary>
		/// <param name="propertyName">Property Name.</param>
		/// <returns></returns>
		public IEnumerable GetErrors(string propertyName)
		{
			return _errorsByPropertyName.ContainsKey(propertyName) ?
				_errorsByPropertyName[propertyName] : null;
		}

		/// <summary>
		///  Property indicates whether there are any validation errors.
		/// </summary>
		public virtual bool HasErrors
		{
			get
			{
				return _errorsByPropertyName.Any();
			}
		}

		/// <summary>
		/// Event must occur when the validation errors have changed
		/// for a property or for the entity.
		/// </summary>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		/// <summary>
		/// Event triggering.
		/// </summary>
		/// <param name="propertyName"></param>
		public void OnErrorsChanged(string propertyName)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Adds an error message to the error dictionary.
		/// </summary>
		/// <param name="propertyName">Property Name.</param>
		/// <param name="error">Error message.</param>
		protected void AddError(string propertyName, string error)
		{
			if (!_errorsByPropertyName.ContainsKey(propertyName))
				_errorsByPropertyName[propertyName] = new List<string>();

			if (!_errorsByPropertyName[propertyName].Contains(error))
			{
				_errorsByPropertyName[propertyName].Add(error);
				OnErrorsChanged(propertyName);
			}
		}

		/// <summary>
		/// Removes all errors by key.
		/// </summary>
		/// <param name="propertyName">Property Name.</param>
		protected void ClearErrors(string propertyName)
		{
			if (_errorsByPropertyName.ContainsKey(propertyName))
			{
				_errorsByPropertyName.Remove(propertyName);
				OnErrorsChanged(propertyName);
			}
		}
	}
}
