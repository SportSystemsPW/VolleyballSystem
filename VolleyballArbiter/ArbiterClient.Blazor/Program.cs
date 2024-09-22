using ArbiterClient.Blazor;
using ArbiterClient.Blazor.VolleballMatch;
using ArbiterClient.Blazor.VolleballMatch.SpeechRecognition;
using ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddRadzenComponents();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IMatchRepository, MockMatchRepository>();
builder.Services.AddScoped<ICommandMatcher>(_ => new CommandMatcher([
    new UndoPointACommand(),
    new UndoPointBCommand(),
    new PointACommand(),
    new PointBCommand(),
    new BallACommand(),
    new BallBCommand(),
    new BreakACommand(),
    new BreakBCommand()
]));

builder.Services.AddHttpClient("arbiter-api", client => { client.BaseAddress = new Uri("http://localhost:5218"); });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
