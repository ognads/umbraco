using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace baseCMS.Models
{
    public class Entity
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name ="Name")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Display(Name = "Employee List Url")]
        public string EmployeeListUrl { get; set; }
    }
}
