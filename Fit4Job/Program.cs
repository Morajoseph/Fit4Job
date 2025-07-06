using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Fit4Job
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();


            /******************************* Swagger & OpenAPI ******************************/

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /********************************************************************************/



            /*************************** Database & User Identity ***************************/

            builder.Services.AddDbContext<Fit4JobDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("GlobalConnectionString"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<Fit4JobDbContext>()
                .AddDefaultTokenProviders();

            /********************************************************************************/


            /***************************** Dependency Injection *****************************/

            builder.Services.AddScoped<IAdminProfileRepository, AdminProfileRepository>();
            builder.Services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
            builder.Services.AddScoped<IJobSeekerProfileRepository, JobSeekerProfileRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            /********************************************************************************/




            /************************* Add JWT-Bearer  Authentication ***********************/


            string defaultKey = "xP5QyBL0T3yaUjvbf5BfM3znTT9gKpALDF6rD6+q9BQ=";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                string keyString = builder.Configuration["JWT:Key"] ?? defaultKey;
                byte[]? keyBytes = Convert.FromBase64String(keyString);
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });


            /********************************************************************************/



            /********************** Cross-Origin Resource Sharing (CORS) ********************/




            /********************************************************************************/



            /****************************** Application  Build ******************************/
            var app = builder.Build();


            // Seed roles after building the app
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                await SeedRolesAsync(roleManager);
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
        /******************************** Helper  methods ********************************/

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roleNames = { "Admin", "Company", "JobSeeker" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }
    }
}
