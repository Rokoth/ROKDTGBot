using ROTGBot.Db.Attributes;

namespace ROTGBot.Db.Model
{
    [TableName("newsmessage")]
    public class NewsMessage : Entity
    {
        [ColumnName("newsid")]
        public Guid NewsId { get; set; }
        [ColumnName("tgmessageid")]
        public long TGMessageId { get; set; }
        
        [ColumnName("valuetext")]
        public string? TextValue { get; set; }
    }
}