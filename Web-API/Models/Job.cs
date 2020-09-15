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
        public string mediaURI { get; set; }

        [Required]
        public string jobDescription { get; set; }
        [Required]
        public string location { get; set; }
        [Required]
        public bool isCompleted { get; set; }

        [ForeignKey("posterId")]
        public UserBoard poster { get; set; }
    }
}
