using System;

namespace ROTGBot.Db.Attributes
{
    /// <summary>
    /// Атрибут Имя колонки БД
    /// </summary>
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// Имя колоник БД
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
