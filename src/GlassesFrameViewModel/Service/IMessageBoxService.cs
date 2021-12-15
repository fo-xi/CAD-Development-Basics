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
        /// <param name="caption">Заголовок.</param>
        /// <param name="messageType">Иконка.</param>
        void Show(string text, string caption, MessageType messageType);
    }
}
