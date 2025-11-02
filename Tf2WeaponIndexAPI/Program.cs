using Tf2WeaponIndexAPI.Models;
using Tf2WeaponIndexAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JwtSettings = Tf2WeaponIndexAPI.Models.JwtSettings;
var builder = WebApplication.CreateBuilder(args);



//----MongoDB----
builder.Services.Configure<MongoDBSettings>(
builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<MongoDBService>();

//----Controllers & Swagger----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//----CORS Configuration----
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
    policy => policy
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());
});
//----JWT Authentication Configuration----
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});
builder.Services.AddScoped<JwtTokenGenerator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
