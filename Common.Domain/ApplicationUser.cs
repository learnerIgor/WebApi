using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Common.Domain
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;

        [JsonIgnore]
        [IgnoreDataMember]
        public List<ToDo> ToDos { get; set; } = new();

        public List<ApplicationUserApplicationRole> Roles { get; set; } = new();

        public virtual List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
