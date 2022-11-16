using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Model;
using Miritush.DTO.Const;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<List<DTO.Product>> GetProducts(CancellationToken cancelToken = default)
        {
            return await productService.GetList(cancelToken);
        }

        [HttpGet("{productId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.Product> GetProduct(
            [FromRoute] int productId,
            CancellationToken cancelToken = default)
        {
            return await productService.GetById(productId, cancelToken);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.Product> AddProduct(
            [FromBody] CreateProductData data,
            CancellationToken cancelToken = default)
        {
            return await productService.CreateProduct(
                data.Name,
                data.CategoryID,
                data.Description,
                data.Price,
                data.Active,
                cancelToken);
        }
        [HttpPost("{productId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.Product> UpdateProduct(
            [FromBody] CreateProductData data,
            [FromRoute] int productId,
            CancellationToken cancelToken = default)
        {
            return await productService.UpdateProduct(
                productId,
                data.Name,
                data.CategoryID,
                data.Description,
                data.Price,
                data.Active,
                cancelToken);
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteProduct(
            [FromRoute] int productId,
            CancellationToken cancelToken = default)
        {
            await productService.DeleteProduct(productId, cancelToken);
            return Ok();
        }
    }
}