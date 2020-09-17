using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.DTOs
{
    [NotMapped]
    public class BoardListDTO
    {
        public int boardId { get; set; }
        public string boardName { get; set; }
    }
}
