using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fastbank2.Api.Repo;
using Fastbank2.Api.Interfaces;
using Fastbank2.Api.Model;


namespace Fastbank2.UI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase());
            // Add framework services.
            services.AddMvc();
            services.AddTransient<IUnitofWork, UnitofWork>();



            //services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();


            //var context = app.ApplicationServices.GetService<ApiContext>();
           // AddTestData(context);
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void AddTestData(ApiContext context)
        {
            for(int i=1;i<10;i++)
            {
                var tu = new User
                {
                    Id = i,
                    Name = "u" + i.ToString()
                };
            
                context.Users.Add(tu);

                var ta = new Account
                {
                    Id = i,
                    Name = "a" + i.ToString()
                };
                context.Accounts.Add(ta);

            }
            context.SaveChanges();

            for(int i=1;i<10;i++)
            {
                Account currentAct = context.Accounts.Single( a => a.Id == i);
                User currentUsr = context.Users.Single( u => u.Id == i);
                currentAct.AccountUser = currentUsr;
                currentUsr.UserAccounts.Add(currentAct);

            }
            context.SaveChanges();

            Bank b1 = new Bank();
            b1.Id = 1;
            b1.Name = "Bank of America";
            for(int i=1;i<10;i++)
            {
                User currentUsr = context.Users.Single( u => u.Id == i);
                b1.Users.Add(currentUsr);
            }

        
            
        }
    }
}
