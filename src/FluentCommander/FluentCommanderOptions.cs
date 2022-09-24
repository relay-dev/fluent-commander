using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("FluentCommander.SqlServer")]
[assembly: InternalsVisibleTo("FluentCommander.Oracle")]
namespace FluentCommander
{
    public class FluentCommanderOptions
    {
        protected internal string ConnectionString { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}
