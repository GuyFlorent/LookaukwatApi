using System;
using System.Collections.Generic;
using System.Text;

namespace LookaukwatMobile.Models
{
    class RegisterBindingModel
    {
        public string FirstName { get; set; }

        public string ParrainName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
