using Company524.Application.Models.Common;
using Company524.Application.Models.Supplier;

namespace Company524.Application.Contracts.Service
{
    public interface ISupplierService
    {
        Task<PagedResponseDto<SupplierForGettingDto>> GetAllSuppliersAsync(PagedRequestDto parameters);
        Task<SupplierForGettingDto> GetSupplierByIdAsync(Guid id);
        Task<SupplierForGettingDto> CreateSupplierAsync(SupplierForCreatingDto model);
        Task<SupplierForGettingDto> UpdateSupplierAsync(SupplierForUpdatingDto model);
        Task DeleteSupplierAsync(Guid id);
    }
}
