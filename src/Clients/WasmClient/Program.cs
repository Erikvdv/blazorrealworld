
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Components.Authorization;
using Application.Interactors;
using Infrastructure.Clients;
using Blazored.LocalStorage;
using SharedLib.Extensions;
using Microsoft.Extensions.Options;
using Application.Services;

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

            builder.Services.AddHttpClient<IConduitApiService, ConduitApiClient>();
            builder.Services.AddScoped<IArticlesInteractor, ArticlesInteractor>();
            builder.Services.AddScoped<IUserInteractor, UserInteractor>();
            builder.Services.AddScoped<CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
