using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    [MemberAuthorize(AllowGroup ="Human Resources,Admin")]
    public class EntitySurfaceController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";
        // GET: EntityCreateForm
        [HttpGet]
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        public ActionResult GetAddForm()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "Entity/Add.cshtml");
        }
        // GET: EntityUpdateForm
        [HttpGet]
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        public ActionResult GetEditForm(int contentId = 0)
        {
            if (contentId != 0)
            {
                IPublishedContent content = Umbraco.Content(contentId);
                Entity entity = new Entity
                {
                    ID = content.Id,
                    Name = content.GetPropertyValue("entityName").ToString(),
                    Description = content.GetPropertyValue("entityDescription").ToString(),
                    Organization = content.GetPropertyValue("organizationName").ToString()

                };
                return PartialView(PARTIAL_VIEW_FOLDER + "Entity/Edit.cshtml", entity);
            }
            return PartialView(PARTIAL_VIEW_FOLDER + "Entity/Edit.cshtml");
        }
        //POST:CreateEntity
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        public ActionResult CreateEntity(Entity entity)
        {
            TempData["elemId"] = "entity";
            if (ModelState.IsValid)
            {
                IContent content = Services.ContentService.CreateContent(entity.Name, 5016, "entity");
                content.SetValue("entityName", entity.Name);
                content.SetValue("entityDescription", entity.Description);
                content.SetValue("organizationName", entity.Organization);
                if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                {
                    IContent employeeList = Services.ContentService.CreateContent(entity.Name + " employees", content.Id, "employeeList");
                    if (Services.ContentService.SaveAndPublishWithStatus(employeeList).Success)
                    {
                        TempData["Msg"] = "Entity successfully created";
                        return PartialView("~/Views/Partials/FormSuccess.cshtml");
                    }
                }
            }
            TempData["Msg"] = "Cannot create entity";
            return PartialView("~/Views/Partials/FormError.cshtml");
        }
        //POST:UpdateEntity
        [HttpPost]//Only allow post method
        [ValidateAntiForgeryToken]//Checking token
        [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
        public ActionResult UpdateEntity(Entity entity)
        {
            TempData["elemId"] = "entity";
            if (ModelState.IsValid)
            {
                IContent content = Services.ContentService.GetById(entity.ID);
                content.SetValue("entityName", entity.Name);
                content.SetValue("entityDescription", entity.Description);
                content.SetValue("organizationName", entity.Organization);
                if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                {
                    TempData["Msg"] = "Entity successfully edited";
                    return PartialView("~/Views/Partials/FormSuccess.cshtml");
                }
            }
            TempData["Msg"] = "Cannot edit entity";
            return PartialView("~/Views/Partials/FormError.cshtml");
        }

        [HttpGet]
        public ActionResult DrawChart()
        {
            List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("entity", -1, -1);
            ChartData data = new ChartData();
            
            foreach (IPublishedContent content in contents)
            {
                //data.Label.Add(content.GetPropertyValue("entityName").ToString());
                //data.Data.Add(content.Children.Count());
            }
            return PartialView(PARTIAL_VIEW_FOLDER + "Charts/PieChart.cshtml", data);
        }
    }
}
