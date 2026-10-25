using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common
{
    /// <summary>
    /// Настройки авторизации
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// издатель токена
        /// </summary>
        public string Issuer { get; set; } = string.Empty;
        /// <summary>
        /// потребитель токена
        /// </summary>
        public string Audience { get; set; } = string.Empty;
        /// <summary>
        /// ключ для шифрации
        /// </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// время жизни токена - 1 минута
        /// </summary>
        public int LifeTime { get; set; } = 60;

        /// <summary>
        /// получить ключ
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
