using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Education
    {
        [Key]
        public int EducationId { get; set; }

        [DisplayName("Eğitim")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}