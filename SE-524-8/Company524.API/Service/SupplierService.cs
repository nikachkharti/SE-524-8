using Company524.API.Entities;
using Company524.API.Exceptions;
using Company524.API.Models.Common;
using Company524.API.Models.Supplier;
using Company524.API.Repository.Contracts;
using Company524.API.Service.Contracts;
using MapsterMapper;
using System.Linq.Expressions;

namespace Company524.API.Service
{
    public class SupplierService(ISupplierRepository supplierRepository, IMapper mapper) : ISupplierService
    {
        public async Task<SupplierForGettingDto> CreateSupplierAsync(SupplierForCreatingDto model)
        {
            if (model is null)
                throw new BadRequestException($"Request model is required");

            if (model.SupplierName.Length > 100)
                throw new BadRequestException("Supplier name length can't exceed 100");

            var supplier = mapper.Map<Supplier>(model);
            await supplierRepository.AddAsync(supplier);
            await supplierRepository.SaveAsync();
            return mapper.Map<SupplierForGettingDto>(supplier);
        }

        public async Task DeleteSupplierAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Supplier id is required");

            var supplier = await supplierRepository.GetAsync(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            supplierRepository.Remove(supplier);
            await supplierRepository.SaveAsync();
        }

        public async Task<PagedResponseDto<SupplierForGettingDto>> GetAllSuppliersAsync(PagedRequestDto parameters)
        {
            Expression<Func<Supplier, object>> orderBy = parameters.SortBy?.ToLower() switch
            {
                "suppliername" => p => p.SupplierName,
                _ => p => p.Id
            };

            var suppliers = await supplierRepository.GetAllAsync(
                orderBy: orderBy,
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            if (!suppliers.Items.Any())
                return new PagedResponseDto<SupplierForGettingDto>
                {
                    Items = Enumerable.Empty<SupplierForGettingDto>(),
                    TotalCount = 0,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };

            var result = mapper.Map<IEnumerable<SupplierForGettingDto>>(suppliers.Items);
            return new PagedResponseDto<SupplierForGettingDto>
            {
                Items = result,
                TotalCount = suppliers.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<SupplierForGettingDto> GetSupplierByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Supplier id is required");

            var supplier = await supplierRepository.GetAsync(s => s.Id == id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            return mapper.Map<SupplierForGettingDto>(supplier);
        }

        public async Task<SupplierForGettingDto> UpdateSupplierAsync(SupplierForUpdatingDto model)
        {
            if (model is null)
                throw new BadRequestException($"Request model is required");

            if (model.Id == Guid.Empty)
                throw new BadRequestException("Supplier id is required");

            if (model.SupplierName.Length > 100)
                throw new BadRequestException("Supplier name length can't exceed 100");

            var supplier = await supplierRepository.GetAsync(s => s.Id == model.Id);

            if (supplier is null)
                throw new NotFoundException("Supplier not found");

            mapper.Map(model, supplier);
            supplierRepository.Update(supplier);
            await supplierRepository.SaveAsync();

            return mapper.Map<SupplierForGettingDto>(supplier);
        }
    }
}
