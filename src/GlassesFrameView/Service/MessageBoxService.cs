using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GlassesFrameViewModel.Service;

namespace GlassesFrameView.Service
{
    /// <summary>
    /// Сервис окна сообщения.
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        /// <summary>
        /// Показывает окно сообщения.
        /// </summary>
        /// <param name="text">Сообщение.</param>
        public void Show(string text)
        {
            MessageBox.Show(text);
        }
    }
}
