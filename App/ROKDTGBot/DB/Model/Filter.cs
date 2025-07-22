using System;
using System.Linq.Expressions;

namespace ROTGBot.Db.Model
{
    public class Filter<T> where T : IEntity
    {
        public int? Page { get; set; }
        public int? Size { get; set; }
        public string Sort { get; set; } = "";

        public Expression<Func<T, bool>>? Selector { get; set; }
    }
}