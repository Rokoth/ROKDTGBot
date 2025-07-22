namespace ROTGBot.Contract.Model
{
    public class NewsButton : Entity
    {       
        public long ChatId { get; set; }       
        public string? ChatName { get; set; }       
        public long? ThreadId { get; set; }
        public string? ThreadName { get; set; }
        public bool ToSend { get; set; }
        public int ButtonNumber { get; set; }
        public string? ButtonName { get; set; }
    }

}
