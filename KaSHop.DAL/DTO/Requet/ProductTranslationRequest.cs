using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaSHop.DAL.DTO.Requet
{
    public class ProductTranslationRequest
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Language { get; set; } = null!;
    }
}


