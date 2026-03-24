using System.Text.Json.Serialization;
using Zhaoxi.MSACommerce.SharedKernel.Domain;

namespace Zhaoxi.MSACommerce.SearchService.Core.Entities;

public class Product : BaseEntity<long>
{
    // 所有需要被搜索的信息，包括品牌，分类，标题
    public string All { get; set; } = null!;
    
    // 描述
    public string? Description { get; set; }
    public long BrandId { get; set; }
    public long[] CategoryId { get; set; }
    
    // 所有sku的价格集合。方便根据价格进行筛选过滤
    public HashSet<double> Price { get; set; } = [];

    // sku信息的json结构数据
    public string Skus { get; set; } = null!;  
    
    // 规格，key是规格名，value是规格值
    public Dictionary<string, object> Specs { get; set; } = new();
}