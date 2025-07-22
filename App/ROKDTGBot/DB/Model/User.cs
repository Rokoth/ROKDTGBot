using ROTGBot.Db.Attributes;
using System;

namespace ROTGBot.Db.Model
{
    [TableName("user")]
    public class User : Entity
    {
        [ColumnName("tgid")]
        public long TGId { get; set; }
        [ColumnName("name")]
        public string? Name { get; set; }
        [ColumnName("description")]
        public string? Description { get; set; }
        [ColumnName("tglogin")]
        public string? TGLogin { get; set; }
        [ColumnName("isnotify")]
        public bool IsNotify { get; set; }
        [ColumnName("chatid")]
        public long ChatId { get; set; }
    }
}