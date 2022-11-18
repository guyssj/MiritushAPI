using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miritush.DTO.Const;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public TransactionController()
        {

        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<List<DTO.ProductCategory>> GetProductCategorys(CancellationToken cancelToken = default)
        {
            return await productCategoryService.GetList(cancelToken);
        }


    }
}