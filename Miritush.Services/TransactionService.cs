using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public TransactionService(booksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<DTO.Transaction>> GetTransactionsAsync(CancellationToken cacnelToken = default)
        {
            var transactions = await dbContext.Transactions
                .Include(t => t.TransactionItems)
                .ToListAsync(cacnelToken);

            return mapper.Map<List<DTO.Transaction>>(transactions);
        }

        public async Task<DTO.Transaction> GetTransactionAsync(int id, CancellationToken cacnelToken = default)
        {
            var transaction = await dbContext.Transactions.FindAsync(id, cacnelToken);

            return mapper.Map<DTO.Transaction>(transaction);
        }
        public async Task<DTO.Transaction> CreateTransactionAsync(
            int customerId,
            int? bookId = null,
            CancellationToken cacnelToken = default)
        {

            var transaction = new DAL.Model.Transaction()
            {
                CustomerId = customerId,
                BookId = bookId,
                CreatedDate = DateTime.UtcNow,
                Status = 0 //NoPay
            };

            dbContext.Transactions.Add(transaction);
            await dbContext.SaveChangesAsync(cacnelToken);

            return mapper.Map<DTO.Transaction>(transaction);
        }

        public async Task<DTO.TransactionItem> CreateTransactionItemAsync(
            int transactionId,
            decimal? price,
            int quantity,
            int? serviceTypeId = null,
            int? productId = null,
            CancellationToken cacnelToken = default)
        {
            var transactionItem = new DAL.Model.TransactionItem()
            {
                ProductId = productId,
                ServiceTypeId = serviceTypeId,
                Quantity = quantity,
                Price = price,
                TranscationId = transactionId
            };
            dbContext.TransactionItems.Add(transactionItem);
            await dbContext.SaveChangesAsync(cacnelToken);
            return mapper.Map<DTO.TransactionItem>(transactionItem);
        }

        public async Task<List<DTO.TransactionItem>> CreateTransactionItemsAsync(
            List<DTO.CreateTransactionItemData> items,
            CancellationToken cacnelToken = default)
        {

            //change to mapper
            var transactionItems = items.Select(item => new DAL.Model.TransactionItem()
            {
                ProductId = item.ProductId,
                ServiceTypeId = item.ServiceTypeId,
                Quantity = item.Quantity,
                Price = item.Price,
                TranscationId = item.TransactionId
            });
            dbContext.TransactionItems.AddRange(transactionItems);
            await dbContext.SaveChangesAsync(cacnelToken);
            return mapper.Map<List<DTO.TransactionItem>>(transactionItems);
        }

        public async Task<List<DTO.TransactionItem>> GetTransactionItemByTransactionIdAsync(
            int transactionId,
            CancellationToken cacnelToken = default)
        {
            var transactionItems = await dbContext.TransactionItems
                .Include(t => t.ServiceType)
                .Include(t => t.Product)
                .Where(t => t.TranscationId == transactionId)
                .ToListAsync();

            return mapper.Map<List<DTO.TransactionItem>>(transactionItems);
        }

        public async Task<List<DTO.Transaction>> GetTransactionsByCustomerIdAsync(
            int customerId,
            CancellationToken cacnelToken = default)
        {
            var transactions = await dbContext.Transactions
                .Where(t => t.CustomerId == customerId)
                .Include(c => c.Customer)
                .Include(t => t.TransactionItems)
                    .ThenInclude(ti => ti.Product)
                .Include(t => t.TransactionItems)
                    .ThenInclude(ti => ti.ServiceType)
                .ToListAsync();

            return mapper.Map<List<DTO.Transaction>>(transactions);
        }
        public async Task<DTO.Transaction> GetTransactionsByBookIdAsync(
            int bookId,
            CancellationToken cacnelToken = default)
        {
            var transaction = await dbContext.Transactions
                .Where(t => t.BookId == bookId)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync();

            return mapper.Map<DTO.Transaction>(transaction);
        }
    }
}