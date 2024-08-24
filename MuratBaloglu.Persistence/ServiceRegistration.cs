using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MuratBaloglu.Application.Abstractions.Services;
using MuratBaloglu.Application.Abstractions.Services.Authentications;
using MuratBaloglu.Application.Repositories.AboutMeImageFileRepository;
using MuratBaloglu.Application.Repositories.AboutMeRepository;
using MuratBaloglu.Application.Repositories.BlogImageFileRepository;
using MuratBaloglu.Application.Repositories.BlogRepository;
using MuratBaloglu.Application.Repositories.CarouselImageFileRepository;
using MuratBaloglu.Application.Repositories.ContactRepository;
using MuratBaloglu.Application.Repositories.EndpointRepository;
using MuratBaloglu.Application.Repositories.FileRepository;
using MuratBaloglu.Application.Repositories.MenuRepository;
using MuratBaloglu.Application.Repositories.NewsImageFileRepository;
using MuratBaloglu.Application.Repositories.NewsRepository;
using MuratBaloglu.Application.Repositories.PatientCommentRepository;
using MuratBaloglu.Application.Repositories.SocialMediaAccountRepository;
using MuratBaloglu.Application.Repositories.SpecialityCategoryRepository;
using MuratBaloglu.Application.Repositories.SpecialityImageFileRepository;
using MuratBaloglu.Application.Repositories.SpecialityRepository;
using MuratBaloglu.Application.Repositories.VideoRepository;
using MuratBaloglu.Application.Repositories.WorkingHourRepository;
using MuratBaloglu.Domain.Entities.Identity;
using MuratBaloglu.Persistence.Contexts;
using MuratBaloglu.Persistence.Repositories.AboutMeImageFileRepository;
using MuratBaloglu.Persistence.Repositories.AboutMeRepository;
using MuratBaloglu.Persistence.Repositories.BlogImageFileRepository;
using MuratBaloglu.Persistence.Repositories.BlogRepository;
using MuratBaloglu.Persistence.Repositories.CarouselImageFileRepository;
using MuratBaloglu.Persistence.Repositories.ContactRepository;
using MuratBaloglu.Persistence.Repositories.EndpointRepository;
using MuratBaloglu.Persistence.Repositories.FileRepository;
using MuratBaloglu.Persistence.Repositories.MenuRepository;
using MuratBaloglu.Persistence.Repositories.NewsImageFileRepository;
using MuratBaloglu.Persistence.Repositories.NewsRepository;
using MuratBaloglu.Persistence.Repositories.PatientCommentRepository;
using MuratBaloglu.Persistence.Repositories.SocialMediaAccountRepository;
using MuratBaloglu.Persistence.Repositories.SpecialityCategoryRepository;
using MuratBaloglu.Persistence.Repositories.SpecialityImageFileRepository;
using MuratBaloglu.Persistence.Repositories.SpecialityRepository;
using MuratBaloglu.Persistence.Repositories.VideoRepository;
using MuratBaloglu.Persistence.Repositories.WorkingHourRepository;
using MuratBaloglu.Persistence.Services;

namespace MuratBaloglu.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<MuratBalogluDbContext>(options => options.UseSqlServer(Configuration.ConnectionString), ServiceLifetime.Scoped);

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<MuratBalogluDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();
            services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            services.AddScoped<IMenuWriteRepository, MenuWriteRepository>();
            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IVideoReadRepository, VideoReadRepository>();
            services.AddScoped<IVideoWriteRepository, VideoWriteRepository>();
            services.AddScoped<IContactReadRepository, ContactReadRepository>();
            services.AddScoped<IContactWriteRepository, ContactWriteRepository>();
            services.AddScoped<ISocialMediaAccountReadRepository, SocialMediaAccountReadRepository>();
            services.AddScoped<ISocialMediaAccountWriteRepository, SocialMediaAccountWriteRepository>();
            services.AddScoped<IWorkingHourReadRepository, WorkingHourReadRepository>();
            services.AddScoped<IWorkingHourWriteRepository, WorkingHourWriteRepository>();
            services.AddScoped<ICarouselImageFileReadRepository, CarouselImageFileReadRepository>();
            services.AddScoped<ICarouselImageFileWriteRepository, CarouselImageFileWriteRepository>();
            services.AddScoped<IPatientCommentReadRepository, PatientCommentReadRepository>();
            services.AddScoped<IPatientCommentWriteRepository, PatientCommentWriteRepository>();

            services.AddScoped<IAboutMeReadRepository, AboutMeReadRepository>();
            services.AddScoped<IAboutMeWriteRepository, AboutMeWriteRepository>();
            services.AddScoped<IAboutMeImageFileReadRepository, AboutMeImageFileReadRepository>();
            services.AddScoped<IAboutMeImageFileWriteRepository, AboutMeImageFileWriteRepository>();

            services.AddScoped<IBlogReadRepository, BlogReadRepository>();
            services.AddScoped<IBlogWriteRepository, BlogWriteRepository>();
            services.AddScoped<IBlogImageFileReadRepository, BlogImageFileReadRepository>();
            services.AddScoped<IBlogImageFileWriteRepository, BlogImageFileWriteRepository>();

            services.AddScoped<ISpecialityReadRepository, SpecialityReadRepository>();
            services.AddScoped<ISpecialityWriteRepository, SpecialityWriteRepository>();
            services.AddScoped<ISpecialityImageFileReadRepository, SpecialityImageFileReadRepository>();
            services.AddScoped<ISpecialityImageFileWriteRepository, SpecialityImageFileWriteRepository>();
            services.AddScoped<ISpecialityCategoryReadRepository, SpecialityCategoryReadRepository>();
            services.AddScoped<ISpecialityCategoryWriteRepository, SpecialityCategoryWriteRepository>();

            services.AddScoped<INewsReadRepository, NewsReadRepository>();
            services.AddScoped<INewsWriteRepository, NewsWriteRepository>();
            services.AddScoped<INewsImageFileReadRepository, NewsImageFileReadRepository>();
            services.AddScoped<INewsImageFileWriteRepository, NewsImageFileWriteRepository>();
        }
    }
}
