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

        public GetAllProductsHandler(IProductRepository productRepository) 
        { 
            _productRepository = productRepository;
        }
        public async Task<List<ProductListDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var items = await _productRepository.GetAll();
            return items.Select(p => new ProductListDto
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl[0],
                Stock = p.Stock,
            }).ToList();
        }
    }
}
