using Company524.Application.Models.Common;
using Company524.Application.Models.External;

namespace Company524.Application.Contracts.Service
{
    public interface IExternalProductCatalogService
    {
        Task<PagedResponseDto<ExternalProductDto>> GetProductsAsync(
            PagedRequestDto parameters,
            CancellationToken cancellationToken = default);

        Task<PagedResponseDto<ExternalProductDto>> SearchProductsAsync(
            string query,
            PagedRequestDto parameters,
            CancellationToken cancellationToken = default);
    }
}
