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

        public ProductRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Product?> GetById(int id, CancellationToken ct = default)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        }
            
        public async Task<List<Product>> GetAll(CancellationToken ct = default)
        {
            var products = await _context.Products.ToListAsync(ct);
            return products;
        }

        public async Task Create(Product product, CancellationToken ct = default)
        {
            await _context.Products.AddAsync(product, ct);
        }

        public async Task Update(Product product, CancellationToken ct = default)
        {
            _context.Products.Update(product);
        }

        public async Task Delete(Product product, CancellationToken ct = default)
        {
            _context.Products.Remove(product);
        }
    }
}
