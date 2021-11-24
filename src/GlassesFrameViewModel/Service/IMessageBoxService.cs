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
