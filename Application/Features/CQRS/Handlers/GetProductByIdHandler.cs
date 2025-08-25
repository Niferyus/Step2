using Application.Dtos;
using Application.Features.CQRS.Queries;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CQRS.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;

        public GetProductByIdHandler(IProductRepository productRepository, IRedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"product:id:{request.Id}";

            // Cache'den kontrol et
            var cachedProduct = await _redisService.Get<ProductDto>(cacheKey, cancellationToken);
            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            // Cache'de yoksa veritabanından çek
            var item = await _productRepository.GetById(request.Id, cancellationToken);
            var productDto = new ProductDto
            {
                Name = item.Name,
                Price = item.Price,
                Stock = item.Stock,
                ImageUrl = item.ImageUrl,
                Description = item.Description
            };

            // Cache'e kaydet (30 dakika)
            await _redisService.SetAsync(cacheKey, productDto, TimeSpan.FromMinutes(30), cancellationToken);

            return productDto;
        }
    }
}
