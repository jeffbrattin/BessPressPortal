using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BessPressPortal.Shared.Models
{
    public class CreateUserDto
    {

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        public string Password { get; set; }

    }
}
