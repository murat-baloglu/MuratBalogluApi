using Microsoft.Extensions.Configuration;

namespace MuratBaloglu.Persistence
{
    static class Configuration
    {
        public static string? ConnectionString
        {
            get
            {
                var configurationManager = new ConfigurationManager();
                //Dot net cli kullanarak Migration yapılacaksa bu path i kullan. Migrationdan sonra tekrar eski haline getir. 
                //string basePath = Path.Combine(Directory.GetCurrentDirectory(), "../MuratBaloglu.API");
                string basePath = Directory.GetCurrentDirectory();
                configurationManager.SetBasePath(basePath);
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("SQLServer");
            }
        }
    }
}
