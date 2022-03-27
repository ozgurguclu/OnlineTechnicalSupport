using OnlineTechnicalSupport.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.ViewModels
{
    public class IssueReportsViewModel
    {
        public IEnumerable<IssueReport> UnManagedRequests { get; set; }
        public IEnumerable<IssueReport> ManagedRequests { get; set; }
    }

    public class LiveSupportRequestsViewModel
    {
        public IEnumerable<Chat> UnManagedRequests { get; set; }
        public IEnumerable<Chat> ManagedRequests { get; set; }
    }
}