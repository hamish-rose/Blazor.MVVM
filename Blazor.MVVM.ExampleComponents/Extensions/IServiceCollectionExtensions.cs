using Blazor.MVVM.ExampleComponents.Components.Modal;
using Blazor.MVVM.ExampleComponents.Services;
using Blazor.MVVM.Lib.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.MVVM.ExampleComponents.Extensions;

/// <summary>
/// Service collection extension methods
/// </summary>
// ReSharper disable once InconsistentNaming -- extension methods on interface.
public static class IServiceCollectionExtensions {

    public static IServiceCollection AddComponentsFromLib(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddViewModelsFromAssembly(typeof(IServiceCollectionExtensions).Assembly);

        return serviceCollection;
    }
    
    public static WebAssemblyHostBuilder AddModals(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<ModalHost>("body::after");
        return builder;
    }
}