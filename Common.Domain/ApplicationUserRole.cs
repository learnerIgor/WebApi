namespace Common.Domain
{
    public class ApplicationUserRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public List<ApplicationUserApplicationRole> Users { get; set; } = new();
    }
}
