using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [DisplayName("Eposta")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Eposta yazımına dikkat ediniz.")]
        public string Email { get; set; }

        [DisplayName("Kullanıcı adı")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "En az 8 karakter uzunluğunda olmalıdır.")]
        public string LoginName { get; set; }

        [DisplayName("Parola")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "En az 8 karakter uzunluğunda olmalıdır.")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        [NotMapped]
        [DisplayName("Parola (tekrar)")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [DataType(DataType.Password)]
        [Compare("LoginPassword")]
        public string ReLoginPassword { get; set; }

        // CUSTOMER DETAILS

        [DisplayName("Ad")]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayName("Soyad")]
        [StringLength(25, MinimumLength = 2)]
        public string Surname { get; set; }

        [DisplayName("Telefon numarası")]
        [StringLength(25, MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DisplayName("Şehir")]
        [StringLength(25, MinimumLength = 2)]
        public string City { get; set; }

        [DisplayName("Posta kodu")]
        public string PostCode { get; set; }

        [DisplayName("Adres")]
        [StringLength(100, MinimumLength = 25)]
        public string Address { get; set; }

        [DisplayName("Cinsiyet")]
        public Nullable<int> GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        [DisplayName("Eğitim")]
        public Nullable<int> EducationId { get; set; }
        public virtual Education Education { get; set; }

        [DisplayName("Meslek")]
        public Nullable<int> JobId { get; set; }
        public virtual Job Job { get; set; }

        [DisplayName("Bilgisayar bilgisi")]
        public Nullable<int> ComputerSkillId { get; set; }
        public virtual ComputerSkill ComputerSkill { get; set; }

        [DisplayName("Bölge")]
        public Nullable<int> CountryId { get; set; }
        public virtual Country Country { get; set; }

        // CUSTOMER DETAILS END

        [DisplayName("Son giriş tarihi")]
        public Nullable<DateTime> LastLoginDate { get; set; }
        [DisplayName("Son giriş IP adresi")]
        public string LastLoginIpAddress { get; set; }

        [DisplayName("Hesap oluşturma tarihi")]
        public DateTime DateOfCreate { get; set; }
        [DisplayName("Doğrulanmış hesap")]
        public string ConfirmedEmail { get; set; }
        [DisplayName("Aktif hesap")]
        public string Activated { get; set; }

        public virtual ICollection<IssueReport> IssueReports { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<TextMessage> TextMessages { get; set; }
        public virtual ICollection<CampaignApplication> CampaignApplications { get; set; }
    }
}