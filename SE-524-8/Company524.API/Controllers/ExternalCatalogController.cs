using Company524.Application.Contracts.Service;
using Company524.Application.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Company524.API.Controllers
{
    /// <summary>
    /// HttpClient-ის გამოყენების დემონსტრაციისთვის: მონაცემებს იღებს გარე საჯარო API-დან (dummyjson.com)
    /// </summary>
    [Route("api/external-products")]
    [ApiController]
    public class ExternalCatalogController(IExternalProductCatalogService externalProductCatalogService) : ControllerBase
    {
        /// <summary>
        /// გარე API-დან პროდუქტების გვერდობრივი ჩამონათვალის აღება
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] PagedRequestDto parameters, CancellationToken cancellationToken)
        {
            var result = await externalProductCatalogService.GetProductsAsync(parameters, cancellationToken);
            var response = new CommonResponse(CommonResponseMessage.SuccessMessage, result, true, Convert.ToInt32(HttpStatusCode.OK));

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// გარე API-ში პროდუქტების ძებნა საკვანძო სიტყვით
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string query,
            [FromQuery] PagedRequestDto parameters,
            CancellationToken cancellationToken)
        {
            var result = await externalProductCatalogService.SearchProductsAsync(query, parameters, cancellationToken);
            var response = new CommonResponse(CommonResponseMessage.SuccessMessage, result, true, Convert.ToInt32(HttpStatusCode.OK));

            return StatusCode(response.HttpStatusCode, response);
        }
    }
}
