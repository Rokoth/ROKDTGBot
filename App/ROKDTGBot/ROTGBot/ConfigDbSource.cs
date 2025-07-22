using Microsoft.EntityFrameworkCore;

namespace ROTGBot
{
    public class ConfigDbSource(Action<DbContextOptionsBuilder> optionsAction) : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction = optionsAction;

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {           
            return new ConfigDbProvider(_optionsAction);
        }
    }
}
