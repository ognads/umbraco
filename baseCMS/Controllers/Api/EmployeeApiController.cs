using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
    public class EmployeeApiController : UmbracoApiController
    {
        //GET: Employees
        /// <summary>
        ///  Get all employees in idendified content
        /// </summary>
        /// <param name="page"> Represents current page number for pagination </param>
        /// <param name="max"> Represents take amount</param>
        /// <param name="contentId"> Represents content (employee) id</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public List<Employee> GetEmployees(int page = 0, int max = 10, int contentId = 0)
        {
            
            List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("employee", contentId, page, max);

            List<Employee> employeeList = new List<Employee>();

            foreach (var content in contents)
            {
                
                Employee employee = new Employee
                {
                    ID = content.Id,
                    Name = content.GetPropertyValue("employeeName").ToString(),
                    LastName = content.GetPropertyValue("employeeSurname").ToString(),
                    Gender = content.GetPropertyValue("employeeGender").ToString(),
                    Salary = Convert.ToDecimal(content.GetPropertyValue("employeeSalary")),
                    BirthDay = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday")),
                    Email = content.GetPropertyValue("employeeEmail").ToString(),
                    EmployeeUrl = content.Url
                };

                employeeList.Add(employee);
            }


            return employeeList;
        }
        //dummy positions and entities
        //when there are entity and postiion controllers, data will be called from there
        
        public static List<SelectListItem> GetPositions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Text="position1",Value="position1"},
                new SelectListItem{Text="position2",Value="position2"},
                new SelectListItem{Text="position3",Value="position3"},
                new SelectListItem{Text="position4",Value="position4"}
            };
        }
        public static List<SelectListItem> GetEntities()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<IPublishedContent> contentList = ContentServiceController.Instance.GetContentListByTypeAndParentId("entity", 1525, -1);
            selectList.Add(new SelectListItem { Text = "Select organization...", Value = "0" });
            foreach (IPublishedContent content in contentList)
            {
                string entityName = content.GetPropertyValue("entityName").ToString();
                SelectListItem item = new SelectListItem { Text = entityName, Value = entityName };
                selectList.Add(item);
            }
            return selectList;

        }
        public static List<SelectListItem> GetOrganizationNames()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Text="entity1",Value="entity1"},
                new SelectListItem{Text="entity2",Value="entity2"},
                new SelectListItem{Text="entity3",Value="entity3"},
                new SelectListItem{Text="entity4",Value="entity4"}
            };
        }
    }
}
