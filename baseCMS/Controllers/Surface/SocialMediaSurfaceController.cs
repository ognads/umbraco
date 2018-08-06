using baseCMS.Models;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    public class SocialMediaSurfaceController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/SocialMedia/";
        // GET: SocialMediaCreateForm
        /// <summary>
        /// Returns the partial view for the social media add form
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public ActionResult GetAddForm()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "Add.cshtml");
        }
        /// <summary>
        /// Returns the partial view for the social media edit form
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns>ActionResult</returns>
        // GET: SocialMediaUpdateForm
        [HttpGet]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public ActionResult GetEditForm(int contentId = 0)
        {
            if (contentId != 0)
            {
                IPublishedContent content = Umbraco.Content(contentId);
                SocialMedia socialMedia = new SocialMedia
                {
                    ID = contentId,
                    Name = content.GetPropertyValue("socialMediaName").ToString(),
                    Url = content.GetPropertyValue("socialMediaUrl").ToString()

                };
                return PartialView(PARTIAL_VIEW_FOLDER + "Edit.cshtml", socialMedia);
            }
            return PartialView(PARTIAL_VIEW_FOLDER + "Edit.cshtml");
        }
        //POST:CreateSocialMedia
        /// <summary>
        /// Creates a new social media 
        /// </summary>
        /// <param name="socialMedia"></param>
        /// <returns>ActionResult</returns>
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        public ActionResult CreateSocialMedia(SocialMedia socialMedia)
        {
            TempData["elemId"] = "socialmedia";
            if (ModelState.IsValid)
            {
                IContent content = Services.ContentService.CreateContent(socialMedia.Name, socialMedia.ParentId, "socialMediaInformation");
                content.SetValue("socialMediaName", socialMedia.Name);
                content.SetValue("socialMediaUrl", socialMedia.Url);
                if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                {
                    TempData["Msg"] = "Social Media successfully created";
                        return PartialView("~/Views/Partials/FormSuccess.cshtml");
                }
            }
            TempData["Msg"] = "Cannot create social media";
            return PartialView("~/Views/Partials/FormError.cshtml");
        }
        //POST:UpdateSocialMedia
        /// <summary>
        /// Edits an existing social media
        /// </summary>
        /// <param name="socialMedia"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        public ActionResult EditSocialMedia(SocialMedia socialMedia)
        {
            TempData["elemId"] = "socialmedia";
            if (ModelState.IsValid)
            {
                IContent content = Services.ContentService.GetById(socialMedia.ID);
                content.SetValue("socialMediaName", socialMedia.Name);
                content.SetValue("socialMediaUrl", socialMedia.Url);
                if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                {
                    TempData["Msg"] = "Social media successfully edited";
                    return PartialView("~/Views/Partials/FormSuccess.cshtml");
                }
            }
            TempData["Msg"] = "Cannot edit social media";
            return PartialView("~/Views/Partials/FormError.cshtml");
        }
    }
}
