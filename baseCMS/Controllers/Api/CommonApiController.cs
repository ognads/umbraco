using baseCMS.Models;
using baseCMS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers.Api
{
    public class CommonApiController : UmbracoApiController
    {
        /// <summary>
        /// Deletes the content of that Id
        /// </summary>
        /// <param name="contentId">Id of the content</param>
        [HttpDelete]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public void Delete(int contentId)
        {
            IContent parentContent = Services.ContentService.GetById(contentId);

            //Deleting child attachment files
            DeleteChildAttachmentFiles(parentContent);

            Services.ContentService.UnPublish(parentContent);
        }

        /// <summary>
        /// Deleting all child attachment files
        /// </summary>
        /// <param name="parentContent"></param>
        private void DeleteChildAttachmentFiles(IContent parentContent)
        {
            //We are getting all child attachments and deleting the files that attachments has
            IEnumerable<IContent> childAttachmentContents = parentContent.Descendants().Where(x => x.ContentType.Alias == "attachment");
            foreach (var content in childAttachmentContents)
            {
                string attachmentUrl = content.GetValue("attachmentUrl").ToString();
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~" + attachmentUrl);


                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }

        }

        [HttpGet]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public List<Tag> GetTags(string type)
        {
            List<Tag> tagList = new List<Tag>();

            var tags = Umbraco.TagQuery.GetAllTags(type);
            foreach (var tag in tags)
            {
                tagList.Add(new Tag { Text = tag.Text, Id = tag.Id });
            }
            return (tagList);
        }

        [HttpPost]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public string SetTags(string type, int contentId, string tags)
        {
            var service = Services.ContentService;
            IContent content = service.GetById(contentId);
            content.SetTags(TagCacheStorageType.Json, "resourceTags", string.IsNullOrEmpty(tags) ? new string[] { "" } : tags.Split(','), true, type);
            service.SaveAndPublishWithStatus(content);
            return "Success";
        }
        /// <summary>
        /// Updates a value of an existing content
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyAlias"></param>
        /// <param name="newValue"></param>
        /// <returns>int</returns>
        [HttpPost]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public int UpdateValueOfContent(int contentId, string propertyAlias, string newValue)
        {
            IContent content = Services.ContentService.GetById(contentId);
            if (content.HasProperty(propertyAlias))
            {

                if (newValue.Trim() == "" || (content.GetValue(propertyAlias) != null &&
                    newValue == content.GetValue(propertyAlias).ToString()))
                {
                    return 400;
                }
                content.SetValue(propertyAlias, newValue);
                if (Services.ContentService.SaveAndPublishWithStatus(content))
                {
                    return 200;
                }
                else
                {
                    return 400;
                }
            }

            return 400;

        }
        /// <summary>
        /// Gets all the social medias under any given content
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="page"></param>
        /// <param name="max"></param>
        /// <returns>List<SocialMedia></returns>
        [HttpGet]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public List<SocialMedia> GetSocialMedia(int contentId, int page = 0, int max = 10)
        {
            IPublishedContent publishedContent = Umbraco.Content(contentId);
            List<IPublishedContent> contents = ContentServiceController.
                Instance.GetContentListByTypeAndParentId("socialMediaInformation", contentId, page, max);
            List<SocialMedia> socialMedias = new List<SocialMedia>();

            foreach (IPublishedContent content in contents)
            {
                SocialMedia socialMedia = new SocialMedia
                {
                    ID = content.Id,
                    Name = content.GetPropertyValue("socialMediaName").ToString(),
                    Url = content.GetPropertyValue("socialMediaUrl").ToString()
                };

                socialMedias.Add(socialMedia);
            }


            return socialMedias;
        }
        /// <summary>
        /// Gets all the health events under any given content
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="page"></param>
        /// <param name="max"></param>
        /// <returns>List<HealthEvent></returns>
        [HttpGet]
        [Umbraco.Web.WebApi.MemberAuthorize]
        public List<HealthEvent> GetHealth(int contentId, int page = 0, int max = 10)
        {
            IPublishedContent publishedContent = Umbraco.Content(contentId);
            List<IPublishedContent> contents = ContentServiceController.
                Instance.GetContentListByTypeAndParentId("healthEvent", contentId, page, max);
            List<HealthEvent> healthEvents = new List<HealthEvent>();

            foreach (IPublishedContent content in contents)
            {
                HealthEvent healthEvent = new HealthEvent
                {
                    ID = content.Id,
                    Title = content.GetPropertyValue("healthEventTitle").ToString(),
                    Description = content.GetPropertyValue("HealthEventDescription").ToString()
                };

                healthEvents.Add(healthEvent);
            }


            return healthEvents;
        }
        
    }
}
