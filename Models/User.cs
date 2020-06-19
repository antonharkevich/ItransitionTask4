using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentityApp.Models
{
    public class User : IdentityUser
    {
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastRegistrationDate { get; set; }

        public string Status { get; set; }
        public bool IsBlocked { get; set; }

        public bool IsChecked { get; set; }

        public string Name { get; set; }
    }
}
