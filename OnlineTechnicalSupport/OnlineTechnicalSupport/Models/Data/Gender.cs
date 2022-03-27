using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }

        [DisplayName("Cinsiyet")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}