using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface IProductService
    {
        /// <summary>
        ///  Get list Products
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns> List of Products</returns>
        Task<List<DTO.Product>> GetList(CancellationToken cancelToken = default);
        /// <summary>
        /// Get Product by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task<DTO.Product> GetById(int id, CancellationToken cancelToken = default);
        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="active"></param>
        /// <param name="cancelToken"></param>
        /// <returns>new products</returns>
        Task<DTO.Product> CreateProduct(
            string name,
            int categoryId,
            string description,
            decimal price,
            bool active = true,
            CancellationToken cancelToken = default);
        /// <summary>
        /// UpdateProduct
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="active"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task<DTO.Product> UpdateProduct(
            int id,
            string name,
            int categoryId,
            string description,
            decimal price,
            bool active = true,
            CancellationToken cancelToken = default);

        /// <summary>
        /// Delete Product 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task DeleteProduct(int id, CancellationToken cancelToken = default);
    }
}