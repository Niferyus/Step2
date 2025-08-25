using Application.Dtos;
using Application.Features.CQRS.Commands;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CQRS.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }
        public async Task<UpdateProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var item = await _productRepository.GetById(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Product not found.");

            item.Name = request.Name;
            item.Price = request.Price;
            item.Stock = request.Stock;
            item.ImageUrl = request.ImageUrl;
            item.Description = request.Description;
            await _productRepository.Update(item, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _redisService.RemoveByPatternAsync("product:*", cancellationToken);

            return new UpdateProductDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Stock = item.Stock,
                ImageUrl = item.ImageUrl,
                Description = item.Description
            };

        }
    }
}
