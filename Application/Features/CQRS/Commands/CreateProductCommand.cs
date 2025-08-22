using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Application.Dtos;

namespace Application.Features.CQRS.Commands
{
    public class CreateProductCommand : IRequest<CreateProductDto>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<string> ImageUrl { get; set; }
        public string Description { get; set; }
    }
}
