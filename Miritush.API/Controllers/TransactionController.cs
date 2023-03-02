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
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<List<DTO.Transaction>> GetTransactionsAsync(CancellationToken cancelToken = default)
        {
            return await transactionService.GetTransactionsAsync(cancelToken);
        }

        [HttpGet("{transactionId:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.Transaction> GetTransactionAsync(
            int transactionId,
            CancellationToken cancelToken = default)
        {
            return await transactionService.GetTransactionAsync(transactionId, cancelToken);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.Transaction> CreateTransaction(
            [FromBody] CreateTransactionData data,
            CancellationToken cancelToken = default)
        {
            return await transactionService.CreateTransactionAsync(data.CustomerId, data.BookId, cancelToken);
        }
        [HttpPost("item")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<DTO.TransactionItem> CreateTransactionItemAsync(
            [FromBody] DTO.CreateTransactionItemData data,
            CancellationToken cancelToken = default)
        {
            return await transactionService.CreateTransactionItemAsync(
                data.TransactionId,
                data.Price,
                data.Quantity,
                data.ServiceTypeId,
                data.ProductId,
                cancelToken);
        }
        [HttpPost("items")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<List<DTO.TransactionItem>> CreateTransactionItemsAsync(
            [FromBody] List<DTO.CreateTransactionItemData> items,
            CancellationToken cancelToken = default)
        {
            return await transactionService.CreateTransactionItemsAsync(items, cancelToken);
        }

        [HttpGet("{transactionId}/items")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<List<DTO.TransactionItem>> GetTransactionItemsByTransactionIdAsync(
            int transactionId,
            CancellationToken cancelToken = default)
        {
            return await transactionService.GetTransactionItemByTransactionIdAsync(
                transactionId,
                cancelToken);
        }
    }
}