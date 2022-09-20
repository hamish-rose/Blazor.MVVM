using System.Runtime.CompilerServices;
using Blazor.MVVM.ExampleComponents.Components.Tables;
using Blazor.MVVM.Lib.ViewModels;
using Blazor.MVVM.Models;

namespace Blazor.MVVM.ViewModels;

public class TablesPageViewModel : ComponentViewModelBase
{
    private Random Random { get; } = new Random();
    
    public List<Tick> Ticks { get; } = Enumerable.Range(0, 10000)
        .Select(x => new Tick { Name= Guid.NewGuid().ToString(), Price = x, Timestamp = DateTimeOffset.UtcNow.DateTime})
        .ToList();

    public Task<PageResponse<Tick>> GetTickPageAsync(PageRequest pageRequest)
    {
        return Task.FromResult(new PageResponse<Tick>(Ticks.Skip(pageRequest.Index).Take(pageRequest.Count),
            Ticks.Count));
    }
    
    public async IAsyncEnumerable<Tick> GetTickFeedAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        Random random = new();

        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(random.Next(1, 25), ct);

            yield return new Tick {Name = Guid.NewGuid().ToString(), Price = random.Next(), Timestamp = DateTimeOffset.UtcNow.DateTime};
        }
    }
}