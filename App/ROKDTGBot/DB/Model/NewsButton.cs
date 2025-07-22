using ROTGBot.Db.Attributes;

namespace ROTGBot.Db.Model
{
    [TableName("newsbutton")]
    public class NewsButton : Entity
    {       
        [ColumnName("chatid")]
        public long ChatId { get; set; }
        [ColumnName("chatname")]
        public string? ChatName { get; set; }
        [ColumnName("threadid")]
        public long? ThreadId { get; set; }
        [ColumnName("threadname")]
        public string? ThreadName { get; set; }
        [ColumnName("tosend")]
        public bool ToSend { get; set; }
        [ColumnName("buttonnumber")]
        public int ButtonNumber { get; set; }
        [ColumnName("buttonname")]
        public string? ButtonName { get; set; }
    }
}