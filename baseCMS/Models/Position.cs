using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace baseCMS.Models
{
    public class Position
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Job Post List Url")]
        public string JobPostListUrl { get; set; }
        [AllowHtml]
        [MaxLength(100)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Entity ID")]
        public int EntityID { get; set; }
        [Display(Name = "Position Url")]
        public string PositionUrl { get; set; }

    }
}
