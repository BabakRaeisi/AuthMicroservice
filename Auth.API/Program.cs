using Auth.API.Middleware;
using Auth.Core;
using Auth.Core.Entities;
using Auth.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

   
builder.Services.AddInfrastructure();
            
builder.Services.AddCore();

builder.Services.AddControllers().AddJsonOptions(options=>
options.JsonSerializerOptions.Converters.Add(new  JsonStringEnumConverter())
);
//builder.Services.AddAutoMapper(
//    typeof(ApplicationUserMappingProfile).Assembly,
//    typeof(RegisterRequestMappingProfile).Assembly
// );
//fluent validation
 builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt=>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173","http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JwtSettings"));

var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] 
    ?? throw new InvalidOperationException("JwtSettings:Issuer is missing.");
var jwtAudience = builder.Configuration["JwtSettings:Audience"] 
    ?? throw new InvalidOperationException("JwtSettings:Audience is missing.");
var jwtSecret = builder.Configuration["JwtSettings:Secret"] 
    ?? throw new InvalidOperationException("JwtSettings:Secret is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

