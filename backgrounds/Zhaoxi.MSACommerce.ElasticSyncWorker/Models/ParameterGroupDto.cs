namespace Zhaoxi.MSACommerce.ElasticSyncWorker.Models;

public record ParameterGroupDto
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public IEnumerable<ParameterKeyDto> ParameterKeys { get; init; }
}