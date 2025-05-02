using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using MySql.Data.MySqlClient;
using brunchie_backend.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using brunchie_backend.Repositories;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Identity;
using brunchie_backend.Services;
using brunchie_backend.Models;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Brunchie_Connection"),
        new MySqlServerVersion(new Version(8, 0, 27))));

builder.Services.AddSingleton<MySqlConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Brunchie_Connection"); // Assuming you have a connection string in appsettings.json
    return new MySqlConnection(connectionString);
});

builder.Services.AddIdentity<User, IdentityRole>()
       .AddEntityFrameworkStores<AppDbContext>()
       .AddDefaultTokenProviders();


    
    

builder.Services.AddAuthorization();
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<AuthService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuRepository,MenuRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Feedback, FeedbackDto>()
       .ForAllMembers(options =>
           options.Condition((source, destination, sourceMember) => sourceMember != null));
    
    
});

builder.Services.AddSingleton<IMapper>(config.CreateMapper());

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string[] roleNames = { "Admin", "User", "Vendor" };  
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
