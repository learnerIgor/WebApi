namespace Common.Domain
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Label { get; set; } = default!;
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
