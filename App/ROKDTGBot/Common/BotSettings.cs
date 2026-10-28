namespace Common
{
    /// <summary>
    /// Настройки подключения к боту
    /// </summary>
    public class BotSettings
    {       
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// Адрес сервера (прокси) Телеграм
        /// </summary>
        public string ServerAddress { get; set; } = string.Empty;
    }
}
