using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.DTOs
{
    [NotMapped]
    public class LoginDTO
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}
