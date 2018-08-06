using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace baseCMS.Models
{
    public class JobPost
    {
        public int ID { get; set; }
        [AllowHtml]
        [Required]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        [AllowHtml]
        [Required]
        [Display(Name = "Needed Skills")]
        public string NeededSkills { get; set; }
        [AllowHtml]
        [Display(Name = "Job Overview")]
        public string JobOverview { get; set; }
        [AllowHtml]
        [Display(Name = "Job Offer")]
        public string JobOffer { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Range(0, 100, ErrorMessage = "Can only be between 0 .. 100")]
        [Display(Name = "Number Of Vacancy")]
        public int NumberOfVacancy { get; set; }

        [Required]
        [Url]
        [Display(Name = "Url Of Post")]
        public string UrlOfPost { get; set; }
        [Display(Name = "Post Umbraco Url")]
        public string PostUmbracoUrl { get; set; }

    }
}
