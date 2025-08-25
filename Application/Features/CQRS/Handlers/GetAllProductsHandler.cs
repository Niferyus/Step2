using Application.Dtos;
using Application.Features.CQRS.Queries;
using Application.Interfaces;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CQRS.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductQuery, List<ProductListDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;
        private const string PRODUCTS_CACHE_KEY = "product:all";

        public GetAllProductsHandler(IProductRepository productRepository, IRedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
        }
        public async Task<List<ProductListDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var cachedProducts = await _redisService.Get<List<ProductListDto>>(PRODUCTS_CACHE_KEY, cancellationToken);
            if (cachedProducts != null && cachedProducts.Count() != 0)
            {
                return cachedProducts;
            }

            var items = await _productRepository.GetAll(cancellationToken);
            var productList = items.Select(p => new ProductListDto
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl[0],
                Stock = p.Stock,
            }).ToList();

            await _redisService.SetAsync(PRODUCTS_CACHE_KEY, productList, TimeSpan.FromMinutes(15), cancellationToken);

            return productList;
        }
    }
}
