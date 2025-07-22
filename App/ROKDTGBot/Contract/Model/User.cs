namespace ROTGBot.Contract.Model
{
    public class User : Entity
    {        
        public long TGId { get; set; }        
        public string? Name { get; set; }        
        public string? Description { get; set; }       
        public string? TGLogin { get; set; }        
        public bool IsNotify { get; set; }       
        public long ChatId { get; set; }

        public List<RoleEnum> Roles { get; set; } = [];

        public bool IsAdmin => Roles.Contains(RoleEnum.administrator);
        public bool IsModerator => Roles.Contains(RoleEnum.moderator);
    }

}
