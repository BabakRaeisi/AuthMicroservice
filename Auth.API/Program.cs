using Auth.API.Middleware;
using Auth.Core;
using Auth.Core.Entities;
using Auth.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddCore();
builder.Services.AddHealthChecks();
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalClient", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            if (origin == "https://microbooker.babakraeisi.com")
                return true;

            return Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
                   (uri.Host == "localhost" || uri.Host == "127.0.0.1");
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JwtSettings"));

var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] 
    ?? throw new InvalidOperationException("JwtSettings:Issuer is missing structure.");
var jwtAudience = builder.Configuration["JwtSettings:Audience"] 
    ?? throw new InvalidOperationException("JwtSettings:Audience is missing structure.");

// Fallback pattern to prevent container crashing if the environment key layout mismatches
var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JwtSettings:Secret is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)),
            RoleClaimType = "role",
            NameClaimType = "name"
        };
    });

var app = builder.Build();

// Initialize DB schema
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("PostgresConnection");
    using var conn = new NpgsqlConnection(connStr);
    await conn.OpenAsync();
    using var cmd = conn.CreateCommand();
    cmd.CommandText = """
        CREATE TABLE IF NOT EXISTS public."Users" (
            "UserID" UUID PRIMARY KEY,
            "Email" TEXT NOT NULL UNIQUE,
            "PersonName" TEXT,
            "Gender" TEXT,
            "Password" TEXT NOT NULL,
            "Role" TEXT NOT NULL DEFAULT 'Customer'
        );

        ALTER TABLE public."Users"
        ADD COLUMN IF NOT EXISTS "Role" TEXT NOT NULL DEFAULT 'Customer';
        """;
    await cmd.ExecuteNonQueryAsync();
}

// CORS must execute first to respond to preflight browser requests


app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors("AllowLocalClient");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();