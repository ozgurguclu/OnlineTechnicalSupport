using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.ViewModels
{
    public class OverViewModel
    {
        public int MissedReportCount { get; set; }
        public int MissedChatCount { get; set; }
        public int TotalReportCount { get; set; }
        public int TotalChatCount { get; set; }
        public int TotalRepliedReportCount { get; set; }
        public int TotalRepliedChatCount { get; set; }
        public int ReceivedReportCount { get; set; }
        public int ReceivedChatCount { get; set; }
        public int RepliedReportCount { get; set; }
        public int RepliedChatCount { get; set; }
    }

    public class ReplyRequestView
    {
        [DisplayName("Yanıtınız")]
        public string ReplyContent { get; set; }
    }
}