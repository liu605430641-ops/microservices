namespace Zhaoxi.MSACommerce.CategoryService.UseCases;

public record CategoryDto(long Id, string Name);

public record SpecKeyDto(long Id, string Name);

public record ParameterKeyDto(long Id, string Name);

public record ParameterGroupDto
{
    public long                         Id               { get; init; }
    public string                       Name             { get; init; } = null!;
    public IEnumerable<ParameterKeyDto> ParameterKeysDto { get; init; }
}