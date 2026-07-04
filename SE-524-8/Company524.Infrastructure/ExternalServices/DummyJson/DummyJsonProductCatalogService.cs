using Company524.Application.Contracts.Service;
using Company524.Application.Exceptions;
using Company524.Application.Models.Common;
using Company524.Application.Models.External;
using System.Net.Http.Json;

namespace Company524.Infrastructure.ExternalServices.DummyJson
{
    // Demo integration: shows how a typed HttpClient (registered via AddHttpClient in Program.cs)
    // is used to call a third-party REST API from behind an Application-layer interface.
    public class DummyJsonProductCatalogService(HttpClient httpClient) : IExternalProductCatalogService
    {
        public async Task<PagedResponseDto<ExternalProductDto>> GetProductsAsync(
            PagedRequestDto parameters,
            CancellationToken cancellationToken = default)
        {
            var skip = BuildSkip(parameters);
            var url = $"products?limit={parameters.PageSize}&skip={skip}{BuildSortQuery(parameters)}";

            var result = await FetchAsync(url, cancellationToken);

            return ToPagedResponse(result, parameters);
        }

        public async Task<PagedResponseDto<ExternalProductDto>> SearchProductsAsync(
            string query,
            PagedRequestDto parameters,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new BadRequestException("Search query is required");

            var skip = BuildSkip(parameters);
            var url = $"products/search?q={Uri.EscapeDataString(query)}&limit={parameters.PageSize}&skip={skip}";

            var result = await FetchAsync(url, cancellationToken);

            return ToPagedResponse(result, parameters);
        }

        private async Task<DummyJsonProductListResponse> FetchAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<DummyJsonProductListResponse>(url, cancellationToken)
                    ?? new DummyJsonProductListResponse();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                throw new InternalServerException($"Failed to reach external product catalog: {ex.Message}");
            }
        }

        private static int BuildSkip(PagedRequestDto parameters) =>
            Math.Max(parameters.PageNumber - 1, 0) * parameters.PageSize;

        private static string BuildSortQuery(PagedRequestDto parameters)
        {
            var sortBy = parameters.SortBy?.Trim().ToLowerInvariant() switch
            {
                "productname" => "title",
                "price" => "price",
                "quantity" => "stock",
                _ => null
            };

            return sortBy is null ? string.Empty : $"&sortBy={sortBy}&order={(parameters.Ascending ? "asc" : "desc")}";
        }

        private static PagedResponseDto<ExternalProductDto> ToPagedResponse(
            DummyJsonProductListResponse response,
            PagedRequestDto parameters) => new()
            {
                Items = response.Products.Select(Map),
                TotalCount = response.Total,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

        private static ExternalProductDto Map(DummyJsonProduct product) => new()
        {
            ExternalId = product.Id,
            ProductName = product.Title,
            Price = product.Price,
            Quantity = product.Stock,
            Category = product.Category,
            Brand = product.Brand
        };
    }
}
