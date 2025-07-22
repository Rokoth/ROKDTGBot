using System;

namespace ROTGBot.Db.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }        
    }
}