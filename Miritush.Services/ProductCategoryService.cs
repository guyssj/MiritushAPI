using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IUserContextService userContext;
        private readonly IMemoryCache memoryCache;

        public ProductCategoryService(
            booksDbContext dbContext,
            IMapper mapper,
            IUserContextService userContext,
            IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userContext = userContext;
            this.memoryCache = memoryCache;
        }

        public async Task<List<DTO.ProductCategory>> GetList(CancellationToken cancelToken = default)
        {
            var cacheKey = $"LIST_PROCUDESCATEGORY_{userContext.Identity.UserId}";

            return await memoryCache.GetOrCreateAsync(cacheKey, async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(5);
                var listProductCategory = await dbContext.ProductCategorys.ToListAsync(cancelToken);
                return mapper.Map<List<DTO.ProductCategory>>(listProductCategory);
            });

        }
        public async Task<DTO.ProductCategory> GetById(int id, CancellationToken cancelToken = default)
        {
            var productCategory = await dbContext.ProductCategorys.FindAsync(id);
            return mapper.Map<DTO.ProductCategory>(productCategory);
        }
        public async Task<DTO.ProductCategory> CreateCategory(string name, CancellationToken cancelToken = default)
        {

            var category = new DAL.Model.ProductCategory
            {
                Name = name
            };

            dbContext.ProductCategorys.Add(category);
            await dbContext.SaveChangesAsync(cancelToken);

            return mapper.Map<DTO.ProductCategory>(category);
        }

        public async Task<DTO.ProductCategory> UpdateCategory(
            int id,
            string name,
            CancellationToken cancelToken = default)
        {

            var category = await dbContext.ProductCategorys.FindAsync(id, cancelToken);

            //update the name
            category.Name = name;

            await dbContext.SaveChangesAsync(cancelToken);

            return mapper.Map<DTO.ProductCategory>(category);
        }


        public async Task DeleteCategory(int id, CancellationToken cancelToken = default)
        {
            var productCategory = await dbContext.ProductCategorys.FindAsync(id, cancelToken);

            dbContext.Remove(productCategory);
            await dbContext.SaveChangesAsync(cancelToken);
        }
    }
}