using Company524.API.Models.Common;
using Company524.API.Models.Supplier;

namespace Company524.API.Service.Contracts
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
