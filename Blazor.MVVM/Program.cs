using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.MVVM;
using Blazor.MVVM.ExampleComponents.Extensions;
using Blazor.MVVM.Lib.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.AddModals();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Add the components from MVVM lib
builder.Services.AddComponentsFromLib();

//Use MVVM lib helper to auto register view models in this assembly.
builder.Services.AddViewModelsFromAssembly(typeof(Program).Assembly);

await builder.Build().RunAsync();