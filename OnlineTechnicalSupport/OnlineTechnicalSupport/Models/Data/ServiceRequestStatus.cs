using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class ServiceRequestStatus
    {
        [Key]
        public int ServiceRequestStatusId { get; set; }

        [DisplayName("Durum")]
        [Required(ErrorMessage = "Bir etiket belirleyiniz.")]
        [StringLength(50, MinimumLength = 2)]
        public string Tag { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}