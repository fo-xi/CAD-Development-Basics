using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GlassesFrameViewModel;
using GlassesFrameViewModel.Service;

namespace GlassesFrameView.Service
{
    /// <summary>
    /// Сервис окна сообщения.
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        /// <summary>
        /// Соотносит тип сообщения с иконкой.
        /// </summary>
        private Dictionary<MessageType, MessageBoxImage> _messageBoxImages = 
            new Dictionary<MessageType, MessageBoxImage>()
            {
                {MessageType.Error, MessageBoxImage.Error},
                {MessageType.Warning, MessageBoxImage.Warning}
            };

        /// <summary>
        /// Показывает окно сообщения.
        /// </summary>
        /// <param name="text">Сообщение.</param>
        /// <param name="caption">Заголовок.</param>
        /// <param name="messageType">Иконка.</param>
        public void Show(string text, string caption, MessageType messageType)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, _messageBoxImages[messageType]);
        }
    }
}
