using Newtonsoft.Json;

namespace Zhaoxi.MSACommerce.OrderService.UseCases;

public class NonEscapedStringConverter : JsonConverter
{
    /// <summary>
    /// 判断是否可以转换指定的对象类型
    /// </summary>
    /// <param name="objectType">要转换的对象类型</param>
    /// <returns>如果可以转换返回true，否则返回false</returns>
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }

    /// <summary>
    /// 将对象值转换为JSON格式并写入到JsonWriter中
    /// </summary>
    /// <param name="writer">用于写入JSON的JsonWriter</param>
    /// <param name="value">要转换的对象值</param>
    /// <param name="serializer">用于序列化的JsonSerializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // 直接写入字符串而不转义
        writer.WriteRawValue(value.ToString());
    }
    
    /// <summary>
    /// 从JsonReader中读取JSON格式的数据并转换为指定的对象类型
    /// </summary>
    /// <param name="reader">用于读取JSON的JsonReader</param>
    /// <param name="objectType">要转换的对象类型</param>
    /// <param name="existingValue">现有的对象值（如果有的话）</param>
    /// <param name="serializer">用于反序列化的JsonSerializer</param>
    /// <returns>转换后的对象值</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return reader.Value.ToString();
    }
}