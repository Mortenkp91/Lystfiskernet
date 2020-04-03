using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Configurations;
using Website.Database;

namespace Website
{
    /// <summary>
    /// I Startup.cs klassen har man mulighed for at tilføje diverse ekstra .NET/C# libraries/pakker.
    /// Det er også her man blandt andet kan opsætte en forbindelse til en database.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Her kan man tilføje ting til sin "service container". En service container er en slags liste/kollektioner over services, som man skal/vil bruge rundt omkring i sin applikation.
        // Det kunne fx. være at man skal bruge en database forbindelse til sin SQL Database. For at undgå, at man manuelt skal skrive en connection string til databasen hver gang man skal bruge den,
        // så kan man tilføje en klasse, som kan lave en forbindelse til databasen. Man kalder det for Dependency Injection. Det er et avanceret udtryk. Du kommer ikke rigtig til at bruge det i starten.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddTransient<SqlDatabaseConnectionFactory>();

            // Her tilføjer jeg SqlServerConfiguration klassen til vores service container. Det giver os mulighed for, at vi kan "injecte" klassen andre steder i vores applikation, når vi skal bruge den.
            // Vi skal kun bruge den i SqlDatabaseConnectionFactory klassen, da det er dens ansvar at oprette forbindelser til databasen.
            services.Configure<SqlServerConfiguration>(Configuration.GetSection("ConnectionStrings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
