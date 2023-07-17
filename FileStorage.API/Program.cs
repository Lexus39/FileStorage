using FileStorage.Core;
using FileStorage.DAL;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<FileService>();

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
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}