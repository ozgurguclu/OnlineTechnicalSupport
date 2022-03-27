using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class ProductModel
    {
        [Key]
        public int ProductModelId { get; set; }

        [DisplayName("Ürün modeli")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayName("Ürün kategorisi")]
        [Required(ErrorMessage = "Ürün kategorisini belirleyiniz.")]
        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}