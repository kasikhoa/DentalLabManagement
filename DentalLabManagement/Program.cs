using DentalLabManagement.API.Constants;
using DentalLabManagement.API.Converter;
using DentalLabManagement.API.Middlewares;
using DentalLabManagement.API.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace DentalLabManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
                var builder = WebApplication.CreateBuilder(args);
                builder.Logging.ClearProviders();
               

                // Add services to the container.
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(name: CorsConstant.PolicyName,
                        policy => { policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); });
                });
                builder.Services.AddControllers().AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
                });
                builder.Services.AddDatabase();
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