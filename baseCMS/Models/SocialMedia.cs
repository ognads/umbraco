using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace baseCMS.Models
{
    public class SocialMedia
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Url]
        [Display(Name = "Url")]
        public string Url { get; set; }
        [Display(Name = "Parent Id")]
        public int ParentId { get; set; }
    }
}
