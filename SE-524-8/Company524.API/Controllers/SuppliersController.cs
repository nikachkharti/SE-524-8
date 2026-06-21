using Company524.API.Exceptions;
using Company524.API.Models.Common;
using Company524.API.Models.Supplier;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Security.Claims;

namespace Company524.API.Controllers
{
    [Route("api/suppliers")]
    [ApiController]
    public class SuppliersController(IProductService productService, ISupplierService supplierService) : ControllerBase
    {
        /// <summary>
        /// პროდუქტები ავტორიზებული Supplier - ის მიხედვით
        /// </summary>
        [HttpGet("{supplierId}/products")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> GetSupplierProducts([FromQuery] PagedRequestDto parameters)
        {
            var supplierId = GetAuthenticatedUserId(User);

            var result = await productService.GetAllProductsOfSupplierAsync(supplierId, parameters);

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
        /// ყველა Supplier
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        /// კონკრეტული Supplier
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
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
        /// ახალი Supplier
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        /// Supplier - ის წაშლა
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
        /// Supplier - ის განახლება
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Admin")]
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



        private static Guid GetAuthenticatedUserId(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("User is not authorized in system");

            return Guid.Parse(userId);
        }
    }
}
