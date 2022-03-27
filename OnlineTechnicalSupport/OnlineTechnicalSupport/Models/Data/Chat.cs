using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [DisplayName("Başlık")]
        [Required(ErrorMessage = "* Sorun hakkında bir başlık belirleyiniz.")]
        [StringLength(70, MinimumLength = 10, ErrorMessage = "* En az 10 karakter uzunluğunda olmalıdır.")]
        public string Title { get; set; }

        [DisplayName("Kod")]
        public string Tag { get; set; }

        [DisplayName("Oluşturma tarihi")]
        public DateTime DateOfCreate { get; set; }

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
        public int ChatStatusId { get; set; }
        public virtual ChatStatus ChatStatus { get; set; }

        public virtual ICollection<TextMessage> TextMessages { get; set; }
    }
}