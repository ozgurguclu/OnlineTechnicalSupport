using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class TextMessage
    {
        [Key]
        public int TextMessageId { get; set; }

        [DisplayName("Yazılı cevap")]
        [MaxLength(100)]
        public string TextContent { get; set; }

        [DisplayName("Resimli cevap")]
        public string ImageContent { get; set; }

        [DisplayName("Gönderen")]
        public string SenderName { get; set; }

        [DisplayName("Tarih")]
        public DateTime Date { get; set; }

        [DisplayName("Ip Adres")]
        public string IpAddress { get; set; }

        public Nullable<int> CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public Nullable<int> ManagerId { get; set; }
        public virtual Manager Manager { get; set; }

        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}