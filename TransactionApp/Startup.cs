using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TransactionApp.Data.Context;
using TransactionApp.Data.Repositories;
using TransactionApp.Core;
using TransactionApp.Domain.Objects;
using TransactionApp.Core.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TransactionApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TransactConnection"))
                );
            
            // Add framework services.
            services.AddMvc();
            services.AddScoped<IXlsxStreamProcessor<TransactionRowModel>, XlsxTransactionRawProcessor<TransactionRowModel>>();
            services.AddScoped<IUploadRepository, UploadRepository>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IValidationTransactService, ValidationTransactService>();
            services.AddScoped<IReportRepository, ReportRepository>(provider => new ReportRepository(Configuration.GetConnectionString("TransactConnection")));
            services.AddScoped<IReportService, ReportService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<DataContext>().Database.Migrate();                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
