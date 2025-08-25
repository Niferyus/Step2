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
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var item = await _productRepository.GetById(request.Id, cancellationToken);
            if (item == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            await _productRepository.Delete(item, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _redisService.RemoveByPatternAsync("product:*", cancellationToken);
        }
    }
}
