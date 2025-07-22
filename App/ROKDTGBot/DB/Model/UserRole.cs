using ROTGBot.Db.Attributes;

namespace ROTGBot.Db.Model
{
    [TableName("userrole")]
    public class UserRole : Entity
    {
        [ColumnName("userid")]
        public Guid UserId { get; set; }
        [ColumnName("roleid")]
        public Guid RoleId { get; set; }        
    }
}