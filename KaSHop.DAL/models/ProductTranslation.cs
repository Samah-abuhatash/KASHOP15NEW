using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaSHop.DAL.models
{
    public class ProductTranslation
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Language { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
