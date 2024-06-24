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
        Primary = "#413A9A",
        Secondary = "#313056",
        Success = "#FF7A79",
        Info = "#7885FF",
        Danger = "#D21D04",
        Dark = "#313056",
    },
    BackgroundOptions = new ThemeBackgroundOptions
    {
        Primary = "#42437E",
        Secondary = "#37385F",
        Success = "#FF7A79",
        Info = "#7885FF",
        Danger = "#D21D04",
        Dark = "#2F2E48",
    },
    TextColorOptions = new ThemeTextColorOptions
    {
        Primary = "#F9F9FC",
        Dark = "#9E9EB1"
    }
};

builder.Services.AddSingleton(theme);


builder.Services
    .AddBlazorise()
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


await builder.Build().RunAsync();