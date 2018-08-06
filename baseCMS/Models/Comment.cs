using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;

namespace baseCMS.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [AllowHtml]
        public String Description { get; set; }
        [Display(Name ="Creation Time")]
        public DateTime CreationTime { get; set; }
        [Display(Name ="Creator")]
        public String Creator { get; set; }
        [Display(Name ="Parent Comment")]
        public int ParentComment { get; set; }
        [Display(Name ="Last Edited")]
        public DateTime LastEdited { get; set; }
        [Display(Name ="Employee Photo")]
        public String EmployeePhoto { get; set; }
    }
}
