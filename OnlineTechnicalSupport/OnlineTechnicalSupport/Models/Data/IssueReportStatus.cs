using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class IssueReportStatus
    {
        [Key]
        public int IssueReportStatusId { get; set; }

        [Required(ErrorMessage = "Bir etiket belirleyiniz.")]
        [StringLength(50, MinimumLength = 2)]
        public string Tag { get; set; }

        public virtual ICollection<IssueReport> IssueReports { get; set; }
    }
}