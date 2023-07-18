using FileStorage.DAL;
using Microsoft.EntityFrameworkCore;
using FileStorage.Client;
using FileStorage.Core.Services;
using FileStorage.Core.Interfaces;
using FileStorage.DAL.Repositories;

namespace FileStorage.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("Default");

            // Add services to the container.

            builder.Services.AddDbContext<FileStorageDbContext>(options =>
            options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IFileModelRepository, FileModelRepository>();
            builder.Services.AddScoped<IOneTimeLinkRepository, OneTimeLinkRepository>();
            builder.Services.AddScoped<FileService>();
            builder.Services.AddScoped<LinkService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseWebAssemblyDebugging();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}