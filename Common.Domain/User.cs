using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Common.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual List<ToDo> ToDos { get; set; } = new();
    }
}
