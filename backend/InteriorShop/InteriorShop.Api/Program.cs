using InteriorShop.Application.Interfaces;
using InteriorShop.Infrastructure.Identity;
using InteriorShop.Infrastructure.Persistence;
using InteriorShop.Infrastructure.Seeders;
using InteriorShop.Infrastructure.Services;
using InteriorShop.Infrastructure.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT
var key = builder.Configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key missing");
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// AutoMapper: load all assemblies (or specify your infra assembly)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// DI services
builder.Services.AddScoped<IFileStorage, FileStorageLocal>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// apply migrations + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    // Migrate
    db.Database.Migrate();

    // call your seeder (async)
    await DbSeeder.SeederAsync(db, userMgr, roleMgr);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
