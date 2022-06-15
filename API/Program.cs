using API.ApiConfiguration;

using Carter;

using Core.Interfaces;
using Core.Security;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Host.AddSerilog();

var services = builder.Services;

const string CorsName = "BuyersCors";
var cadenaConexion = builder.Configuration.GetConnectionString("Dev");
services.AddCarter();

var jwtConfig = new JwtConfiguration();
builder.Configuration.Bind("Jwt", jwtConfig);

services.AddCustomSecurity(jwtConfig);
services.AddBuyersCors(CorsName); //azzule.com/buyersapi   > sam.com

services.AddSwagger();
services.AddMediator();
services.AddValidators();
services.AddAutoMapper(typeof(IEndpoint));
var cnx = services.AddConnectionFactory(cadenaConexion);
services.AddProductRepository(cnx);
services.AddHttpClients();


var app = builder.Build();

app.MapSwagger();
app.UseCors(CorsName);
app.MapCarter();
app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>(jwtConfig);

app.Run();