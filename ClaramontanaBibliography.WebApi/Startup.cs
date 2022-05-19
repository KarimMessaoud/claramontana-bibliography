using ClaramontanaBibliography.Data.Entities;
using ClaramontanaBibliography.Service;
using ClaramontanaBibliography.Service.PasswordHashers;
using ClaramontanaBibliography.Service.TokenGenerators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace ClaramontanaBibliography.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AddNewtonsoftJson replaces the System.Text.Json - based input and output formatters
            //used for formatting all JSON content.
            //The below option settings: options => options.InputFormatters.Insert(0, GetJsonPatchInputFormatter())
            //are to add support for JSON Patch using Newtonsoft.Json, while leaving the other formatters unchanged

            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            }).AddNewtonsoftJson();

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            Configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            services.AddSingleton<AccessTokenGenerator>();
            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILibraryItemService, LibraryItemService>();
            services.AddDbContext<LibraryContext>(options
                => options.UseSqlServer(Configuration.GetConnectionString("ClaramontanaLibraryConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClaramontanaBibliography.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClaramontanaBibliography.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
