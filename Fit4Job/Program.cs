namespace Fit4Job
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();




            /*********************** Database & User Identity ***********************/
            builder.Services.AddDbContext<Fit4JobDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnectionString"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<Fit4JobDbContext>()
                .AddDefaultTokenProviders();
            /********************************************************************************/






            var app = builder.Build();

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
    }
}
