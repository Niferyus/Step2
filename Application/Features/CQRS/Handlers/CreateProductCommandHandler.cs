using Application.Dtos;
using Application.Features.CQRS.Commands;
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
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var item = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                ImageUrl = request.ImageUrl,
                Description = request.Description
            };

            await _productRepository.Create(item, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _redisService.RemoveByPatternAsync("product:*", cancellationToken);

            return new CreateProductDto
            {
                Name = item.Name,
                Price = item.Price,
                Stock = item.Stock,
                ImageUrl = item.ImageUrl,
                Description = item.Description
            };
        }
    }
}
