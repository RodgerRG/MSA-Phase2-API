using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.DTOs
{
    [NotMapped]
    public class BoardDTO
    {
        public int boardId { get; set; }
        public string boardName { get; set; }
        public int ownerId { get; set; }
        public List<JobDTO> jobs { get; set; }
    }
}
