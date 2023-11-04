using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using DentalLabManagement.API.Services.Implements;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Repository.Implement;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DentalLabManagement.API.Extensions
{

    public static class DependencyServices
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<DentalLabManagementContext>, UnitOfWork<DentalLabManagementContext>>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductionStageService, ProductionStageService>();
            services.AddScoped<IDentalService, DentalService>();
            services.AddScoped<ITeethPositionServices, TeethPositionService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemStageService, OrderItemStageService>();
            services.AddScoped<IWarrantyCardService, WarrantyCardService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<ICardTypeService, CardTypeService>();
            return services;
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = "DentalLab",
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("DentalLabNumberOne"))
                    };
                });
            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "DentalLab", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
            });
            return services;
        }
    }
}