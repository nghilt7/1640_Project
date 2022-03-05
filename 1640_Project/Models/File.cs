using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _1640_Project.Models
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileID { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime LastChange { get; set; }
    }
}