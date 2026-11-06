using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaSHop.DAL.models
{
    public  class Category
    {
        public int Id { get; set; }
        
        public bool Status { get; set; }
        public List<CategoryTranslation> Translations { get; set; } = new();
        public List<Product> Products { get; set; } = new();

    }
}
