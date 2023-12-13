using MedicinJournal.Core.IServices;
using MedicinJournal.Domain.IRepositories;
using MedicinJournal.Domain.Services;
using MedicinJournal.Infrastructure.Repositories;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using MedicinJournal.API;
using MedicinJournal.API.Jwt;
using MedicinJournal.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure;
using MedicinJournal.Security;
using MedicinJournal.Security.Repositories;
using Microsoft.AspNetCore.Http.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    });
});

builder.Services.AddAuthentication(authenticationOptions =>
    {
        authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddDbContext<MedicinJournalDbContext>(options =>
    options.UseSqlite("Data Source=/data/journal.db"));

builder.Services.AddDbContext<UserLoginDbContext>(options =>
    options.UseSqlite("Data Source=/data/auth.db"));

builder.Services.AddScoped<IJournalRepository, JournalRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserLoginRepository, UserLoginRepository>();


builder.Services.AddCors(options => options
    .AddPolicy("dev-policy", policyBuilder =>
        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MedicinJournalDbContext>();
    var authCtx = scope.ServiceProvider.GetRequiredService<UserLoginDbContext>();
    var passwordHash = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    await ctx.Database.EnsureDeletedAsync();
    await authCtx.Database.EnsureDeletedAsync();

    await ctx.Database.EnsureCreatedAsync();
    await authCtx.Database.EnsureCreatedAsync();

    var testDataGenerator = new TestDataGenerator(ctx, authCtx, passwordHash);

    testDataGenerator.Generate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("dev-policy");

app.MapControllers();

app.Run();
