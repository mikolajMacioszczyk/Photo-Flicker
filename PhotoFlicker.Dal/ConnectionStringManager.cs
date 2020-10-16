using System.IO;
using Microsoft.Extensions.Configuration;

namespace PhotoFlicker.Dal
{
    public class ConnectionStringManager
    {
        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("/PhotoFlicker.Web/appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build().GetConnectionString("DefaultConnection");
        }
    }
}