using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class AccountSecurityRequest
    {
        [Key]
        public int RequestId { get; set; }

        public string Reason { get; set; }

        public string Email { get; set; }

        public string VerificationCode { get; set; }

        public DateTime Expires { get; set; }
    }
}