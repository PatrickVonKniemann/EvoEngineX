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


const string primaryColor = "#402E7A";
const string secondaryColor = "#4C3BCF";
const string infoColor = "#4B70F5";
const string linkColor = "#3DC2EC";
const string lightColor = "#8173AF";
const string darkColor = "#150742";
var theme = new Theme
{
    ButtonOptions = new ThemeButtonOptions
    {
        Padding = "0.5rem 1rem",
        Margin = "0.5rem",
        BoxShadowSize = "4px 4px 8px",
        BoxShadowTransparency = 127,
        HoverDarkenColor = 5f, // Slight darken effect on hover
        HoverLightenColor = 10f, // Slight lighten effect on hover
        ActiveDarkenColor = 10f, // Darken effect on active
        ActiveLightenColor = 15f, // Lighten effect on active
        LargeBorderRadius = ".3rem",
        SmallBorderRadius = ".2rem",
        GradientBlendPercentage = 25f, // More pronounced gradient for a better visual effect
        DisabledOpacity = 0.65f,
        Size = Size.Medium,
    },
    ColorOptions = new ThemeColorOptions
    {
        Primary = primaryColor,
        Secondary = secondaryColor,
        Success = "#CA5EFF",
        Info = infoColor,
        Link = linkColor,
        Danger = "#D21D04",
        Dark = darkColor,
        Light = lightColor,
    },
    BackgroundOptions = new ThemeBackgroundOptions
    {
        Primary = primaryColor,
        Secondary = secondaryColor,
        Success = "#FF7A79",
        Info = infoColor,
        Danger = "#D21D04",
        Dark = darkColor,
        Light = lightColor,
    },
    TextColorOptions = new ThemeTextColorOptions
    {
        Primary = "#FEFCFD",
        Secondary = "#D2D1D6",
    }
};

builder.Services.AddSingleton(theme);


builder.Services
    .AddBlazorise()
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


await builder.Build().RunAsync();