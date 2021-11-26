using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GlassesFrame;
using GlassesFrameViewModel.Service;
using InventorApi;

namespace GlassesFrameViewModel
{
	/// <summary>
	/// Класс, связывающий модель и представление.
	/// </summary>
	public class MainWindowVM : ViewModelBase, INotifyDataErrorInfo
	{
		/// <summary>
		/// Объект, хранящий в себе параметры оправы для очков.
		/// </summary>
		private readonly GlassesFrameParameters _glassesFrameParameters = new GlassesFrameParameters();

		/// <summary>
		/// Объект, хранящий в себе методы создания оправы для очков.
		/// </summary>
		private readonly GlassesFrameBuilder _glassesFrameBuilder = new GlassesFrameBuilder();

		/// <summary>
		/// Интерфейс окна сообщения.
		/// </summary>
		private readonly IMessageBoxService _messageBoxService;

		/// <summary>
		/// Соотносит свойство и метод присвоения значения.
		/// </summary>
		private readonly Dictionary<Parameters, Action<double>> _parameters;

		/// <summary>
		/// Соотносит название свойства со свойством.
		/// </summary>
		private readonly Dictionary<Parameters, string> _parametersName;

		/// <summary>
		/// Длина моста.
		/// </summary>
		private string _bridgeLength;

		/// <summary>
		/// Длина концевого элемента.
		/// </summary>
		private string _endPieceLength;

		/// <summary>
		/// Ширина оправы.
		/// </summary>
		private string _frameWidth;

		/// <summary>
		/// Ширина рамы линзы.
		/// </summary>
		private string _lensFrameWidth;

		/// <summary>
		/// Ширина линзы.
		/// </summary>
		private string _lensWidth;

		/// <summary>
		/// Содержит ошибки свойства.
		/// </summary>
		protected readonly Dictionary<string, List<string>> _errorsByPropertyName
			= new Dictionary<string, List<string>>();

		/// <summary>
		/// Получает ошибки всех свойств.
		/// </summary>
		public string Errors
		{
			get => AllErrors();
		}

		/// <summary>
		/// Возвращает и задает длину моста.
		/// </summary>
		public string BridgeLength
		{
			get => _bridgeLength;
			set
			{
				Validate(value, Parameters.BridgeLength);
				_bridgeLength = value;
				RaisePropertyChanged(nameof(BridgeLength));
				RaisePropertyChanged(nameof(Errors));
			}
		}

		/// <summary>
		/// Возвращает и задает длину концевого элемента.
		/// </summary>
		public string EndPieceLength
		{
			get => _endPieceLength;
			set
			{
				Validate(value, Parameters.EndPieceLength);
				_endPieceLength = value;
				RaisePropertyChanged(nameof(EndPieceLength));
				RaisePropertyChanged(nameof(Errors));
			}
		}

		/// <summary>
		/// Возвращает и задает ширину оправы.
		/// </summary>
		public string FrameWidth
		{
			get => _frameWidth;
			set
			{
				Validate(value, Parameters.FrameWidth);
				_frameWidth = value;
				RaisePropertyChanged(nameof(FrameWidth));
				RaisePropertyChanged(nameof(Errors));
			}
		}

		/// <summary>
		/// Возвращает и задает ширину рамы линзы.
		/// </summary>
		public string LensFrameWidth
		{
			get => _lensFrameWidth;
			set
			{
				Validate(value, Parameters.LensFrameWidth);
				_lensFrameWidth = value;
				RaisePropertyChanged(nameof(LensFrameWidth));
				RaisePropertyChanged(nameof(Errors));
            }
		}

		/// <summary>
		/// Возвращает и задает ширину линзы.
		/// </summary>
		public string LensWidth
		{
			get => _lensWidth;
			set
			{
				Validate(value, Parameters.LensWidth);
				_lensWidth = value;
                RaisePropertyChanged(nameof(LensWidth));
				RaisePropertyChanged(nameof(Errors));
            }
		}

