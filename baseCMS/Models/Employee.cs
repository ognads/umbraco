using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace baseCMS.Models
{
    public class Employee
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name ="Name")]
        public string Name { get; set; }
        [MaxLength(20)]
        [Display(Name ="MiddleName")]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name ="Surname")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Display(Name ="Gender")]
        public string Gender { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Birthday")]
        public DateTime BirthDay { get; set; }
        [Display(Name ="Salary")]
        public decimal Salary { get; set; }
        [MaxLength(50)]
        [Display(Name ="Address Line 1")]
        public string AddressLine1 { get; set; }
        [MaxLength(50)]
        [Display(Name ="Address Line 2")]
        public string AddressLine2 { get; set; }
        [MaxLength(20)]
        [Display(Name ="State")]
        public string State { get; set; }
        [MaxLength(20)]
        [Display(Name ="Country")]
        public string Country { get; set; }
        [MaxLength(10)]
        [Display(Name ="Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name ="Longitude")]
        public string Longitude { get; set; }
        [Display(Name ="Latitude")]
        public string Latitude { get; set; }
        [Display(Name="Position")]
        public int PositionID { get; set; }
        [Display(Name ="Tags")]
        public string Tags { get; set; }
        [Display(Name ="URL")]
        public string EmployeeUrl { get; set; }
        [Display(Name ="Social Security No")]
        public string SocialSecurityNo { get; set; }
        [Display(Name ="National ID")]
        public string NationalID { get; set; }
        [Display(Name ="Health Known Issues")]
        public string KnownIssues { get; set; }
        [RegularExpression(@"^[0-9]*$")]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Display(Name ="Alternative Email")]
        public string AlternativeEmail { get; set; }
        [AllowHtml]
        [Display(Name ="Position Description")]
        public string PositionDescription { get; set; }



    }
}
