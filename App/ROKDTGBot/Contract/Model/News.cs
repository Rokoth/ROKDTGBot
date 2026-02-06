namespace ROTGBot.Contract.Model
{
    public class News : Entity
    {        
        public string? Title { get; set; }        
        public string? Description { get; set; }        
        public string? State { get; set; }     
        public long ChatId { get; set; }     
        public Guid UserId { get; set; }        
        public string Type { get; set; } = "news";     
        public long? GroupId { get; set; }       
        public long? ThreadId { get; set; }              
        public DateTime CreatedDate { get; set; }
        public bool IsMulti { get; set; }
    }
}
