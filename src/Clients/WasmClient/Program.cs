
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Components.Authorization;
using Application.Interactors;
using Application.Clients;
using Infrastructure.Clients;
using Blazored.LocalStorage;
using SharedLib.Extensions;
using Microsoft.Extensions.Options;

namespace WasmClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.Configure<ConduitClientSettings>(options => options.BaseAddress = "https://conduit.productionready.io");

            builder.Services.AddHttpClient<IConduitClient, ConduitClient>();
            builder.Services.AddScoped<IArticlesInteractor, ArticlesInteractor>();
            builder.Services.AddScoped<IUserInteractor, UserInteractor>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
