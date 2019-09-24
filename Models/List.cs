using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace cexam.Models
{
    public class List
    {
        [Key]
        public int PartyId { get; set; }

        [Required(ErrorMessage = "This is a required Field.")]
        public string ActName { get; set; }

        [Required(ErrorMessage = "This is a required Field.")]
        [FutureDate]
        public DateTime ActDate { get; set; }

        [Required(ErrorMessage = "This is a required Field.")]
        public string TimeFormat { get; set; }

        [Required(ErrorMessage = "This is a required Field.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "This is a required Field.")] 
        public string Description { get; set; }

        public int CreatorId { get; set; }

        public User Creator { get; set; }

        public List<Join> Attendees { get; set; }
    }
}