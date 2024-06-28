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
const string primaryBackgroundColor = "#131313"; // Dark
const string secondaryBackgroundColor = "#1C1A1C"; // Light dark
const string darkBackgroundColor = "#000000"; //pure black
const string infoBackgroundColor = "#BB86FD"; // light purple

// Buttons color
const string purple = "#A45DE8"; // purple
const string purpleLight = "#BC84F1"; // purple light
const string purpleAccentDark = "#7F39FB"; // purple darker
const string purpleAccentDarkWithOpacity = "#503FCD7F"; // purple darker
const string teal = "#03DAC5"; // teal
const string tealWithAccent = "#5AE3D4"; // teal
const string tealWithDarkAccent = "#00C2AE"; // teal

// Text color
const string primaryTextColor = "#E1E1E1";
const string secondaryTextColor = "#666666";
const string infoTextColor = "#A77AE0";
const string successTextColor = "#6200ED";


var theme = new Theme
{
    // IsGradient = true,
    // IsRounded = true,
    ColorOptions = new ThemeColorOptions
    {
        Primary = purple,
        Secondary = teal,
        Dark = purpleAccentDark,
        Success = purpleAccentDarkWithOpacity,
        Light = purpleLight,
        Info = tealWithAccent,
        Link = tealWithDarkAccent
    },
    BackgroundOptions = new ThemeBackgroundOptions
    {
        Primary = primaryBackgroundColor,
        Secondary = secondaryBackgroundColor,
        Dark = darkBackgroundColor,
        Info = infoBackgroundColor,
    },
    TextColorOptions = new ThemeTextColorOptions
    {
        Primary = primaryTextColor,
        Secondary = secondaryTextColor,
        Info = infoTextColor,
        Success = successTextColor
    },
    BarOptions = new ThemeBarOptions
    {
        DarkColors = new ThemeBarColorOptions
        {
            BackgroundColor = darkBackgroundColor,
            Color = secondaryTextColor,
            ItemColorOptions = new ThemeBarItemColorOptions
            {
                ActiveBackgroundColor = secondaryBackgroundColor,
                HoverBackgroundColor = secondaryBackgroundColor,
                ActiveColor = primaryTextColor,
                HoverColor = primaryTextColor,
            },
            BrandColorOptions = new ThemeBarBrandColorOptions
            {
                BackgroundColor = darkBackgroundColor
            }
        }
    },
    BodyOptions = new ThemeBodyOptions
    {
        BackgroundColor = primaryBackgroundColor
    }
};

builder.Services.AddSingleton(theme);


builder.Services
    .AddBlazorise()
    .AddTailwindProviders()
    .AddFontAwesomeIcons();


await builder.Build().RunAsync();