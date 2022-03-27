using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Issue
    {
        [Key]
        public int IssueId { get; set; }

        [DisplayName("Sorun türü")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
        public virtual ICollection<IssueReport> IssueReports { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}