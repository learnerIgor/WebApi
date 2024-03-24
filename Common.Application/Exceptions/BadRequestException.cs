namespace Common.Application.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException(string error): base(error) { }
    }
}
