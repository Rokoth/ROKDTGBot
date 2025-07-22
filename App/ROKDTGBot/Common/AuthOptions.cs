using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common
{
    public class AuthOptions
    {
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
        /// получить ключ
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
