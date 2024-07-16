using Core.Security.Hashing;
using Core.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Application;
using Persistence;
using Core.Security;
using Infrastructure;
using Core.CrossCuttingConcerns.Exceptions;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddSecurityServices();

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    redisOptions.Configuration = builder.Configuration.GetConnectionString("RedisLocalConnectionString");
});

builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => { p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));


TokenOptions? tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidIssuer = tokenOptions?.Issuer,
        ValidAudience = tokenOptions?.Audience,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions?.SecurityKey ?? "")
    };
});

builder.Services.AddSwaggerGen(opt =>
{
    // opt.EnableAnnotations();

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345.54321\""
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
                { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] { }
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
    app.ConfigureCustomExceptionMiddleware();


app.UseCors(
    opt =>
        opt.WithOrigins(app.Configuration.GetSection("WebAPIConfiguration").Get<WebAPIConfiguration>().AllowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
