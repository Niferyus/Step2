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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var item = await _productRepository.GetById(request.Id)
                ?? throw new KeyNotFoundException("Product not found.");

            item.Name = request.Name;
            item.Price = request.Price;
            item.Stock = request.Stock;
            item.ImageUrl = request.ImageUrl;
            item.Description = request.Description;
            _productRepository.Update(item);
            // save changes to the database
        }
    }
}
