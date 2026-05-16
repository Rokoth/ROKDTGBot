using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common
{
    /// <summary>
    /// Настройки авторизации
    /// </summary>
    public class AuthOptions
    {
        private Encoding DefaultEncoding => Encoding.GetEncoding(EncodingDefault);

        /// <summary>
        /// издатель токена
        /// </summary>
        public string Issuer { get; set; } = "";
        /// <summary>
        /// потребитель токена
        /// </summary>
        public string Audience { get; set; } = "";
        /// <summary>
        /// ключ для шифрации
        /// </summary>
        public string Key { get; set; } = "";
        /// <summary>
        /// время жизни токена - 1 минута
        /// </summary>
        public int LifeTime { get; set; }
        /// <summary>
        /// кодировка
        /// </summary>
        public string EncodingDefault { get; set; } = "";

        /// <summary>
        /// получить ключ
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new(DefaultEncoding.GetBytes(Key));
    }
}
