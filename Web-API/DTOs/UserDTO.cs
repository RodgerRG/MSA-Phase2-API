using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.DTOs
{
    [NotMapped]
    public class UserDTO
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}
