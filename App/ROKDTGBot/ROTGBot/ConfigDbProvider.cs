using Microsoft.EntityFrameworkCore;
using ROTGBot.Db.Context;

namespace ROTGBot
{
    public class ConfigDbProvider(Action<DbContextOptionsBuilder> options) : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder> _options = options;

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<DbPgContext>();
            _options(builder);

            using var context = new DbPgContext(builder.Options);
            var items = context.Settings
                .AsNoTracking()
                .ToList();

            foreach (var item in items)
            {
                Data.Add(item.ParamName, item.ParamValue);
            }
        }                
    }
}
