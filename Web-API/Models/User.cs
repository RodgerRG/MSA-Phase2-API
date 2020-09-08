using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Web_API.DTOs;

namespace Web_API.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }

        public List<UserLeague> leagues { get; set; }

        public User(UserDTO dto)
        {
            this.FirstName = dto.firstName;
            this.LastName = dto.lastName;
            this.Email = dto.email;
            this.Gender = dto.gender;
            this.UserName = dto.username;


        }

        //Required for migrations
        public User() {
            leagues = new List<UserLeague>();
        }
    }
}
