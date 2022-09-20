namespace Blazor.MVVM.ExampleComponents.Components.Tables {
    public record PageResponse<TItem>(IEnumerable<TItem> Items, int TotalItems);
}