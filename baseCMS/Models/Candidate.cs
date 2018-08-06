using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace baseCMS.Models
{
    public class Candidate
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name ="Name")]
        public string CandidateName { get; set; }

        [MaxLength(20)]
        [Display(Name ="Middle Name")]
        public string CandidateMiddleName { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name ="Surname")]
        public string CandidateSurname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string CandidateEmail { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Birthday")]
        public DateTime CandidateBirthday { get; set; }
        [RegularExpression(@"^[0-9]*$")]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [AllowHtml]
        [Display(Name ="Health Known Issues")]
        public string HealthKnownIssues { get; set; }

        [Display(Name ="Legally Disabled")]
        public bool HealthLegallyDisabled { get; set; }
        [AllowHtml]
        [Display(Name ="Work Experience")]
        public string CandidateWorkExperience { get; set; }
        [AllowHtml]
        [Display(Name ="Education")]
        public string CandidateEducation { get; set; }
        [AllowHtml]
        [Display(Name ="Skills")]
        public string CandidateSkills { get; set; }

        [Display(Name ="Gender")]
        public string CandidateGender { get; set; }

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

        [Display(Name ="URL")]
        public string CandidateUrl { get; set; }

        [Display(Name ="Status")]
        public string CandidateStatus { get; set; }

        [Display(Name ="Resource Description")]
        public string ResourceDescription { get; set; }
        [EmailAddress]
        [Display(Name ="Alternative Email")]
        public string AlternativeEmail { get; set; }
    }
}
