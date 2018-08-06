using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using baseCMS.Models;
using baseCMS.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers.Api
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
    public class JobPostApiController : UmbracoApiController
    {
        /// <summary>
        ///     Getting jobpost contents and mapping them to model
        /// </summary>
        /// <param name="page"></param>
        /// <param name="max"></param>
        /// <param name="parentId"></param>
        /// <returns>Jobpost Models</returns>
        [HttpGet]
        public List<JobPost> GetJobPosts(int page = 0, int max = 10, int parentId = 0)
        {
            ContentServiceController contentService = new ContentServiceController();
            List<IPublishedContent> contents = contentService.GetContentListByTypeAndParentId("jobPost", parentId, page, max);

            List<JobPost> jobPostList = new List<JobPost>();

            foreach (var content in contents)
            {
                JobPost jobPost = new JobPost
                {
                    ID = content.Id,
                    JobDescription = content.GetPropertyValue("jobDescription").ToString(),
                    NeededSkills = content.GetPropertyValue("neededSkills").ToString(),
                    ContactEmail = content.GetPropertyValue("contactEmail").ToString(),
                    NumberOfVacancy = (int)content.GetPropertyValue("numberOfVacancy"),
                    EndDate = Convert.ToDateTime(content.GetPropertyValue("endDate")),
                    UrlOfPost = content.GetPropertyValue("urlOfPost").ToString(),
                    PostUmbracoUrl = content.Url,
                    JobOffer = content.GetPropertyValue("jobOffer").ToString(),
                    JobOverview = content.GetPropertyValue("jobOverview").ToString()
                };

                jobPostList.Add(jobPost);
            }


            return jobPostList;
        }
    }
}
