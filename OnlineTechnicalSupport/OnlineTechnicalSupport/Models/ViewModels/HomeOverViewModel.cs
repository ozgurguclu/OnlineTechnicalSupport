using OnlineTechnicalSupport.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.ViewModels
{
    public class HomeOverViewModel
    {
        public IEnumerable<Asset> Assets { get; set; }
        public IEnumerable<ServiceRequest> ServiceRequests { get; set; }
        public IEnumerable<IssueReport> IssueReports { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
    }

    public class ServiceSupportOverViewModel
    {
        public IEnumerable<Asset> SelectedAsset { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
    }
}