using ELfR.Communications.Email.Smtp.Resources;

namespace ELfR.Communications.Email.Smtp
{
    /// <summary>
    /// An exception that is thrown if an error occurs during the submission of an e-mail through SMTP.
    /// </summary>
    public class SmtpEmailException : EmailException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SmtpEmailException"/>
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SmtpEmailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SmtpEmailException" />.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public SmtpEmailException(Exception innerException)
            : this(ExceptionMessages.UnexpectedError, innerException)
        {
        }
    }
}