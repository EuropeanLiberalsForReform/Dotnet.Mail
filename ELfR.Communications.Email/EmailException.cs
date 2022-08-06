namespace ELfR.Communications.Email
{
    /// <summary>
    /// An exception that is thrown during handing e-mails.
    /// </summary>
    public class EmailException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EmailException" />.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EmailException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}