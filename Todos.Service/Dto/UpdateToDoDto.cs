
namespace Todos.Service.Dto
{
    public class UpdateToDoDto
    {
        public string Label { get; set; } = default!;
        public bool IsDone { get; set; }
    }
}
