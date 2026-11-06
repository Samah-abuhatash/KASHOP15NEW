using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaSHop.DAL.DTO.Requet
{
    public class CategoryRequest
    {
       
        public bool Status { get; set; }
        public List<CategoryTranslationRequest> Translations { get; set; } = new();

    }
}
