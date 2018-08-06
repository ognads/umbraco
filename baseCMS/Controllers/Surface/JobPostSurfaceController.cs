using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using baseCMS.Models;
using System.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
    public class JobPostSurfaceController : SurfaceController
    {

        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";

        /// <summary>
        ///   Getting add form for jobpost
        /// </summary>
        /// <returns>Partial view</returns>
        [HttpGet]//Allow only get method
        public ActionResult GetAddForm()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "JobPost/Add.cshtml");
        }

        /// <summary>
        ///   Getting edit form for jobpost
        /// </summary>
        /// <returns>Partial view</returns>
        [HttpGet]//Allow only get method
        public ActionResult GetEditForm(int contentId)
        {
            IPublishedContent content = (IPublishedContent)Umbraco.Content(contentId);

            JobPost jobPost = new JobPost
            {
                ID = content.Id,
                JobDescription = content.GetPropertyValue("jobDescription").ToString(),
                NeededSkills = content.GetPropertyValue("neededSkills").ToString(),
                ContactEmail = content.GetPropertyValue("contactEmail").ToString(),
                NumberOfVacancy = (int)content.GetPropertyValue("numberOfVacancy"),
                EndDate = Convert.ToDateTime(content.GetPropertyValue("endDate")),
                UrlOfPost = content.GetPropertyValue("urlOfPost").ToString(),
                JobOffer = content.GetPropertyValue("jobOffer").ToString(),
                JobOverview = content.GetPropertyValue("jobOverview").ToString()
            };

            return PartialView(PARTIAL_VIEW_FOLDER + "JobPost/Edit.cshtml", jobPost);
        }

        /// <summary>
        ///   Creating jobpost content
        /// </summary>
        /// <param name="parentId"> content's parentId</param>
        /// <param name="jobpost"> jobpost model </param>
        /// <returns>Partial view</returns>
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        public ActionResult Create(JobPost jobPost, int parentId = 0)
        {
            TempData["elemId"] = "jobpost";

            try
            {
                if (ModelState.IsValid)
                {
                    //Global unique id
                    var jobPostContent = Services.ContentService.CreateContent(jobPost.UrlOfPost, parentId, "jobPost");
                    jobPostContent.SetValue("jobDescription", jobPost.JobDescription);
                    jobPostContent.SetValue("neededSkills", jobPost.NeededSkills);
                    jobPostContent.SetValue("contactEmail", jobPost.ContactEmail);
                    jobPostContent.SetValue("endDate", jobPost.EndDate);
                    jobPostContent.SetValue("numberOfVacancy", jobPost.NumberOfVacancy);
                    jobPostContent.SetValue("urlOfPost", jobPost.UrlOfPost);
                    jobPostContent.SetValue("jobOffer", jobPost.JobOffer);
                    jobPostContent.SetValue("jobOverview", jobPost.JobOverview);
                    if (Services.ContentService.SaveAndPublishWithStatus(jobPostContent).Success)
                    {
                        TempData["Msg"] = "Jobpost succesfully created.";
                        return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");
                    }

                }

                TempData["Msg"] = "Cannot create jobpost.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Cannot create jobpost.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                //return Json(new { success = false, responseText = "Content could not be created." }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        ///   Updating jobpost content
        /// </summary>
        /// <param name="jobpost"> jobpost model </param>
        /// <returns>Partial view</returns>
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        public ActionResult Update(JobPost jobPost)
        {
            //This is the position's datatable's id
            //It will be used in FormError and FormSuccess pages to reload the table
            TempData["elemId"] = "jobpost";

            try
            {
                if(ModelState.IsValid)
                {
                    var service = Services.ContentService;
                    var jobPostContent = service.GetById(jobPost.ID);

                    jobPostContent.SetValue("jobDescription", jobPost.JobDescription);
                    jobPostContent.SetValue("neededSkills", jobPost.NeededSkills);
                    jobPostContent.SetValue("contactEmail", jobPost.ContactEmail);
                    jobPostContent.SetValue("endDate", jobPost.EndDate);
                    jobPostContent.SetValue("numberOfVacancy", jobPost.NumberOfVacancy);
                    jobPostContent.SetValue("urlOfPost", jobPost.UrlOfPost);
                    jobPostContent.SetValue("jobOffer", jobPost.JobOffer);
                    jobPostContent.SetValue("jobOverview", jobPost.JobOverview);
                    if (Services.ContentService.SaveAndPublishWithStatus(jobPostContent).Success)
                    {
                        //This is going to be used in FormError and FormSuccess to show user.
                        TempData["Msg"] = "Jobpost succesfully updated.";
                        return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");
                    }

                }

                TempData["Msg"] = "Cannot update jobpost.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Cannot update jobpost.";
                return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                //return Json(new { success = false, responseText = "Content could not be created." }, JsonRequestBehavior.AllowGet);
            }


        }

        //Delete operation of Jobpost is in the CommonSurfaceController
    }
}
