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

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var item = await _productRepository.GetById(request.Id, cancellationToken);
            if (item == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            await _productRepository.Delete(item, cancellationToken);
        }
    }
}
