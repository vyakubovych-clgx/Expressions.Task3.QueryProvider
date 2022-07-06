namespace Expressions.ComplexTask.LinqProvider.Entities;

public record Catalogue : BaseEntity
{
    public int Id { get; init; }
    public string Name { get; init; }
}