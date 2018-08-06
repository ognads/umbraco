using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using baseCMS.Models;
using System.Web.Mvc;

namespace baseCMS.Controllers
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin,Information Technologies")]
    public class PositionSurfaceController : SurfaceController
    {
        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";

        /// <summary>
        ///   Getting add form for position
        /// </summary>
        /// <returns>Partial view</returns>
        [HttpGet]//Allow only get method
        public ActionResult GetAddForm()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "Position/Add.cshtml");
        }

        /// <summary>
        ///   Getting edit form for position
        /// </summary>
        /// <returns>Partial view</returns>
        [HttpGet]//Allow only get method
        public ActionResult GetEditForm(int contentId)
        {
            IPublishedContent content = (IPublishedContent)Umbraco.Content(contentId);
            Position position = new Position
            {
                ID = content.Id,
                Name = content.GetPropertyValue("positionName").ToString(),
                Description = content.GetPropertyValue("positionDescription").ToString(),
                EntityID = Convert.ToInt32(content.GetPropertyValue("entityID"))
            };

            return PartialView(PARTIAL_VIEW_FOLDER + "Position/Edit.cshtml", position);
        }

        /// <summary>
        ///   Creating position content
        /// </summary>
        /// <param name="parentId"> content's parentId</param>
        /// <param name="position"> position model </param>
        /// <returns>Partial view</returns>
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        public ActionResult Create(Position position, int parentId = 0)
        {
            TempData["elemId"] = "position";

            try
            {
                if (ModelState.IsValid)
                {
                    var positionContent = Services.ContentService.CreateContent(position.Name, parentId, "position");

                    positionContent.SetValue("positionName", position.Name);
                    positionContent.SetValue("positionDescription", position.Description);
                    positionContent.SetValue("entityID", position.EntityID);
                    positionContent.SetValue("status", "open");

                    if (Services.ContentService.SaveAndPublishWithStatus(positionContent).Success)
                    {
                        //Generating default new jobPostList Content under position.
                        var jobPostListContent = Services.ContentService.CreateContent("Job Post List", positionContent.Id, "jobPostList");
                        if (Services.ContentService.SaveAndPublishWithStatus(jobPostListContent).Success)
                        {
                            TempData["Msg"] = "Position succesfully created.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");

                        }

                    }

                }

                TempData["Msg"] = "Cannot create position.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Cannot create position.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                //return Json(new { success = false, responseText = "Content could not be created." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Updating position content
        /// </summary>
        /// <param name="position"> position model </param>
        /// <returns>Partial view</returns>
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        public ActionResult Update(Position position)
        {
            TempData["elemId"] = "position";

            try
            {
                if (ModelState.IsValid)
                {
                    var service = Services.ContentService;
                    var positionContent = service.GetById(position.ID);

                    positionContent.SetValue("positionName", position.Name);
                    positionContent.SetValue("positionDescription", position.Description);
                    positionContent.SetValue("entityID", position.EntityID);

                    if (Services.ContentService.SaveAndPublishWithStatus(positionContent).Success)
                    {
                        TempData["Msg"] = "Position succesfully updated.";
                        return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");
                    }

                }

                TempData["Msg"] = "Cannot update position.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Cannot update position.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
            }
        }

        //Delete operation of Position is in the CommonSurfaceController
    }
}
