namespace Todos.Application.Dtos
{
    public class UpdateTodoPayLoad
    {
        public string Label { get; set; } = default!;
        public bool IsDone { get; set; }
    }
}
