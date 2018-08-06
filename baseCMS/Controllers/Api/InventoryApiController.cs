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
    public class InventoryApiController : UmbracoApiController
    {
        [HttpGet]
        [MemberAuthorize(AllowGroup = "Admin,Information Technologies")]
        public List<Inventory> GetInventories(int page = 0, int max = 10, int contentId = 0)
        {
            List<IPublishedContent> contents = ContentServiceController.Instance
                .GetContentListByTypeAndParentId("inventory", contentId, page, max);

            List<Inventory> inventoryList = new List<Inventory>();
            foreach (var content in contents)
            {
                Inventory inventory = new Inventory
                {
                    ID = content.Id,
                    Name = content.GetPropertyValue("inventoryName").ToString(),
                    Invoice = content.GetPropertyValue("inventoryInvoice").ToString(),
                    WarrantyEndDate = Convert.ToDateTime(content.GetPropertyValue("inventoryWarrantyEndDate").ToString()),
                    Cost = Convert.ToDecimal(content.GetPropertyValue("inventoryCost")),
                    Description = content.GetPropertyValue("resourceDescription").ToString(),
                    AssignedTo = getAssignedTo(content),
                    InventoryUrl = content.Url
                };

                inventoryList.Add(inventory);
            }
            return inventoryList;
        }


        private string getAssignedTo(IPublishedContent inventoryContent)
        {
            string assignedTo = "Unassigned";
            object assignedToId = inventoryContent.GetPropertyValue("assignedTo");
            if (assignedToId.ToString() != "0" && assignedToId.ToString() != ""  )
            {
                IPublishedContent assignedToContent = Umbraco.Content(assignedToId);

                switch (assignedToContent.DocumentTypeAlias)
                {
                    case "employee":
                        assignedTo = "Employee-" + assignedToContent.GetPropertyValue("employeeName").ToString() + ' ' + assignedToContent.GetPropertyValue("employeeSurname").ToString();
                        break;
                    case "inventory":
                        assignedTo = "Inventory-" + assignedToContent.GetPropertyValue("inventoryName").ToString() + assignedToContent.Id;
                        break;
                    case "entity":
                        assignedTo = "Entity-" + assignedToContent.GetPropertyValue("entityName").ToString();
                        break;
                    default:
                        assignedTo = "Unassigned";
                        break;
                }
            }
            return assignedTo;

        }

        //for filling the dropdown in inventory assignedTo field
        public static List<SelectListItem> GetAssignedables()
        {
            List<IPublishedContent> entities = ContentServiceController.Instance.GetContentListByTypeAndParentId("entity", -1, -1);

            List<IPublishedContent> employees = ContentServiceController.Instance.GetContentListByTypeAndParentId("employee", -1, -1);

            List<IPublishedContent> inventories = ContentServiceController.Instance.GetContentListByTypeAndParentId("inventory", -1, -1);


            List<SelectListItem> optionList = new List<SelectListItem>();
            SelectListGroup entityGroup = new SelectListGroup { Name = "Entity" };

            foreach (var content in entities)
            {
                optionList.Add(new SelectListItem { Text = content.GetPropertyValue("entityName").ToString(), Value = content.Id.ToString(), Group = entityGroup });
            }

            SelectListGroup employeeGroup = new SelectListGroup { Name = "Employee" };

            foreach (var content in employees)
            {
                optionList.Add(new SelectListItem { Text = content.GetPropertyValue("employeeName").ToString() + ' ' + content.GetPropertyValue("employeeSurname").ToString(), Value = content.Id.ToString(), Group = employeeGroup });
            }

            SelectListGroup inventoryGroup = new SelectListGroup { Name = "Inventory" };

            foreach (var content in inventories)
            {
                optionList.Add(new SelectListItem { Text = content.GetPropertyValue("inventoryName").ToString(), Value = content.Id.ToString(), Group = inventoryGroup });
            }


            return optionList;
        }

    }
}
