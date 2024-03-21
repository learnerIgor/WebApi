namespace Common.Service
{
    public interface ICurrentUserService
    {
        public int CurrentUserId { get; }
        public string[] UserRole { get; }
    }
}
