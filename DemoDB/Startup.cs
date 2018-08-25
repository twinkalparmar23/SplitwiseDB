using DemoDB.Database;
using DemoDB.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoDB
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
            services.AddDbContext<DemoDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DemoDB")));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFriendListRepository, FriendListRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ISettlementRepository, SettlementRepository>();

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors((corsPolicyBuilder) =>
            {
                corsPolicyBuilder.AllowAnyOrigin();
                corsPolicyBuilder.AllowAnyMethod();
                corsPolicyBuilder.AllowAnyHeader();
                corsPolicyBuilder.WithExposedHeaders("X-InlineCount");
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
