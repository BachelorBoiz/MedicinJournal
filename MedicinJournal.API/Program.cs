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
using MedicinJournal.API.Middleware;

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

builder.Services.AddCors(options => options
    .AddPolicy("dev-policy", policyBuilder =>
        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ISymmetricCryptographyService, SymmetricCryptographyService>();
builder.Services.AddScoped<IAsymmetricCryptographyService, AsymmetricCryptographyService>();

builder.Services.AddDbContext<MedicinJournalDbContext>(options =>
    options.UseSqlite("Data Source=/data/journal.db"));

builder.Services.AddDbContext<SecurityDbContext>(options =>
    options.UseSqlite("Data Source=/data/auth.db"));

builder.Services.AddScoped<IJournalRepository, JournalRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IUserLoginRepository, UserLoginRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseCors("dev-policy");

await using (var scope = app.Services.CreateAsyncScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MedicinJournalDbContext>();
    var authCtx = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();
    var passwordHash = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    var symmetricService = scope.ServiceProvider.GetRequiredService<ISymmetricCryptographyService>();
    var asymmetricService = scope.ServiceProvider.GetRequiredService<IAsymmetricCryptographyService>();

    await ctx.Database.EnsureDeletedAsync();
    await authCtx.Database.EnsureDeletedAsync();

    await ctx.Database.EnsureCreatedAsync();
    await authCtx.Database.EnsureCreatedAsync();

    var testDataGenerator = new TestDataGenerator(ctx, authCtx, passwordHash, symmetricService, asymmetricService);

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

app.Use(next => context => {
    context.Request.EnableBuffering();
    return next(context);
});
app.UseMiddleware<AuditLogMiddleware>();

app.MapControllers();

app.Run();
