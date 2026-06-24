namespace Hospital.Domain.Exceptions
{
    public abstract class AppException : Exception
    {
        protected AppException(string message) : base(message) { }
    }

    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message) { }
    }
    public class UnauthorizedAppException : AppException
    {
        public UnauthorizedAppException(string message) : base(message) { }
    }
}
