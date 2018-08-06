using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    public class InventorySurfaceController : SurfaceController
    {
        private const String CONTROLLER_URL = "/umbraco/surface/InventorySurface/";
        [MemberAuthorize(AllowGroup = "Admin,Information Technologies")]
        [HttpGet]
        public ActionResult GetAddForm()
        {
            return PartialView("~/Views/Partials/Inventory/Add.cshtml");
        }
        /// <summary>
        /// Return partial with the content's information in it
        /// </summary>
        /// <param name="contentId">Content that the information will be filled with</param>
        /// <returns>Partial view that is a edit form</returns>
        ///
        [MemberAuthorize(AllowGroup = "Admin,Information Technologies")]
        [HttpGet]
        public ActionResult GetEditForm(int contentId)
        {
            //Get content (employee) with id
            IPublishedContent content = (IPublishedContent)Umbraco.Content(contentId);
            string assignedTo =
            content.GetPropertyValue("assignedTo").ToString();
            if(assignedTo=="0" ||assignedTo =="" )
            {
                assignedTo = "Unassigned";
            }
            Inventory inventory = new Inventory {
                ID = content.Id,
                Name = content.GetPropertyValue("inventoryName").ToString(),
                WarrantyEndDate = Convert.ToDateTime(content.GetPropertyValue("inventoryWarrantyEndDate").ToString()),
                Invoice = content.GetPropertyValue("inventoryInvoice").ToString(),
                Cost = Convert.ToDecimal(content.GetPropertyValue("inventoryCost")),
                Description = content.GetPropertyValue("resourceDescription").ToString(),
                AssignedTo = assignedTo
            };
             return PartialView("~/Views/Partials/Inventory/Edit.cshtml", inventory);
        }
        
        /// <summary>
        /// Creates a new Inventory without assigning it
        /// </summary>
        /// <param name="inventory">Inventory that is needed to create a content</param>
        /// <param name="parentId"></param>
        /// <returns> Partial view that is acccording to the response</returns>
        [ValidateAntiForgeryToken]
        [MemberAuthorize(AllowGroup = "Admin,Information Technologies")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,Invoice,WarrantyEndDate,Cost,Description,AssignedTo")]Inventory inventory, int parentId = 0)
        {
            if (ModelState.IsValid)
            {
                TempData["elemId"] = "inventory";
                try
                {
                    var content = Services.ContentService.CreateContent(inventory.Name, parentId, "inventory");
                    content.SetValue("inventoryName", inventory.Name);
                    content.SetValue("inventoryInvoice", inventory.Invoice);
                    content.SetValue("inventoryWarrantyEndDate", inventory.WarrantyEndDate);
                    content.SetValue("inventoryCost", inventory.Cost);
                    content.SetValue("resourceDescription", inventory.Description);
                    content.SetValue("assignedTo", inventory.AssignedTo);
                    if (Services.ContentService.SaveAndPublishWithStatus(content).Success)
                    {
                        TempData["Msg"] = "Inventory successfully created";
                        return PartialView("~/Views/Partials/FormSuccess.cshtml");
                    }
                    else
                    {
                        TempData["Msg"] = "Inventory was not created";
                        return PartialView("~/Views/Partials/FormError.cshtml");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Inventory was not created";
                    return PartialView("~/Views/Partials/FormError.cshtml");
                }
            }
            else
            {
                TempData["Msg"] = "Inventory was not created";
                return PartialView("~/Views/Partials/FormError.cshtml");
            }
        }
        
        /// <summary>
        /// Edit the given Inventory
        /// </summary>
        /// <param name="inventory">Inventory that is being edited</param>
        /// <param name="parentId"></param>
        /// <returns>returns the partial view according to the response</returns>
        [ValidateAntiForgeryToken]
        [MemberAuthorize(AllowGroup = "Admin,Information Technologies")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Name,Invoice,WarrantyEndDate,Cost,Description,AssignedTo")]Inventory inventory, int parentId)
        {
            if(ModelState.IsValid){
                TempData["elemId"] = "inventory";
                var service = Services.ContentService;
                var content = service.GetById(inventory.ID);
                content.SetValue("inventoryName", inventory.Name);
                content.SetValue("inventoryInvoice", inventory.Invoice);
                content.SetValue("inventoryWarrantyEndDate", inventory.WarrantyEndDate);
                content.SetValue("inventoryCost", inventory.Cost);
                content.SetValue("assignedTo", inventory.AssignedTo);
                content.SetValue("resourceDescription", inventory.Description);
                if (inventory.AssignedTo == "Unassigned")
                    content.SetValue("assignedTo", 0);
                if (service.SaveAndPublishWithStatus(content).Success)
                {
                    TempData["Msg"] = "Inventory was edited";
                    return PartialView("~/Views/Partials/FormSuccess.cshtml");
                }
                else {
                    TempData["Msg"] = "Inventory was not edited";
                    return PartialView("~/Views/Partials/FormError.cshtml");
                }
                    
            }
            else{
                TempData["Msg"] = "Inventory was not created";
                return PartialView("~/Views/Partials/FormError.cshtml");
            }
        }
        /// <summary>
        /// Assigning the given inventory
        /// </summary>
        /// <param name="InventoryId">Id of the assigned Inventory</param>
        /// <param name="ContentId">Content that it's being assigned to</param>
        /// <returns>Partial according to the status</returns>
        public ActionResult AssignInventory(int InventoryId, int ContentId)
        {
            var service = Services.ContentService;
            IContent inventoryContent = service.GetById(InventoryId);
            IContent Content = service.GetById(ContentId);
            inventoryContent.SetValue("assignedTo", Content.Id);
            if (service.SaveAndPublishWithStatus(inventoryContent).Success)
            {

                return PartialView("~/Views/Partials/Employee/FormSuccess.cshtml");
            }
            else
                return PartialView("~/Views/Partials/Employee/FormError.cshtml");
        }

        public ActionResult GetAssignmentRateChart()
        {
            List<IPublishedContent> inventoryList = ContentServiceController.Instance.GetContentListByTypeAndParentId("inventory", -1, -1);
            int unassignedInventoryCount = inventoryList.Where(z => z.GetPropertyValue("assignedTo").ToString().Equals("0")).ToList().Count;
            int assignedInventoryCount = inventoryList.Count - unassignedInventoryCount;
            ChartData chartData = new ChartData();
            chartData.KeyValues.Add("Assigned", assignedInventoryCount);
            chartData.KeyValues.Add("Unassigned", unassignedInventoryCount);
            chartData.Title = "Assignment Rate";
            chartData.Style = "pie";
            return PartialView("~/Views/Partials/Charts/Chart.cshtml", chartData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthsLeft"></param>
        /// <returns></returns>
        public ActionResult GetInventoriesByWarrantDateChart(int monthsLeft = 5)
        {
            DateTime fiveMonthAgo = DateTime.Now.AddMonths(monthsLeft * -1);
            List<IPublishedContent> inventoryList = ContentServiceController.Instance.GetContentListByTypeAndParentId("inventory", -1, -1)
                .Where(z => !z.GetPropertyValue("inventoryWarrantyEndDate").ToString().Equals("") && DateTime.Parse(z.GetPropertyValue("inventoryWarrantyEndDate").ToString()).Date > fiveMonthAgo.Date).ToList();
            int inventoryWarranty;
            ChartData chartData = new ChartData();
            Dictionary<int, int> lister = new Dictionary<int, int>();
            for (int looper = 0; looper <= monthsLeft; looper++)
            {
                lister.Add(looper, 0);
            }
            foreach (IPublishedContent inventory in inventoryList)
            {
                inventoryWarranty = DateTime.Parse(inventory.GetPropertyValue("inventoryWarrantyEndDate").ToString()).Subtract(fiveMonthAgo).Days / (365 / 12);
                if (inventoryWarranty <= 5)
                    lister[inventoryWarranty] += 1;
            }
            foreach (KeyValuePair<int, int> per in lister)
            {
                chartData.KeyValues.Add(per.Key.ToString(), per.Value);
            }
            chartData.Title = "Inventories by Warranty Date";
            chartData.Style = "line";
            return PartialView("~/Views/Partials/Charts/Chart.cshtml", chartData);
        }

        /// <summary>
        /// Gets all possible charts of this seciton as list of string
        /// FOR FUTHER REFERENCE PLEASE ADD ADDITIONAL CHARTS HERE
        /// </summary>
        /// <returns>List of possible controller links</returns>
        public List<String> GetCharts()
        {
            List<String> controllerUrls = new List<String>();
            controllerUrls.Add(CONTROLLER_URL + "GetAssignmentRateChart");
            controllerUrls.Add(CONTROLLER_URL + "GetInventoriesByWarrantDateChart");
            return controllerUrls;
        }
    }
}
