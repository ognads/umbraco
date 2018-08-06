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

namespace baseCMS.Controllers.Api
{
    public class EntityApiController : UmbracoApiController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";
        //GET: Employees
        /// <summary>
        ///  Get all entities in idendified content
        /// </summary>
        /// <param name="page"> Represents current page number for pagination </param>
        /// <param name="max"> Represents take amount</param>
        /// <param name="contentId"> Represents content (entity) id</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        public List<Entity> GetEntities(int page = 0, int max = 10, int contentId = 0)
        {
            ContentServiceController contentService = new ContentServiceController();
            List<IPublishedContent> contents = contentService.GetContentListByTypeAndParentId("entity", contentId, page, max);

            List<Entity> entityList = new List<Entity>();

            foreach (var content in contents)
            {

                Entity entity = new Entity
                {
                    ID = content.Id,
                    Name = content.GetPropertyValue("entityName").ToString(),
                    Organization = content.GetPropertyValue("organizationName").ToString(),
                    EmployeeListUrl = content.Children().First().Url
                    
                };

                entityList.Add(entity);
            }


            return entityList;
        }

        //for filling the dropdown in the object that related entity
        public static List<SelectListItem> GetAllEntities()
        {
            List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("entity", -1, -1);

            List<SelectListItem> entityList = new List<SelectListItem>();

            foreach (var content in contents)
            {
                entityList.Add(new SelectListItem { Text = content.GetPropertyValue("entityName").ToString(), Value = content.Id.ToString() });
            }

            return entityList;
        }

    }
}
