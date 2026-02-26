using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.DTO;
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
                .AsNoTracking()
                .Include(t => t.TransactionItems)
                .ToListAsync(cacnelToken);

            return mapper.Map<List<DTO.Transaction>>(transactions);
        }

        public async Task<ListResult<DTO.Transaction>> GetTransactionsPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cacnelToken = default)
        {
            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize <= 0)
                pageSize = 50;

            var query = dbContext.Transactions
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedDate);

            var totalRecords = await query.CountAsync(cacnelToken);

            var transactions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.TransactionItems)
                .ToListAsync(cacnelToken);

            var result = new ListResult<DTO.Transaction>(pageNumber, pageSize)
            {
                TotalRecord = totalRecords,
                Data = mapper.Map<List<DTO.Transaction>>(transactions)
            };

            return result;
        }

        public async Task<DTO.Transaction> GetTransactionAsync(int id, CancellationToken cacnelToken = default)
        {
            var transaction = await dbContext.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cacnelToken);

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
                .AsNoTracking()
                .Include(t => t.ServiceType)
                .Include(t => t.Product)
                .Where(t => t.TranscationId == transactionId)
                .ToListAsync(cacnelToken);

            return mapper.Map<List<DTO.TransactionItem>>(transactionItems);
        }

        public async Task<List<DTO.Transaction>> GetTransactionsByCustomerIdAsync(
            int customerId,
            CancellationToken cacnelToken = default)
        {
            var transactions = await dbContext.Transactions
                .AsNoTracking()
                .Where(t => t.CustomerId == customerId)
                .Include(c => c.Customer)
                .Include(t => t.TransactionItems)
                    .ThenInclude(ti => ti.Product)
                .Include(t => t.TransactionItems)
                    .ThenInclude(ti => ti.ServiceType)
                .ToListAsync(cacnelToken);

            return mapper.Map<List<DTO.Transaction>>(transactions);
        }
        public async Task<DTO.Transaction> GetTransactionsByBookIdAsync(
            int bookId,
            CancellationToken cacnelToken = default)
        {
            var transaction = await dbContext.Transactions
                .AsNoTracking()
                .Where(t => t.BookId == bookId)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(cacnelToken);

            return mapper.Map<DTO.Transaction>(transaction);
        }
    }
}