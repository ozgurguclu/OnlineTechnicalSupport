using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class CampaignApplication
    {
        [Key]
        [DisplayName("Kayıt no")]
        public int CampaignApplicationId { get; set; }

        [DisplayName("Ürün Seri Numarası")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string DeviceSerialNo { get; set; }

        [DisplayName("Fatura Numarası")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string BillNo { get; set; }

        [DisplayName("Ürün Seri Numarası Resmi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string AttachmentDeviceSerialNoImage { get; set; }

        [DisplayName("Fatura Resmi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string AttachmentBillImage { get; set; }

        [DisplayName("Ürün Satın Alma Tarihi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Satın Alınan Mağaza")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Store { get; set; }

        [DisplayName("Kullanım Amacı")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string UsageType { get; set; }

        [NotMapped]
        [DisplayName("Hesaba kayıtlı isim")]
        public bool SelectUserName { get; set; }

        [DisplayName("Ödülü Alacak Kişinin İsmi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [NotMapped]
        [DisplayName("Hesaba kayıtlı epostayı seç")]
        public bool SelectUserEmail { get; set; }

        [DisplayName("Eposta")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Eposta yazımına dikkat ediniz.")]
        public string Email { get; set; }

        [NotMapped]
        [DisplayName("Hesaba kayıtlı telefon numarasını seç")]
        public bool SelectUserPhoneNumber { get; set; }

        [DisplayName("Telefon numarası")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(25, MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [NotMapped]
        [DisplayName("Hesaba kayıtlı yerleşim adresini seç")]
        public bool SelectUserAddress { get; set; }

        [DisplayName("Ödül Teslim Adresi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(75, MinimumLength = 25)]
        public string Address { get; set; }

        [DisplayName("Başvuru Tarihi")]
        public DateTime ApplicationDate { get; set; }

        public string IpAddress { get; set; }

        [DisplayName("Başvuru Sonucu")]
        public string Result { get; set; }

        [DisplayName("Başvuran hesap")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [DisplayName("Yöneten hesap")]
        public Nullable<int> ManagerId { get; set; }
        public virtual Manager Manager { get; set; }

        [DisplayName("Kampanya")]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}