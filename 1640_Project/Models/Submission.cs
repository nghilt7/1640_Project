using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _1640_Project.Models
{
    public class Submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubmissionID { get; set; }
        public string SubmissionName { get; set; }
        public string SubmissionDescription { get; set; }
        public DateTime CloseDate { get; set; }
        public DateTime FinalDate { get; set; }    
    }
}