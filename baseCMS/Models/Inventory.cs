using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace baseCMS.Models
{
    public class Inventory
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Warranty End Date")]
        public DateTime WarrantyEndDate { get; set; }

        [MaxLength(20)]
        [Display(Name = "Invoice")]
        public string Invoice {get;set;}

        [Display(Name = "Cost")]
        public decimal Cost {get;set;}
        [AllowHtml]
        [MaxLength(100)]
        [Display(Name = "Description")]
        public string Description {get;set;}

        [Required]
        [Display(Name = "Assigned To")]
        public string AssignedTo {get;set;}

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Inventory Url")]
        public string InventoryUrl { get; set; }

    }
}
