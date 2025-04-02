namespace MassTransit.Tests.Serialization;

using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MassTransit.Serialization.JsonConverters;
using NUnit.Framework;

[TestFixture]
public class JsonExtensionData_Specs
{
    [DatapointSource] public JsonSerializerOptions[] Values =
    [
        new JsonSerializerOptions(JsonSerializerDefaults.Web),
        new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = {new SystemTextJsonConverterFactory()}
        }
    ];

    [Theory]
    public void Should_be_able_to_serialize_dictionary(JsonSerializerOptions options)
    {
        var obj = new Dictionary<string, object?>();
        obj.Add("Key1", 1);
        obj.Add("Key2", "String");
        obj.Add("Key3", true);

        var result = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(obj, options));

        Assert.That(result, Is.EqualTo("{\"Key1\":1,\"Key2\":\"String\",\"Key3\":true}"));
    }

    [Theory]
    public void Should_be_able_to_deserialize_dictionary(JsonSerializerOptions options)
    {
        var result = JsonSerializer.Deserialize<IDictionary<string, object?>>("{\"Key1\":1,\"Key2\":\"String\",\"Key3\":true}", options);

        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result, Does.ContainKey("Key1"));
        Assert.That(result, Does.ContainKey("Key2"));
        Assert.That(result, Does.ContainKey("Key3"));
    }

    [Theory]
    public void Should_be_able_to_serialize_problem_details_with_json_extensions_data_attribute(JsonSerializerOptions options)
    {
        var obj = new ProblemDetailsWithExtensions();
        obj.Title = "Test";
        obj.Extensions.Add("Key1", 1);
        obj.Extensions.Add("Key2", "String");
        obj.Extensions.Add("Key3", true);


        var result = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(obj, options));

        Assert.That(result, Is.EqualTo("{\"title\":\"Test\",\"Key1\":1,\"Key2\":\"String\",\"Key3\":true}"));
    }


    [Theory]
    public void Should_be_able_to_deserialize_problem_details_with_json_extensions_data_attribute(JsonSerializerOptions options)
    {
        var result = JsonSerializer.Deserialize<ProblemDetailsWithExtensions>("{\"title\":\"Test\",\"Key1\":1,\"Key2\":\"String\",\"Key3\":true}", options);

        Assert.That(result.Title, Is.EqualTo("Test"));
        Assert.That(result.Extensions, Has.Count.EqualTo(3));
        Assert.That(result.Extensions, Does.ContainKey("Key1"));
        Assert.That(result.Extensions, Does.ContainKey("Key2"));
        Assert.That(result.Extensions, Does.ContainKey("Key3"));
    }

    [Theory]
    public void Should_be_able_to_serialize_problem_details_without_json_extensions_data_attribute(JsonSerializerOptions options)
    {
        var obj = new ProblemDetailsWithoutExtensions();
        obj.Title = "Test";
        obj.Extensions.Add("Key1", 1);
        obj.Extensions.Add("Key2", "String");
        obj.Extensions.Add("Key3", true);


        var result = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(obj, options));

        Assert.That(result, Is.EqualTo("{\"title\":\"Test\",\"extensions\":{\"Key1\":1,\"Key2\":\"String\",\"Key3\":true}}"));
    }

    [Theory]
    public void Should_be_able_to_deserialize_problem_details_without_json_extensions_data_attribute(JsonSerializerOptions options)
    {
        var result = JsonSerializer.Deserialize<ProblemDetailsWithoutExtensions>("{\"title\":\"Test\",\"extensions\":{\"Key1\":1,\"Key2\":\"String\",\"Key3\":true}}", options);

        Assert.That(result.Title, Is.EqualTo("Test"));
        Assert.That(result.Extensions, Has.Count.EqualTo(3));
        Assert.That(result.Extensions, Does.ContainKey("Key1"));
        Assert.That(result.Extensions, Does.ContainKey("Key2"));
        Assert.That(result.Extensions, Does.ContainKey("Key3"));
    }

    public class ProblemDetailsWithExtensions
    {
        public string Title { get; set; }
        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
    }

    public class ProblemDetailsWithoutExtensions
    {
        public string Title { get; set; }
        public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
    }
}
