using ROTGBot.Db.Attributes;

namespace ROTGBot.Db.Model
{
    [TableName("news")]
    public class News : Entity
    {
        [ColumnName("title")]
        public string? Title { get; set; }
        [ColumnName("description")]
        public string? Description { get; set; }
        [ColumnName("state")]
        public string? State { get; set; }
        [ColumnName("chatid")]
        public long ChatId { get; set; }
        [ColumnName("userid")]
        public Guid UserId { get; set; }
        [ColumnName("type")]
        public string Type { get; set; } = "news";
        [ColumnName("groupid")]
        public long? GroupId { get; set; }
        [ColumnName("threadid")]
        public long? ThreadId { get; set; }

        [ColumnName("createddate")]
        [ColumnType("timestamp")]
        public DateTime CreatedDate { get; set; }
        [ColumnName("moderatorid")]
        public Guid? ModeratorId { get; set; }
    }
}