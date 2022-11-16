using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class ProductService : IProductService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly IUserContextService userContext;

        public ProductService(
            booksDbContext dbContext,
            IMapper mapper,
            IMemoryCache memoryCache,
            IUserContextService userContext)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
            this.userContext = userContext;
        }

        public async Task<List<DTO.Product>> GetList(CancellationToken cancelToken = default)
        {
            var cacheKey = $"LIST_Products_{userContext.Identity.UserId}";

            return await memoryCache.GetOrCreateAsync(cacheKey, async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(5);
                var listProductCategory = await dbContext.Products
                    .Include(p => p.Category)
                    .ToListAsync(cancelToken);
                return mapper.Map<List<DTO.Product>>(listProductCategory);
            });

        }
        public async Task<DTO.Product> GetById(int id, CancellationToken cancelToken = default)
        {
            var product = await dbContext.Products.FindAsync(id);
            return mapper.Map<DTO.Product>(product);
        }
        public async Task<DTO.Product> CreateProduct(
            string name,
            int categoryId,
            string description,
            decimal price,
            bool active = true,
            CancellationToken cancelToken = default)
        {

            var product = new DAL.Model.Product
            {
                Name = name,
                CategoryID = categoryId,
                Description = description,
                Price = price,
                Active = active
            };

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync(cancelToken);

            return mapper.Map<DTO.Product>(product);
        }

        public async Task<DTO.Product> UpdateProduct(
            int id,
            string name,
            int categoryId,
            string description,
            decimal price,
            bool active = true,
            CancellationToken cancelToken = default)
        {

            var product = await dbContext.Products.FindAsync(id, cancelToken);

            //update
            product.Name = name;
            product.CategoryID = categoryId;
            product.Description = description;
            product.Price = price;
            product.Active = active;

            //save the updated data
            await dbContext.SaveChangesAsync(cancelToken);

            return mapper.Map<DTO.Product>(product);
        }


        public async Task DeleteProduct(int id, CancellationToken cancelToken = default)
        {
            var product = await dbContext.Products.FindAsync(id, cancelToken);

            dbContext.Remove(product);
            await dbContext.SaveChangesAsync(cancelToken);
        }
    }
}