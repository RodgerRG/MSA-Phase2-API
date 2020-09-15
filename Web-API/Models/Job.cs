using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Models
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int jobId { get; set; }
        public int posterId { get; set; }
        public int boardId { get; set;}
        public string mediaURI { get; set; }

        [Required]
        public string jobDescription { get; set; }
        [Required]
        public string location { get; set; }
        [Required]
        public bool isCompleted { get; set; }
        public int workerId { get; set; }

        [ForeignKey("posterId")]
        public User poster { get; set; }

        [ForeignKey("boardId")]
        public Board board { get; set; }

        [ForeignKey("workerId")]
        public User worker { get; set; }
    }
}
