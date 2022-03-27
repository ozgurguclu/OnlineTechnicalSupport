using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }

        [DisplayName("Ürün kategorisi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public virtual ICollection<ProductModel> ProductModels { get; set; }
    }
}