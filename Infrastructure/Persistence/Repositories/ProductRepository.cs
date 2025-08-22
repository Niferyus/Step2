using Application.Interfaces;
using Core.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "products_all";

        public ProductRepository(AppDbContext context, IDistributedCache cache)
            : base(context)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Product?> GetById(int id, CancellationToken ct = default)
            => await _context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task<List<Product>> GetAll(CancellationToken ct = default)
        {
            var cached = await _cache.GetStringAsync(CacheKey, ct);
            if (cached is not null)
            {
                return JsonSerializer.Deserialize<List<Product>>(cached)!;
            }

            var products = await _context.Products.ToListAsync(ct);

            await _cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(products),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                }, ct);

            return products;
        }

        public async Task Create(Product product, CancellationToken ct = default)
        {
            await _context.Products.AddAsync(product, ct);
            await InvalidateCache(ct);
        }

        public async Task Update(Product product, CancellationToken ct = default)
        {
            _context.Products.Update(product);
            await InvalidateCache(ct);
        }

        public async Task Delete(Product product, CancellationToken ct = default)
        {
            _context.Products.Remove(product);
            await InvalidateCache(ct);
        }

        private async Task InvalidateCache(CancellationToken ct = default)
        {
            await _cache.RemoveAsync(CacheKey, ct);
        }
    }
}
