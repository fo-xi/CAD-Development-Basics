using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassesFrameViewModel.Service
{
    /// <summary>
    /// Интерфейс окна сообщения.
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Показывает окно сообщения.
        /// </summary>
        /// <param name="text">Сообщение.</param>
        void Show(string text);
    }
}
