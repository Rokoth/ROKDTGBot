namespace Common
{
    public class CommonOptions
    {
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();
        
    }
}
