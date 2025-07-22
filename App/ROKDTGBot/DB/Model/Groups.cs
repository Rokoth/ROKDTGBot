using ROTGBot.Db.Attributes;

namespace ROTGBot.Db.Model
{
    [TableName("groups")]
    public class Groups : Entity
    {
        [ColumnName("title")]
        public string? Title { get; set; }
        [ColumnName("description")]
        public string? Description { get; set; }
        [ColumnName("sendnews")]
        public bool SendNews { get; set; }
        [ColumnName("chatid")]
        public long ChatId { get; set; }
        [ColumnName("threadid")]
        public long? ThreadId { get; set; }        
    }
}