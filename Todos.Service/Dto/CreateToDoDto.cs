
namespace Todos.Service.Dto
{
    public class CreateToDoDto
    {
        public string Label { get; set; } = default!;
        public bool IsDone { get; set; }
    }
}
