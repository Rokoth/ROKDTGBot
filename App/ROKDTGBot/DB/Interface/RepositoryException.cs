using System;
using System.Runtime.Serialization;

namespace ROTGBot.Db.Interface
{
    /// <summary>
    /// Wrapper for exceptions in DB methods
    /// </summary>
    [Serializable]
    public class RepositoryException : Exception
    {
        /// <summary>
        /// default ctor
        /// </summary>
        public RepositoryException()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public RepositoryException(string message) : base(message)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }                
    }
}
