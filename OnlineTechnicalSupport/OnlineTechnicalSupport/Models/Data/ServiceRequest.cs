using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class ServiceRequest
    {
        [Key]
        [DisplayName("Kayıt no")]
        public int ServiceRequestId { get; set; }

        [DisplayName("Müşteri adı")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayName("Soyadı")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(25, MinimumLength = 2)]
        public string Surname { get; set; }

        [DisplayName("Telefon numarası")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(20, MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DisplayName("Başlık notu")]
        //[Required(ErrorMessage = "* Sorun hakkında bir başlık belirleyiniz.")]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        [DisplayName("Açıklama notu")]
        //[Required(ErrorMessage = "* Sorun hakkında bir açıklama yapınız.")]
        [StringLength(200, MinimumLength = 10)]
        public string Description { get; set; }

        [DisplayName("Oluşturma tarihi")]
        public DateTime DateOfCreate { get; set; }

        [DisplayName("Ip adresi")]
        public string IpAddress { get; set; }

        [DisplayName("Takip kodu")]
        public string Tag { get; set; }

        [DisplayName("Ek not")]
        public string Notes { get; set; }

        [DisplayName("Oluşturan")]
        public string CreatedBy { get; set; }
        [DisplayName("Düzenleyen")]
        public string EditedBy { get; set; }
        [DisplayName("Düzenleme tarihi")]
        public Nullable<DateTime> DateOfEdit { get; set; }
        [DisplayName("Cihaz teslim tarihi")]
        public Nullable<DateTime> DeviceDeliveryDate { get; set; }

        [DisplayName("Sorun türü")]
        [Required(ErrorMessage = "* Sorun türünü belirleyiniz.")]
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }

        [DisplayName("Ürün")]
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        [DisplayName("Düzenleyen hesap")]
        public int ManagerId { get; set; }
        public virtual Manager Manager { get; set; }

        [DisplayName("Durum")]
        public int ServiceRequestStatusId { get; set; }
        public virtual ServiceRequestStatus ServiceRequestStatus { get; set; }
    }
}