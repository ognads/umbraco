using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
    public class PositionApiController : UmbracoApiController
    {
        //GET: Positions With Paging For Listing in table
        [HttpGet]
        public List<Position> GetPositions(int page = 0, int max = 10, int parentId = 0)
        {
            ContentServiceController contentService = new ContentServiceController();
            List<IPublishedContent> contents = contentService.GetContentListByTypeAndParentId("position", parentId, page, max);
            List<Position> positionList = new List<Position>();
            
            foreach (var content in contents)
            {
                Position position = new Position
                {
                    ID = content.Id,
                    JobPostListUrl = content.Children().First().Url,
                    Name = content.GetPropertyValue("positionName").ToString(),
                    Description = content.GetPropertyValue("positionDescription").ToString(),
                    PositionUrl = content.Url
                };

                positionList.Add(position);
            }


            return positionList;
        }
        [HttpPost]
        public void TogglePosition(int positionId, string status)
        {
            if (status != "open" && status != "closed")
            {
                return;
            }
            Dictionary<string, string> candidateConfig = ConfigurationApiController.Instance.GetConfiguration("Candidate");
            IContent position = Services.ContentService.GetById(positionId);
            IEnumerable<IContent> jobPostLists = position.Children().Where(x => x.Published);

            if (position.GetValue("status") != null && position.GetValue("status").ToString() == status)
                return;
            List<string> candidateEmailList = new List<string>();

            foreach (IContent jobPostList in jobPostLists)
            {
                IEnumerable<IContent> jobPosts = jobPostList.Children().Where(x => x.Published);
                foreach (IContent jobPost in jobPosts)
                {
                    IEnumerable<IContent> candidateList = jobPost.Children().Where(x => x.Published);
                    foreach (IContent candidate in candidateList)
                    {
                        candidate.SetValue("candidateStatus", status);
                        Services.ContentService.SaveAndPublishWithStatus(candidate);
                        if (status == "closed")
                        {
                            candidateEmailList.Add(candidate.GetValue("candidateEmail").ToString());
                        }

                    }

                    jobPost.SetValue("status", status);
                    Services.ContentService.SaveAndPublishWithStatus(jobPost);

                }

            }
            if (candidateEmailList.Count != 0 && candidateConfig != null)
            {
                ScheduleJob job = new ScheduleJob { SenderName = candidateConfig["emailSenderName"], Body = candidateConfig["emailBody"], Subject = candidateConfig["emailSubject"], TemplateFile = candidateConfig["emailTemplate"], BCC = string.Join<string>(",", candidateEmailList) };
                MailServiceController.MailService.SendMail(job);
            }
            position.SetValue("status", status);
            Services.ContentService.SaveAndPublishWithStatus(position);

        }

        //for filling the dropdown in the object that related position
        public List<SelectListItem> GetAllPositions(int entityId)
        {
            try
            {
                entityId = ((IPublishedContent)Umbraco.Content(entityId)).Ancestor().Id;
            }
            catch(Exception ex)
            {
                return new List<SelectListItem>();
            }
            List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("position", -1, -1);

            List<SelectListItem> positionList = new List<SelectListItem>();

            foreach (var content in contents)
            {
                if(Convert.ToInt32(content.GetPropertyValue("entityId"))==entityId)
                    positionList.Add(new SelectListItem { Text = content.GetPropertyValue("positionName").ToString(), Value = content.Id.ToString() });
            }

            return positionList;
        }
    }
}
