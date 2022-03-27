using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Campaign
    {
        [Key]
        public int CampaignId { get; set; }

        [DisplayName("Kampanya adı")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Name { get; set; }

        [DisplayName("Başlık")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Title { get; set; }

        [DisplayName("Açıklama")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Description { get; set; }

        [DisplayName("Kampanya Resmi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Image { get; set; }

        [DisplayName("Kampanya Başlangıç Tarihi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public DateTime StartDate { get; set; }

        [DisplayName("Kampanya Bitiş Tarihi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public DateTime EndDate { get; set; }

        [DisplayName("Kampanya Aktifliği")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public int Activated { get; set; }

        public virtual ICollection<CampaignApplication> CampaignApplications { get; set; }
    }
}