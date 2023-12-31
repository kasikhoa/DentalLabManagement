using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.API.Converter;
using DentalLabManagement.API.Middlewares;
using DentalLabManagement.API.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using DentalLabManagement.DataTier.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace DentalLabManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Logging.ClearProviders();

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            builder.Services.AddDbContext<DentalLabManagementContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDatabase"));
            });

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsConstant.PolicyName,
                    policy => 
                    { 
                        policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod(); 
                    });
            });
            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddUnitOfWork();
            builder.Services.AddServices();
            builder.Services.AddJwtValidation();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddConfigSwagger();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            //app.UseHttpsRedirection();
            app.UseCors(CorsConstant.PolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            
            app.Run();


        }
    }
}

