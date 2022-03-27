using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Product
    {
        [Key]
        [DisplayName("Ürün kayıt no")]
        public int ProductId { get; set; }

        [DisplayName("Ürün adı")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayName("Ürün etiketi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "* 20 karakter uzunluğunda olmalıdır.")]
        public string Tag { get; set; }

        [DisplayName("Ürün resmi")]
        [Required(ErrorMessage = "* Ürüne ait bir resim yükleyiniz.")]
        public string Image { get; set; }

        [DisplayName("Ürün modeli")]
        [Required(ErrorMessage = "* Ürün modeli belirleyiniz.")]
        public int ProductModelId { get; set; }
        public virtual ProductModel ProductModel { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}