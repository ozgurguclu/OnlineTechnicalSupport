using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [DisplayName("Ülke kodu")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "2 karakter uzunluğunda olmalıdır.")]
        public string Code { get; set; }

        [DisplayName("Ülke")]
        [Required(ErrorMessage = "* Boş geçilemez")]
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}