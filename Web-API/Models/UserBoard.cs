using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_API.Models
{
    public class UserBoard
    {
        public int leagueId { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public Board board { get; set; }
        public int rep { get; set; }
    }
}
