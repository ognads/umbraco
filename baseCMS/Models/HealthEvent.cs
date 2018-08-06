using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace baseCMS.Models
{
    public class HealthEvent
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Is Reported")]
        public bool IsReported { get; set; }
        [Display(Name = "Parent ID")]
        public int ParentId { get; set; }
    }
}
