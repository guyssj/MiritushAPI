using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ITransactionService
    {
        /// <summary>
        /// Get all transcations
        /// </summary>
        /// <param name="cacnelToken"></param>
        /// <returns></returns>
        Task<List<DTO.Transaction>> GetTransactionsAsync(CancellationToken cacnelToken = default);
        /// <summary>
        /// get transaction by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cacnelToken"></param>
        /// <returns></returns>
        Task<DTO.Transaction> GetTransactionAsync(int id, CancellationToken cacnelToken = default);
        /// <summary>
        /// Create a new transaction for customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="status"></param>
        /// <param name="cacnelToken"></param>
        /// <returns>new transaction </returns>
        Task<DTO.Transaction> CreateTransactionAsync(
            int customerId,
            CancellationToken cacnelToken = default);
        /// <summary>
        /// Create Transaction item to transaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="serviceTypeId"></param>
        /// <param name="productId"></param>
        /// <param name="cacnelToken"></param>
        /// <returns></returns>
        Task<DTO.TransactionItem> CreateTransactionItemAsync(
            int transactionId,
            decimal? price,
            int quantity,
            int? serviceTypeId = null,
            int? productId = null,
            CancellationToken cacnelToken = default);
        /// <summary>
        /// Get all items for transaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cacnelToken"></param>
        /// <returns></returns>
        Task<List<DTO.TransactionItem>> GetTransactionItemByTransactionIdAsync(
            int transactionId,
            CancellationToken cacnelToken = default);

        /// <summary>
        ///  Get Transactions by customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cacnelToken"></param>
        /// <returns></returns>
        Task<List<DTO.Transaction>> GetTransactionsByCustomerIdAsync(
            int customerId,
            CancellationToken cacnelToken = default);
    }
}