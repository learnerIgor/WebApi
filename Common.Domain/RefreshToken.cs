namespace Common.Domain
{
    public class RefreshToken
    {
        public string Id { get; set; } = default!;
        public int ApplicationUserId {  get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
