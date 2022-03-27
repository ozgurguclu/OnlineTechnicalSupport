using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class ComputerSkill
    {
        [Key]
        public int ComputerSkillId { get; set; }

        [DisplayName("Bilgisayar bilgisi")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}