namespace MuratBaloglu.Application.Exceptions
{
    public class AuthenticationErrorException : Exception
    {
        public AuthenticationErrorException() : base("Şifreniz hatalı. Lütfen tekrar deneyin.")
        {
        }

        public AuthenticationErrorException(string? message) : base(message)
        {
        }

        public AuthenticationErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
