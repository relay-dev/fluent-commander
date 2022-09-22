using Microsoft.Extensions.Configuration;

namespace FluentCommander
{
    public class FluentCommanderOptions
    {
        public string ConnectionString { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}
