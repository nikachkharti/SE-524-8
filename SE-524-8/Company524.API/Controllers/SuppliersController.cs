using Company524.API.Models.Common;
using Company524.API.Models.Supplier;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Company524.API.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SuppliersController(IProductService productService, ISupplierService supplierService) : ControllerBase
    {
        [HttpGet("{supplierId}/products")]
        public async Task<IActionResult> GetSupplierProducts([FromRoute] Guid supplierId, [FromQuery] PagedRequestDto parameters)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// List All Suppliers
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] PagedRequestDto parameters)
        {
            var result = await supplierService.GetAllSuppliersAsync(parameters);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Get Supplier with Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            var result = await supplierService.GetSupplierByIdAsync(id);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Create a new Supplier
        /// </summary>
        [HttpPost]
        [SwaggerRequestExample(typeof(SupplierForCreatingDto), typeof(SupplierForCreatingDtoExample))]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierForCreatingDto model)
        {
            var result = await supplierService.CreateSupplierAsync(model);
            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = result
            };
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Delete a Supplier
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            await supplierService.DeleteSupplierAsync(id);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK)
            };
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// Update a Supplier
        /// </summary>
        [HttpPut]
        [SwaggerRequestExample(typeof(SupplierForUpdatingDto), typeof(SupplierForUpdatingDtoExample))]
        public async Task<IActionResult> UpdateSupplier([FromBody] SupplierForUpdatingDto model)
        {
            var result = await supplierService.UpdateSupplierAsync(model);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };
            return StatusCode(response.HttpStatusCode, response);
        }

    }
}
