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
		/// Радиус рамы линзы.
		/// </summary>
		private string _lensFrameRadius;

		/// <summary>
		/// Радиус линзы.
		/// </summary>
		private string _lensRadius;

		/// <summary>
		/// Ширина рамы линзы
		/// </summary>
		private string _lensFrameWidth;

		/// <summary>
		/// Высота рамы линзы.
		/// </summary>
		private string _lensFrameHeight;

		/// <summary>
		/// Форма линзы.
		/// </summary>
		private LensShape _selectedLensShape;

		/// <summary>
		///  Доступ к полям ширины и высоты рамы ринзы.
		/// </summary>
		private bool _isEnabledWidthHeight;

		/// <summary>
		/// Доступ к полям радиуса линзы и рамы линзы.
		/// </summary>
		private bool _isEnabledRadius;

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
                RaisePropertyChanged(nameof(HasErrors));
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
                RaisePropertyChanged(nameof(HasErrors));
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
                RaisePropertyChanged(nameof(HasErrors));
			}
		}

		/// <summary>
		/// Возвращает и задает радиус рамы линзы.
		/// </summary>
		public string LensFrameRadius
		{
			get => _lensFrameRadius;
			set
			{
				Validate(value, Parameters.LensFrameRadius);
				_lensFrameRadius = value;
				RaisePropertyChanged(nameof(LensFrameRadius));
				RaisePropertyChanged(nameof(Errors));
                RaisePropertyChanged(nameof(HasErrors));
			}
		}

		/// <summary>
		/// Возвращает и задает радиус линзы.
		/// </summary>
		public string LensRadius
		{
			get => _lensRadius;
			set
			{
				Validate(value, Parameters.LensRadius);
				_lensRadius = value;
                RaisePropertyChanged(nameof(LensRadius));
				RaisePropertyChanged(nameof(Errors));
                RaisePropertyChanged(nameof(HasErrors));
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
				RaisePropertyChanged(nameof(HasErrors));
			}
		}

		/// <summary>
		/// Возвращает и задает высоту рамы линзы.
		/// </summary>
		public string LensFrameHeight
		{
			get => _lensFrameHeight;
			set
			{
				Validate(value, Parameters.LensFrameHeight);
				_lensFrameHeight = value;
				RaisePropertyChanged(nameof(LensFrameHeight));
				RaisePropertyChanged(nameof(Errors));
				RaisePropertyChanged(nameof(HasErrors));
			}
		}

		/// <summary>
		/// Возвращает и задает форму линзы.
		/// </summary>
		public LensShape SelectedLensShape
		{
			get => _selectedLensShape;
			set
			{
				_selectedLensShape = value;
				ClearLensErrors();
				RaisePropertyChanged(nameof(SelectedLensShape));
			}
		}

		/// <summary>
		/// Возвращает и задает название формы линзы.
		/// </summary>
		public Dictionary<LensShape, string> GetLensShapeName { get; }

        /// <summary>
		/// Доступ к полям ширины и высоты рамы линзы.
		/// </summary>
		public bool IsEnabledWidthHeight
		{
			get => _isEnabledWidthHeight;
			set
			{

				_isEnabledWidthHeight = value;
				RaisePropertyChanged(nameof(IsEnabledWidthHeight));
			}
		}

		/// <summary>
		/// Доступ к полям радиуса линзы и рамы линзы.
		/// </summary>ы
		public bool IsEnabledRadius
		{
			get => _isEnabledRadius;
			set
			{

				_isEnabledRadius = value;
				RaisePropertyChanged(nameof(IsEnabledRadius));
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
			IsEnabledRadius = true;

			_parameters = new Dictionary<Parameters, Action<double>>
			{
				{ Parameters.BridgeLength, value => 
				_glassesFrameParameters.BridgeLength = value },
				{ Parameters.EndPieceLength, value => 
				_glassesFrameParameters.EndPieceLength = value },
				{ Parameters.FrameWidth, value => 
				_glassesFrameParameters.FrameWidth = value },
				{ Parameters.LensFrameRadius, value => 
				_glassesFrameParameters.LensFrameRadius = value },
				{ Parameters.LensRadius, value => 
				_glassesFrameParameters.LensRadius = value},
				{ Parameters.LensFrameWidth, value =>
				_glassesFrameParameters.LensFrameWidth = value},
				{ Parameters.LensFrameHeight, value =>
				_glassesFrameParameters.LensFrameHeight = value}
			};

			_parametersName = new Dictionary<Parameters, string>
			{
				{ Parameters.BridgeLength, "Длина моста" },
				{ Parameters.EndPieceLength, "Длина концевого элемента" },
				{ Parameters.FrameWidth, "Ширина оправы" },
				{ Parameters.LensFrameRadius, "Радиус рамы линзы" },
				{ Parameters.LensRadius, "Радиус линзы" },
				{ Parameters.LensFrameWidth, "Ширина рамы линзы" },
				{ Parameters.LensFrameHeight, "Высота рамы линзы" }
			};

            GetLensShapeName = new Dictionary<LensShape, string>
            {
                { LensShape.RoundShape, "Круглая" },
                { LensShape.RectangularShape, "Прямоугольная" },
            };

			BridgeLength = _glassesFrameParameters.BridgeLength.ToString();
			EndPieceLength = _glassesFrameParameters.EndPieceLength.ToString();
			FrameWidth = _glassesFrameParameters.FrameWidth.ToString();
			LensFrameRadius = _glassesFrameParameters.LensFrameRadius.ToString();
			LensRadius = _glassesFrameParameters.LensRadius.ToString();
			LensFrameWidth = _glassesFrameParameters.LensFrameWidth.ToString();
			LensFrameHeight = _glassesFrameParameters.LensFrameHeight.ToString();

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
					("Не получается построить модель. Посмотрите ошибки!", 
                    "Ошибка", MessageType.Error);
			}
			else 
			{
				if (SelectedLensShape == LensShape.RoundShape)
                {
					_glassesFrameBuilder.BuilRoundGlassesFrame(_glassesFrameParameters);
				}

				if (SelectedLensShape == LensShape.RectangularShape)
				{
					_glassesFrameBuilder.BuilRectangularGlassesFrame(_glassesFrameParameters);
				}
			}
		}

		/// <summary>
		/// Очищает ошибки свойств при переключении формы линз.
		/// </summary>
		private void ClearLensErrors()
		{
			switch (SelectedLensShape)
			{
				case LensShape.RoundShape:
				{
					IsEnabledWidthHeight = false;
					IsEnabledRadius = true;
					ClearErrors(Parameters.LensFrameWidth.ToString());
					ClearErrors(Parameters.LensFrameHeight.ToString());
					Validate(LensFrameRadius, Parameters.LensFrameRadius);
					Validate(LensRadius, Parameters.LensRadius);
					break;
				}
				case LensShape.RectangularShape:
				{

					IsEnabledWidthHeight = true;
					IsEnabledRadius = false;

					ClearErrors(Parameters.LensFrameRadius.ToString());
					ClearErrors(Parameters.LensRadius.ToString());
					Validate(LensFrameWidth, Parameters.LensFrameWidth);
					Validate(LensFrameHeight, Parameters.LensFrameHeight);
					break;
				}
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
                if (parameter == Parameters.LensFrameRadius || parameter == Parameters.LensRadius)
                {
                    ClearErrors(Parameters.LensFrameRadius.ToString());
                    ClearErrors(Parameters.LensRadius.ToString());
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
		private string AllErrors()
		{
			var errors = string.Empty;

			for (int i = 0; i < _errorsByPropertyName.Keys.Count; i++)
			{
                if (i != 0)
                {
                    errors += "\n";
				}

				var value = _errorsByPropertyName.Values.ToArray()[i];
				foreach (var error in value)
				{
					errors += $"{error}";
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
			RaisePropertyChanged(nameof(HasErrors));
			RaisePropertyChanged(nameof(Errors));
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
