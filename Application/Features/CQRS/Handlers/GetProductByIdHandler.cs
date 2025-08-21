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

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
           var item = await _productRepository.GetById(request.Id);
            return new ProductDto
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
