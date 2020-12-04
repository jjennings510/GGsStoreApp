using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GGsWeb.Models
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string password { get; set; }
    }
}
