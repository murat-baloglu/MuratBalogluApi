using Microsoft.Extensions.DependencyInjection;
using MuratBaloglu.Application.Abstractions.Services.Configurations;
using MuratBaloglu.Application.Abstractions.Storage;
using MuratBaloglu.Application.Abstractions.Token;
using MuratBaloglu.Infrastructure.Enums;
using MuratBaloglu.Infrastructure.Services.Configurations;
using MuratBaloglu.Infrastructure.Services.Storage;
using MuratBaloglu.Infrastructure.Services.Storage.Azure;
using MuratBaloglu.Infrastructure.Services.Storage.Local;
using MuratBaloglu.Infrastructure.Services.Token;

namespace MuratBaloglu.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
            //services.AddScoped<IStorage, LocalStorage>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IApplicationService, ApplicationService>();
        }

        //Bunu kullan. Clean Code olan bu.
        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection services, StorageTypes storageTypes)
        {
            switch (storageTypes)
            {
                case StorageTypes.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageTypes.Azure:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageTypes.AWS:
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }


    }
}
