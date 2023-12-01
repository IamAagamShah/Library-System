using BookHubAPI.Models;
using BookHubAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json;

namespace BookHubAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure BookDbContext
            services.AddDbContext<BooksDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BookDbContextConnection")));

            // Configure ReviewDbContext
            services.AddDbContext<ReviewsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ReviewDbContextConnection")));


            // Register repositories and services
            services.AddScoped<IBookRepository, BookRepository>(); // Assuming BookRepository implements IBookRepository

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookHubAPI", Version = "v1" });
            });
                
            /////////
            

            // Register repositories and services
            services.AddScoped<IReviewRepository, ReviewRepository>(); // Assuming BookRepository implements IBookRepository

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true; // Customize JsonSerializerOptions as needed
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Customize JsonSerializerOptions as needed
                                                                                                 // Other configuration options for System.Text.Json can be added here
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "ReviewHubAPI", Version = "v2" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookHubAPI v1");
                });
            }
            else
            {
                // Add error handling middleware for production if needed
                // app.UseExceptionHandler("/Home/Error");
                // app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}