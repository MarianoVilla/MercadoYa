using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercadoYa.Da.MySql;
using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Rest.Mock;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MercadoYa.Rest
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
            string Conn = Configuration.GetConnectionString("MySqlConn");


            services.AddControllers();

            services.Add(new ServiceDescriptor(typeof(IDatabase), new Database(Conn)));
            services.Add(new ServiceDescriptor(typeof(IValidator), new Validator()));
            services.Add(new ServiceDescriptor(typeof(IAuth), new MockAuth(new Database(Conn))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
