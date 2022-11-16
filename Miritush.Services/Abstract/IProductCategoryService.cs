using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface IProductCategoryService
    {
        /// <summary>
        /// get List of ProductCategory
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns><see cref="DTO.ProductCategory"/></returns>
        Task<List<DTO.ProductCategory>> GetList(CancellationToken cancelToken = default);
        /// <summary>
        /// Get a Product category by ID
        /// </summary>
        /// <param name="id">Product category ID</param>
        /// <param name="cancelToken"></param>
        /// <returns><see cref="DTO.ProductCategory"/></returns>
        Task<DTO.ProductCategory> GetById(int id, CancellationToken cancelToken = default);

        /// <summary>
        /// Create a new product Category
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancelToken"></param>
        /// <returns>The new <see cref="DTO.ProductCategory"/></returns>
        Task<DTO.ProductCategory> CreateCategory(string name, CancellationToken cancelToken = default);

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task<DTO.ProductCategory> UpdateCategory(
            int id,
            string name,
            CancellationToken cancelToken = default);

        /// <summary>
        /// DeleteCategory()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task DeleteCategory(int id, CancellationToken cancelToken = default);
    }
}