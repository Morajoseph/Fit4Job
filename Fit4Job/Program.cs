using Fit4Job.Middlewares;
using Fit4Job.Services;
using Fit4Job.Services.Implementations;
using Fit4Job.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
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
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation   

                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fit4Job WebAPIs",
                    Version = "v1",
                    Description = "Fit4Job platform APIs using ASP.NET Core 8"
                });

                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // Configure Swagger UI
            //builder.Services.Configure<SwaggerUIOptions>(options =>
            //{
            //options.DefaultModelsExpandDepth(-1); // Hide schemas section
            //options.DocExpansion(DocExpansion.None); // Collapse all operations by default
            //options.EnableDeepLinking();
            //options.DisplayOperationId();
            //options.EnableFilter();
            //options.ShowExtensions();
            //options.EnableValidator();
            //options.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete, SubmitMethod.Patch);
            //});

            /********************************************************************************/



            /*************************** Database & User Identity ***************************/

            builder.Services.AddDbContext<Fit4JobDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("GlobalConnectionString"));
            });

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                })
                .AddEntityFrameworkStores<Fit4JobDbContext>()
                .AddDefaultTokenProviders();

            /********************************************************************************/


            /***************************** Dependency Injection *****************************/

            builder.Services.AddScoped<IAdminProfileRepository, AdminProfileRepository>();
            builder.Services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
            builder.Services.AddScoped<IJobSeekerProfileRepository, JobSeekerProfileRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISkillsService, SkillsService>();

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
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });


            /********************************************************************************/



            /********************** Cross-Origin Resource Sharing (CORS) ********************/

            builder.Services.AddCors(options =>
            {
                // Allow all origins (Fro DEVELOPMENT & Testing ONLY)
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.SetIsOriginAllowed(_ => true)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });

            });


            /********************************************************************************/



            /****************************** Application  Build ******************************/
            var app = builder.Build();

            app.UseMiddleware<GlobalErrorHandlerMiddleware>();

            // Seed roles after building the app
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                await SeedRolesAsync(roleManager);
            }

            // Enable CORS - Allow all connections
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

            }
            app.UseSwagger();
            app.UseSwaggerUI();


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
