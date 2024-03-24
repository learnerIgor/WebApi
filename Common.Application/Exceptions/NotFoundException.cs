using Newtonsoft.Json;

namespace Common.Application.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(object filter) : base("Not found by filter: " + JsonConvert.SerializeObject(filter)) { }
    }
}
