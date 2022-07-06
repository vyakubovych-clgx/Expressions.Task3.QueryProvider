namespace Expressions.ComplexTask.LinqProvider.Entities;

public record Material : BaseEntity
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Text { get; init; }
    public int CatalogueId { get; init; }
}