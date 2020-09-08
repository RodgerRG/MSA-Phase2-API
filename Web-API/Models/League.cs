using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Models
{
    public class League
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leagueId { get; set; }
        public int ownerId { get; set; }

        [ForeignKey("ownerId")]
        public User owner { get; set; }

        public List<UserLeague> users { get; set; }

        public League()
        {
            users = new List<UserLeague>();
        }
    }
}
