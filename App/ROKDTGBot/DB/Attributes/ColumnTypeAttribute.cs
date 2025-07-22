using System;

namespace ROTGBot.Db.Attributes
{
    /// <summary>
    /// Атрибут - тип колонки
    /// </summary>
    public class ColumnTypeAttribute : Attribute
    {
        /// <summary>
        /// Наименование типа колонки
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public ColumnTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
