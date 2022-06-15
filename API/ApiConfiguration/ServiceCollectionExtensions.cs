using Core.Domain.Entities;
using Core.Infrastructure;
using Core.Infrastructure.Repositories;
using Core.Interfaces;
using Core.Security;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

namespace API.ApiConfiguration
{
  public static class ServiceCollectionExtensions
  {

    public static IServiceCollection AddCustomSecurity(this IServiceCollection services, JwtConfiguration jwtConfiguration)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidIssuer = jwtConfiguration.Issuer,
          ValidAudience = jwtConfiguration.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.GetKey()),
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = false,
          ValidateIssuerSigningKey = true
        };
      });

      //services.AddAuthorization();



      //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c => {
      //      c.Authority = "https://azzule.com";
      //      //c.TokenValidationParameters = new TokenValidationParameters
      //      //{
      //      //  ValidAudience = jwtConfiguration.Audience,
      //      //  ValidIssuer = jwtConfiguration.Issuer
      //      //};
      //    });

      //services.AddAuthorization(opts => {
      //  opts.AddPolicy("buyers:user", 
      //    p => p.RequireAuthenticatedUser()
      //          .RequireClaim("sam")
      //  );
      //  //opts.FallbackPolicy = new AuthorizationPolicyBuilder()
      //  //  .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      //  //  .RequireAuthenticatedUser()
      //  //  .Build();
      //});


      services.AddSingleton(jwtConfiguration);
      return services;
    }
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
      var securityScheme = new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "jwt security"
      };
      var securityReq = new OpenApiSecurityRequirement()
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
          new string[] {}
        }
      };
      var info = new OpenApiInfo()
      {
        Version = "v1",
        Title = "Buyers Api - DEMO",
        Description = "Buyers api with minimal approach",
        TermsOfService = new Uri("https://www.azzule.com")
      };

      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1", info);
        options.AddSecurityDefinition("Bearer", securityScheme);
        options.AddSecurityRequirement(securityReq);
      });
      return services;
    }
    public static IServiceCollection AddBuyersCors(this IServiceCollection services, string corsName = "BuyersCORS")
    {
      services.AddCors(cors =>
      {
        cors.AddPolicy(name: corsName, builder =>
        {
          builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
        });
      });
      return services;
    }
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
      services.AddMediatR(typeof(IEndpoint));
      //services.AddMediatR(typeof(Program));
      return services;
    }
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
      services.AddValidatorsFromAssemblyContaining(typeof(ProductEntity));
      return services;
    }
    public static IDBConnectionFactory AddConnectionFactory(this IServiceCollection services, string cadenaConexion)
    {
      var sqlConnection = new SQLConnectionFactory(cadenaConexion);
      services.AddSingleton<IDBConnectionFactory>(sqlConnection);
      return sqlConnection;
    }
    public static IServiceCollection AddProductRepository(this IServiceCollection services, IDBConnectionFactory cnx)
    {
      services.AddSingleton<IProductRepository<ProductEntity>>(new ProductRepository(cnx));
      return services;
    }
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
      services.AddHttpClient("API:Classic", client => {
        client.BaseAddress = new Uri("https://demo.azzule.com/api/servicios.asmx/");
        client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
      });
      return services;
    }

  }
}
