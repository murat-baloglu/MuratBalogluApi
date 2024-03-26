using Microsoft.AspNetCore.Http.Features;
using MuratBaloglu.Infrastructure;
using MuratBaloglu.Infrastructure.Services.Storage.Azure;
using MuratBaloglu.Infrastructure.Services.Storage.Local;
using MuratBaloglu.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
//builder.Services.AddStorage(MuratBaloglu.Infrastructure.Enums.StorageTypes.Local);
//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://drmuratbaloglu.com",
                           "https://drmuratbaloglu.com",
                           "http://localhost:7015",
                           "https://localhost:7015",
                           "http://localhost:4200",
                           "https://localhost:4200")
        .AllowAnyHeader().AllowAnyMethod();
    });
});

//builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddControllers();
//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add<ValidationFilter>();
//})
//    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<BlogCreateModelValidator>())
//    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

//app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
