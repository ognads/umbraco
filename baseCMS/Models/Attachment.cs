using System;
using System.ComponentModel.DataAnnotations;


namespace baseCMS.Models
{
    public class Attachment
    {
        public int ID { get; set; }

        [Required]
        [Display(Name ="Attachment Name")]
        public string AttachmentName { get; set; }

        [Required]
        [Display(Name ="Attachment URL")]
        public string AttachmentUrl { get; set; }

        [Required]
        [Display(Name ="Attachment Version")]
        public string AttachmentVersion { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Created At")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name ="Created By")]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Updated At")]
        public Nullable<DateTime> UpdatedAt { get; set; }

        [Display(Name ="Updated By")]
        public string UpdatedBy { get; set; }

    }
}
