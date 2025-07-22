using ROTGBot.Contract.Interfaces;
using ROTGBot.Contract.Model;

namespace ROTGBot.Contract.Filters
{
    public abstract class Filter<T> : IFilter<T> where T : Entity
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="size">Page size</param>
        /// <param name="page">Page number</param>
        /// <param name="sort">Sort field</param>
        public Filter(int? size, int? page, string sort)
        {
            Size = size;
            Page = page;
            Sort = sort;
        }
        /// <summary>
        /// Page size
        /// </summary>
        public int? Size { get; }
        /// <summary>
        /// Page number
        /// </summary>
        public int? Page { get; }
        /// <summary>
        /// Sort field
        /// </summary>
        public string Sort { get; }
    }
}
