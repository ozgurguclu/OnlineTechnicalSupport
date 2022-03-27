using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class IssueReport
    {
        [Key]
        [DisplayName("Kayıt no")]
        public int IssueReportId { get; set; }

        [DisplayName("Eposta")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "* Eposta yazımına dikkat ediniz.")]
        public string Email { get; set; }

        [DisplayName("Başlık")]
        [Required(ErrorMessage = "* Sorun hakkında bir başlık belirleyiniz.")]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        [DisplayName("Açıklama")]
        [Required(ErrorMessage = "* Sorun hakkında bir açıklama yapınız.")]
        [StringLength(200, MinimumLength = 10)]
        public string Description { get; set; }

        [DisplayName("Ek dosya")]
        public string AttachedImage { get; set; }

        [DisplayName("Oluşturma tarihi")]
        public DateTime DateOfCreate { get; set; }

        [DisplayName("Ip adresi")]
        public string IpAddress { get; set; }

        [DisplayName("Takip kodu")]
        public string Tag { get; set; }

        [DisplayName("Cevap")]
        public string Reply { get; set; }
        [DisplayName("Cevap veren")]
        public string RepliedBy { get; set; }
        [DisplayName("Cevap tarihi")]
        public Nullable<DateTime> DateOfReply { get; set; }

        [DisplayName("Düzenleyen")]
        public string EditedBy { get; set; }
        [DisplayName("Düzenleme tarihi")]
        public Nullable<DateTime> DateOfEdit { get; set; }

        [DisplayName("Sorun türü")]
        [Required(ErrorMessage = "* Sorun türünü belirleyiniz.")]
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }

        [DisplayName("Ürün")]
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        [DisplayName("Başvuran hesap")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [DisplayName("Yöneten hesap")]
        public Nullable<int> ManagerId { get; set; }
        public virtual Manager Manager { get; set; }

        [DisplayName("Durum")]
        public int IssueReportStatusId { get; set; }
        public virtual IssueReportStatus IssueReportStatus { get; set; }
    }
}