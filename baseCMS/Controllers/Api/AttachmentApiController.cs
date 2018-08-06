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

    [Umbraco.Web.WebApi.MemberAuthorize]
    public class AttachmentApiController : UmbracoApiController
    {

        /// <summary>
        /// Getting attachments by parent
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns> List<Attachment> attachments</returns>
        [HttpGet]
        public List<Attachment> GetAttachmentsByParent(int parentId)
        {
            List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("attachment", parentId, -1);

            List<Attachment> attachmentList = new List<Attachment>();

            //Mapping attachments contents to attachments model
            foreach (var content in contents)
            {
                Attachment attachment = new Attachment
                {
                    ID = content.Id,
                    AttachmentName = content.GetPropertyValue("attachmentName", "").ToString(),
                    AttachmentUrl = content.GetPropertyValue("attachmentUrl", "").ToString(),
                    AttachmentVersion = content.GetPropertyValue("versionNumber", "").ToString(),
                    CreatedAt = Convert.ToDateTime(content.GetPropertyValue("createdAt")),
                    CreatedBy = content.GetPropertyValue("createdBy", "").ToString(),
                    UpdatedAt = Convert.ToDateTime(content.GetPropertyValue("updatedAt", null)),
                    UpdatedBy = content.GetPropertyValue("updatedBy", "").ToString(),
                };

                attachmentList.Add(attachment);
            }

            return attachmentList;
        }
    }
}
