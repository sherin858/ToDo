using BL.Services.ToDoService;
using DAL.Models;
using DAL.Repos;
using DAL.Repos.ToDo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace lab34
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddControllers();
            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });
            #endregion
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Database

            var connectionString = builder.Configuration.GetConnectionString("ToDo");
            builder.Services.AddDbContext<ToDoContext>(options =>
                options.UseSqlServer(connectionString));

            #endregion

            #region Identity Manager

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<ToDoContext>();

            #endregion

            #region Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Cool";
                options.DefaultChallengeScheme = "Cool";
            })
                .AddJwtBearer("Cool", options =>
                {
                    string keyString = builder.Configuration.GetValue<string>("SecretKey") ?? string.Empty;
                    var keyInBytes = Encoding.ASCII.GetBytes(keyString);
                    var key = new SymmetricSecurityKey(keyInBytes);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            #endregion

            #region Authorization

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Users", policy => policy
                    .RequireClaim(ClaimTypes.Role, "User")
                    .RequireClaim(ClaimTypes.NameIdentifier));
            });

            #endregion

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps("BL");
            });

            #region Layers_Interface_Object
            builder.Services.AddScoped<IToDoRepo, ToDoRepo>();
            builder.Services.AddScoped<IToDoService, ToDoService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}