using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var theme = new Theme
{
    ColorOptions = new ThemeColorOptions
    {
        Primary = "#3498db",
        Secondary = "#2ecc71",
        Success = "#1abc9c",
        Danger = "#e74c3c"
    },
    BackgroundOptions = new ThemeBackgroundOptions
    {
        Primary = "#3498db"
    },
    TextColorOptions = new ThemeTextColorOptions
    {
        Primary = "#ffffff"
    }
};


builder.Services
    .AddBlazorise(
        options => { options.Immediate = true; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddSingleton(theme);

await builder.Build().RunAsync();