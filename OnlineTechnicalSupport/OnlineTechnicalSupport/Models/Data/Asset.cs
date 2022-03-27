using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Asset
    {
        [Key]
        [DisplayName("Cihaz kayıt no")]
        public int AssetId { get; set; }

        [DisplayName("Ürün kimliği")]
        [Required(ErrorMessage = "Ürün seri numarasını giriniz.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "14 karakter uzunluğunda olmalıdır.")]
        public string AssetTag { get; set; }

        [DisplayName("Servis kimliği")]
        [Required(ErrorMessage = "Servis kimliğini giriniz.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "10 karakter uzunluğunda olmalıdır.")]
        public string ServiceTag { get; set; }

        [DisplayName("Satın alma tarihi")]
        public Nullable<DateTime> PurchaseDate { get; set; }

        [DisplayName("Ürün kayıt no")]
        [Required(ErrorMessage = "Ürün seçilmedi.")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public virtual ICollection<AssetWarrantyDetail> AssetWarrantyDetails { get; set; }
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
        public virtual ICollection<IssueReport> IssueReports { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}