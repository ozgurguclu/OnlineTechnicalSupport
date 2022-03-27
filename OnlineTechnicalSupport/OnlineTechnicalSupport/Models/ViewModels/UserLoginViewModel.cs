using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("Kullanıcı adı veya Eposta")]
        public string LoginNameOrEmail { get; set; }

        [DisplayName("Parola")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        [DisplayName("Kullanıcı")]
        public int UserType { get; set; }
    }
}