		/// <summary>
		/// Возвращает и задает команду создания оправы для очков.
		/// </summary>
		public RelayCommand ApplyCommand { get; set; }

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="messageBoxService">Окно сообщений.</param>
		public MainWindowVM(IMessageBoxService messageBoxService)
		{
			_messageBoxService = messageBoxService;

			_parameters = new Dictionary<Parameters, Action<double>>
			{
				{ Parameters.BridgeLength, value => 
				_glassesFrameParameters.BridgeLength = value },
				{ Parameters.EndPieceLength, value => 
				_glassesFrameParameters.EndPieceLength = value },
				{ Parameters.FrameWidth, value => 
				_glassesFrameParameters.FrameWidth = value },
				{ Parameters.LensFrameWidth, value => 
				_glassesFrameParameters.LensFrameWidth = value },
				{ Parameters.LensWidth, value => 
				_glassesFrameParameters.LensWidth = value }
			};

			_parametersName = new Dictionary<Parameters, string>
			{
				{ Parameters.BridgeLength, "Длина моста" },
				{ Parameters.EndPieceLength, "Длина концевого элемента" },
				{ Parameters.FrameWidth, "Ширина оправы" },
				{ Parameters.LensFrameWidth, "Ширина рамы линзы" },
				{ Parameters.LensWidth, "Ширина линзы" }
			};

			BridgeLength = _glassesFrameParameters.BridgeLength.ToString();
			EndPieceLength = _glassesFrameParameters.EndPieceLength.ToString();
			FrameWidth = _glassesFrameParameters.FrameWidth.ToString();
			LensFrameWidth = _glassesFrameParameters.LensFrameWidth.ToString();
			LensWidth = _glassesFrameParameters.LensWidth.ToString();

			ApplyCommand = new RelayCommand(Apply);
		}

		/// <summary>
		/// Создание оправы для очков.
		/// </summary>
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

		/// <summary>
		/// Проверяет строку на число и на пустоту строки.
		/// </summary>
		/// <param name="value">Значение.</param>
		/// <param name="result">Значение типа double.</param>
		/// <param name="parameter">Параметры оправы для очков.</param>
		/// <returns></returns>
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
						$"{_parametersName[parameter]}: строка не должна быть пустой!");
				}
				else
				{
					AddError(parameter.ToString(),
						$"{_parametersName[parameter]}: можно вводить только числовые значения!");
				}

				return false;
			}
		}

		/// <summary>
		/// Валидация свойств.
		/// </summary>
		/// <param name="value">Значение.</param>
		/// <param name="parameter">Параметры оправы для очков.</param>
		private void Validate(string value, Parameters parameter)
		{
			if (!IsNumber(value, out double result, parameter))
			{
				return;
			}

			try
			{
                if (parameter == Parameters.LensFrameWidth || parameter == Parameters.LensWidth)
                {
                    ClearErrors(Parameters.LensFrameWidth.ToString());
                    ClearErrors(Parameters.LensWidth.ToString());
                }

                ClearErrors(parameter.ToString());
                _parameters[parameter](result);
			}
			catch (ArgumentException e)
			{
				AddError(parameter.ToString(), e.Message);
			}
		}

		/// <summary>
		/// Записывает все ошибки в строку.
		/// </summary>
		/// <returns>Строка.</returns>
		public string AllErrors()
		{
			var errors = string.Empty;

			for (int i = 0; i < _errorsByPropertyName.Keys.Count; i++)
			{
				var value = _errorsByPropertyName.Values.ToArray()[i];
				foreach (var error in value)
				{
					errors += $"\n{error}";
				}
			}
			return errors;
		}

		/// <summary>
		/// Получает все сообщения об ошибках свойства.
		/// </summary>
		/// <param name="propertyName">Название свойства.</param>
		/// <returns></returns>
		public IEnumerable GetErrors(string propertyName)
		{
			return _errorsByPropertyName.ContainsKey(propertyName) ?
				_errorsByPropertyName[propertyName] : null;
		}

		/// <summary>
		///  Показывает есть ли ошибки.
		/// </summary>
		public virtual bool HasErrors
		{
			get
			{
				return _errorsByPropertyName.Any();
			}
		}

		/// <summary>
		/// Событие, которое отреагирует при изменении списка ошибок.
		/// </summary>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		/// <summary>
		/// Включает событие.
		/// </summary>
		/// <param name="propertyName">Название свойства.</param>
		public void OnErrorsChanged(string propertyName)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Добавляет ошибку в словарь.
		/// </summary>
		/// <param name="propertyName">Название свойства.</param>
		/// <param name="error">Сообщение ошибки.</param>
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
		/// Удаляет ошибки по ключу.
		/// </summary>
		/// <param name="propertyName">Название свойства.</param>
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
