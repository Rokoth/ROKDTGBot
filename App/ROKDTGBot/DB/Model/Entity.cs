using ROTGBot.Db.Attributes;
using System;
using System.Collections.Generic;

namespace ROTGBot.Db.Model
{
    public abstract class Entity: IEntity
    {
        [PrimaryKey]
        [ColumnName("id")]
        public Guid Id { get; set; }
      
        [ColumnName("is_deleted")]
        public bool IsDeleted { get; set; }
       
    }
}