
using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers
{
    [Umbraco.Web.Mvc.MemberAuthorize]
    public class CommentSurfaceController : SurfaceController
    {
        /// <summary>
        /// Creates a new comment under the given parent
        /// </summary>
        /// <param name="parentId">Parent's Id</param>
        /// <param name="commentModel">Comment model that'll be used to save</param>
        [HttpPost]

        public ActionResult CreateComment([Bind(Include = "description,parentComment")]Comment commentModel)
        {
            IPublishedContent parentContent = Umbraco.Content(commentModel.ParentComment);
            commentModel.CreationTime = DateTime.Now;
            commentModel.ParentComment = parentContent.Id;
            if (ModelState.IsValid)
            {
                MemberHelper memberHelper = new MemberHelper();
                IPublishedContent currentMember =memberHelper.GetCurrentMember();
                var content = Services.ContentService.CreateContent(currentMember.Name +commentModel.CreationTime , parentContent.Id, "comment");
                content.SetValue("commentDescription", commentModel.Description);
                content.SetValue("commentWrittenBy", currentMember.Id);
                if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                {
                      TempData["Msg"] = "Comment successfully added";
                      return PartialView("~/Views/Partials/FormSuccess.cshtml");
                }
                else
                {
                    TempData["Msg"] = "Comment couldn't be added";
                    return PartialView("~/Views/Partials/FormError.cshtml");
                }
            }
            TempData["Msg"] = "Wait... This is not a comment";
            return PartialView("~/Views/Partials/FormError.cshtml");
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns>Comment add partiaş view</returns>
        [HttpGet]
        public ActionResult GetCommentAddForm()
        {
            return PartialView("~/Views/Partials/Comment/Add.cshtml");
        }
    }
}
