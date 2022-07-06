namespace Expressions.ComplexTask.LinqProvider.Entities;

public record Assessment : BaseEntity
{
    public int Id { get; init; }
    public int MaterialId { get; init; }
    public int Rating { get; init; }
}