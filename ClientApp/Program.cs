using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tailwind;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// Background
const string primaryColor = "#131313"; // Dark
const string secondaryColor = "#1C1A1C"; // Light dark
const string darkColor = "#000000"; //pure black
const string infoColor = "#BB86FD"; // light purple

// Buttons color
const string primaryButtonColor = "#A45DE8"; // purple
const string primaryButtonAccentColor = "#7F39FB"; // purple darker
const string secondaryButtonColor = "#03DAC5"; // teal

var theme = new Theme
{
    ColorOptions = new ThemeColorOptions
    {
        Primary = primaryButtonColor,
        Secondary = secondaryButtonColor,
        Dark = primaryButtonAccentColor,
    },
    BackgroundOptions = new ThemeBackgroundOptions
    {
        Primary = primaryColor,
        Secondary = secondaryColor,
        Dark = darkColor,
        Info = infoColor,
    },
    TextColorOptions = new ThemeTextColorOptions
    {
        Primary = "#E1E1E1",
        Secondary = "#666666",
        Info = "#A77AE0",
        Success = "#6200ED",
    }
};

builder.Services.AddSingleton(theme);


builder.Services
    .AddBlazorise()
    .AddTailwindProviders()
    // .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


await builder.Build().RunAsync();