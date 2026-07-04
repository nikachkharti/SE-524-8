using System.Text.Json.Serialization;

namespace Company524.Infrastructure.ExternalServices.DummyJson
{
    internal sealed class DummyJsonProductListResponse
    {
        [JsonPropertyName("products")]
        public List<DummyJsonProduct> Products { get; set; } = [];

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("skip")]
        public int Skip { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }
    }

    internal sealed class DummyJsonProduct
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }
    }
}
