using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restful_Lopputehtava_LauriLeskinen.Models
{
    public partial class Logins
    {
        public int LoginId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter your username")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter your password")]
        public string Password { get; set; }
        public int AccesslevelId { get; set; }
        public string Token { get; set; }
    }
}
