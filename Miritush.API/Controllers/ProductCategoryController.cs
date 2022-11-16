using System;
using System.Collections.Generic;
using System.Linq;
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
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            this.productCategoryService = productCategoryService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<List<DTO.ProductCategory>> GetProductCategorys(CancellationToken cancelToken = default)
        {
            return await productCategoryService.GetList(cancelToken);
        }

        [HttpGet("{productCategoryId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.ProductCategory> GetProductCategory(
            [FromRoute] int productCategoryId,
            CancellationToken cancelToken = default)
        {
            return await productCategoryService.GetById(productCategoryId, cancelToken);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.ProductCategory> AddProductCategory(
            [FromBody] CreateProductCategoryData data,
            CancellationToken cancelToken = default)
        {
            return await productCategoryService.CreateCategory(data.Name, cancelToken);
        }
        [HttpPost("{productCategoryId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.ProductCategory> UpdateCategory(
            [FromBody] CreateProductCategoryData data,
            [FromRoute] int productCategoryId,
            CancellationToken cancelToken = default)
        {
            return await productCategoryService.UpdateCategory(productCategoryId, data.Name, cancelToken);
        }

        [HttpDelete("{productCategoryId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateCategory(
            [FromRoute] int productCategoryId,
            CancellationToken cancelToken = default)
        {
            await productCategoryService.DeleteCategory(productCategoryId, cancelToken);
            return Ok();
        }

    }
}