namespace Common.Service.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("Forbidden") { }
    }
}
