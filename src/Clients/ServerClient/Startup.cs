using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Interactors;
using Application.Clients;
using Infrastructure.Clients;
using SharedLib.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace ServerClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();

            services.Configure<ConduitClientSettings>(Configuration.GetSection(nameof(ConduitClientSettings)));

            services.AddHttpClient<IConduitClient, ConduitClient>();
            services.AddScoped<IArticlesInteractor, ArticlesInteractor>();
            services.AddScoped<IUserInteractor, UserInteractor>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
