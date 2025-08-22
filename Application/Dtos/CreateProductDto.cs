using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<string> ImageUrl { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
    }
}
