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
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var item = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                ImageUrl = request.ImageUrl,
                Description = request.Description
            };

            _productRepository.Create(item);
            // save changes to the database

        }
    }
}
