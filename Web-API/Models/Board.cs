using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Models
{
    public class Board
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int boardId { get; set; }
        public int ownerId { get; set; }

        [Required]
        public string location { get; set; }

        [ForeignKey("ownerId")]
        public User owner { get; set; }

        public List<UserBoard> users { get; set; }

        public List<Job> jobs { get; set; }

        public Board()
        {
            users = new List<UserBoard>();
        }
    }
}
