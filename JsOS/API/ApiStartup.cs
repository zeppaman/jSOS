using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Routing;

using Microsoft.Extensions.Logging;

namespace JsOS.API
{
    public class ApiStartup
    {

        public ApiStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            //share same instance
            foreach(var  service in App.Services)
            {
                services.AddSingleton(service.ServiceType,App.ServiceProvider.GetRequiredService(service.ServiceType));
            }
           
        }


        public void Configure(IApplicationBuilder app)
        {
          
            app.UseDeveloperExceptionPage();
           

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