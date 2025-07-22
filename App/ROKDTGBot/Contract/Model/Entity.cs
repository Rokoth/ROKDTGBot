using ROTGBot.Contract.Filters;
using ROTGBot.Contract.Interfaces;
using System;

namespace ROTGBot.Contract.Model
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
    
}
