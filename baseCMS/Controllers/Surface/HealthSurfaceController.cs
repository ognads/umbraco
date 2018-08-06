using baseCMS.Models;
using System;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    public class HealthSurfaceController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Health/";
        // GET: HealthCreateForm
        /// <summary>
        /// Returns a partial view for the health event add form
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public ActionResult GetAddForm()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "Add.cshtml");
        }
        // GET: HealthUpdateForm
        /// <summary>
        /// Returns a partial view for the health edit form
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public ActionResult GetEditForm(int contentId = 0)
        {
            if (contentId != 0)
            {
                IPublishedContent content = Umbraco.Content(contentId);
                HealthEvent health = new HealthEvent
                {
                    ID = contentId,
                    Title = content.GetPropertyValue("healthEventTitle").ToString(),
                    Description = content.GetPropertyValue("healthEventDescription").ToString(),
                    IsReported = Convert.ToBoolean(content.GetPropertyValue("healthEventReport").ToString())

                };
                return PartialView(PARTIAL_VIEW_FOLDER + "Edit.cshtml", health);
            }
            return PartialView(PARTIAL_VIEW_FOLDER + "Edit.cshtml");
        }
        //POST:CreateHealthEvent
        /// <summary>
        /// Submits the add form and adds a new health event
        /// </summary>
        /// <param name="healthEvent"></param>
        /// <returns>ActionResult</returns>
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        public ActionResult CreateHealth(HealthEvent healthEvent)
        {
            TempData["elemId"] = "health";
            if (ModelState.IsValid)
            {
                IContent content = Services.ContentService.CreateContent(healthEvent.Title, healthEvent.ParentId, "healthEvent");
                content.SetValue("healthEventTitle", healthEvent.Title);
                content.SetValue("healthEventDescription", healthEvent.Description);
                if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                {
                    TempData["Msg"] = "Health event successfully created";
                    return PartialView("~/Views/Partials/FormSuccess.cshtml");

                }
            }
            TempData["Msg"] = "Cannot create health event";
            return PartialView("~/Views/Partials/FormError.cshtml");
        }
        //POST:UpdateHealthEvent
        /// <summary>
        /// Edits an existing health event
        /// </summary>
        /// <param name="healthEvent"></param>
        /// <returns></returns>
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        public ActionResult EditHealth(HealthEvent healthEvent)
        {
            TempData["elemId"] = "health";
            if (ModelState.IsValid)
            {
                IContent content = Services.ContentService.GetById(healthEvent.ID);
                content.SetValue("healthEventTitle", healthEvent.Title);
                content.SetValue("healthEventDescription", healthEvent.Description);
                if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                {
                    TempData["Msg"] = "Health event successfully edited";
                    return PartialView("~/Views/Partials/FormSuccess.cshtml");
                }
            }
            TempData["Msg"] = "Cannot edit health event";
            return PartialView("~/Views/Partials/FormError.cshtml");
        }
    }
}
