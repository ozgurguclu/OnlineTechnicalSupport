using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class AssetWarrantyDetail
    {
        [Key]
        public int AssetWarrantyDetailId { get; set; }

        [DisplayName("Garanti Tipi")]
        [Required(ErrorMessage = "* Zorunlu alan")]
        public string WarrantyType { get; set; }

        [DisplayName("Garanti süresi")]
        [Required(ErrorMessage = "* Zorunlu alan")]
        public int WarrantyYears { get; set; }

        [DisplayName("Garanti başlangıç tarihi")]
        [Required(ErrorMessage = "* Zorunlu alan")]
        public DateTime WarrantyStartDate { get; set; }

        [DisplayName("Garanti bitiş tarihi")]
        public DateTime WarrantyEndDate { get; set; }

        [DisplayName("Ürün")]
        [Required(ErrorMessage = "* Ürün seçilmedi.")]
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
    }
}