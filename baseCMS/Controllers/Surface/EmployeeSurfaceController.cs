using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using baseCMS.Helpers;
using baseCMS.Models;
using baseCMS.Services;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
    public class EmployeeSurfaceController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";
        private const String CONTROLLER_URL = "/umbraco/surface/EmployeeSurface/";
        /// <summary>
        ///  Get employee add form to show in modal
        /// </summary>
        /// <returns>Partial view</returns>
        ///
        [HttpGet]
        public ActionResult GetAddForm()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "Employee/Add.cshtml");
        }
        /// <summary>
        ///  Get employee edit form with data on it to show in modal
        /// </summary>
        /// <param name="contentId"> employee id</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public ActionResult GetEditForm(int contentId)
        {
            //Get content (employee) with id
            IPublishedContent content = (IPublishedContent)Umbraco.Content(contentId);
            IPublishedContent address = content;
            try
            {

                address = content.Descendants().Where(a => a.DocumentTypeAlias == "address").First();
            }
            catch (Exception ex)
            {

            }
            //string date = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday", "01-01-2000")).ToString("MM/dd/yyyy");
            //set emplyee to employee model
            Employee employee = new Employee
            {
                ID = content.Id,
                Name = content.GetPropertyValue("employeeName", "").ToString(),
                MiddleName = content.GetPropertyValue("employeeMiddleName", "").ToString(),
                LastName = content.GetPropertyValue("employeeSurname", "").ToString(),
                Gender = content.GetPropertyValue("employeeGender", "").ToString(),
                //BirthDay = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday", "1900-01-01")),
                BirthDay = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday", "01-01-2000")),
                Salary = Convert.ToDecimal(content.GetPropertyValue("employeeSalary", 0)),
                Email = content.GetPropertyValue("employeeEmail", "").ToString(),
                //Tags = content.GetPropertyValue("resourceTags").ToString(),
                PositionID = ((IPublishedContent)content.GetPropertyValue("position")).Id,
                AddressLine1 = address.GetPropertyValue("addressDescriptionLine1", "").ToString(),
                AddressLine2 = address.GetPropertyValue("addressDescriptionLine2", "").ToString(),
                Country = address.GetPropertyValue("countryName", "").ToString(),
                Latitude = address.GetPropertyValue("latitude", "").ToString(),
                Longitude = address.GetPropertyValue("longtitude", "").ToString(),
                PostalCode = address.GetPropertyValue("postalCode", "").ToString(),
                State = address.GetPropertyValue("stateName", "").ToString()
            };
            //return to partial view with employee
            return PartialView(PARTIAL_VIEW_FOLDER + "Employee/Edit.cshtml", employee);
        }
        /// <summary>
        ///  Get employee detail edit form with data on it to show in detail page
        /// </summary>
        /// <param name="contentId"> employee id</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public ActionResult GetEditDetailForm(int contentId)
        {
            //Get content (employee) with id
            IPublishedContent content = (IPublishedContent)Umbraco.Content(contentId);
            IPublishedContent address = content;
            try
            {

                address = content.Descendants().Where(a => a.DocumentTypeAlias == "address").First();
            }
            catch (Exception ex)
            {

            }
            //string date = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday", "01-01-2000")).ToString("MM/dd/yyyy");
            //set emplyee to employee model
            Employee employee = new Employee
            {
                ID = content.Id,
                Name = content.GetPropertyValue("employeeName", "").ToString(),
                MiddleName = content.GetPropertyValue("employeeMiddleName", "").ToString(),
                LastName = content.GetPropertyValue("employeeSurname", "").ToString(),
                Gender = content.GetPropertyValue("employeeGender", "").ToString(),
                //BirthDay = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday", "1900-01-01")),
                BirthDay = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday", "01-01-2000")),
                Salary = Convert.ToDecimal(content.GetPropertyValue("employeeSalary", 0)),
                Email = content.GetPropertyValue("employeeEmail", "").ToString(),
                //Tags = content.GetPropertyValue("resourceTags").ToString(),
                PositionID = ((IPublishedContent)content.GetPropertyValue("position")).Id,
                AddressLine1 = address.GetPropertyValue("addressDescriptionLine1", "").ToString(),
                AddressLine2 = address.GetPropertyValue("addressDescriptionLine2", "").ToString(),
                Country = address.GetPropertyValue("countryName", "").ToString(),
                Latitude = address.GetPropertyValue("latitude", "").ToString(),
                Longitude = address.GetPropertyValue("longtitude", "").ToString(),
                PostalCode = address.GetPropertyValue("postalCode", "").ToString(),
                KnownIssues = content.GetPropertyValue("knownIssues", "").ToString(),
                State = address.GetPropertyValue("stateName", "").ToString(),
                SocialSecurityNo =content.GetPropertyValue("socialSecurityNo", "").ToString(),
                NationalID = content.GetPropertyValue("nationalIDNo", "").ToString(),
                PhoneNumber = content.GetPropertyValue("phoneNo", "").ToString(),
                AlternativeEmail = content.GetPropertyValue("alternativeEmail", "").ToString(),
                PositionDescription = ((IPublishedContent)content.GetPropertyValue("position")).GetPropertyValue("positionDescription").ToString()
            };
            //return to partial view with employee
            return PartialView(PARTIAL_VIEW_FOLDER + "Employee/EditDetail.cshtml", employee);
        }
        /// <summary>
        ///  Create new employee in specific content
        /// </summary>
        /// <param name="parentId"> Represents parent content's (entity) id</param>
        /// <param name="employee"> Represents employee </param>
        /// <returns>Partial view</returns>
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,LastName,MiddleName,Email,Gender,Birthday,Salary,AddressLine1,AddressLine2,PostalCode,Longitude,Latitude,PositionID,Tags,Country,State")] Employee employee, int parentId = 0)
        {
            TempData["elemId"] = "employee";
            if (ModelState.IsValid)
            {
                string guid = Guid.NewGuid().ToString().GetHashCode().ToString("x");
                try
                {
                    var newEmployee = Services.ContentService.CreateContent(employee.Name + " " + employee.LastName, parentId, "employee");

                    string locaUdi = Services.ContentService.GetById(employee.PositionID).GetUdi().ToString();
                    
                    newEmployee.SetValue("employeeName", employee.Name);
                    newEmployee.SetValue("employeeMiddleName", employee.MiddleName);
                    newEmployee.SetValue("employeeSurname", employee.LastName);
                    newEmployee.SetValue("employeeGender", employee.Gender);
                    newEmployee.SetValue("employeeBirthday", employee.BirthDay);
                    newEmployee.SetValue("employeeSalary", employee.Salary);
                    newEmployee.SetValue("employeeEmail", employee.Email);
                    newEmployee.SetValue("resourceTags", employee.Tags);
                    newEmployee.SetValue("position", locaUdi.ToString());
                    
                    if (Services.ContentService.SaveAndPublishWithStatus(newEmployee).Success)
                    {
                        var newAddress = Services.ContentService.CreateContent(newEmployee.Name + " Address", newEmployee.Id, "address");
                        newAddress.SetValue("postalCode", employee.PostalCode);
                        newAddress.SetValue("longtitude", employee.Longitude);
                        newAddress.SetValue("latitude", employee.Latitude);
                        newAddress.SetValue("addressType", "Home Address");
                        newAddress.SetValue("addressDescriptionLine1", employee.AddressLine1);
                        newAddress.SetValue("addressDescriptionLine2", employee.AddressLine2);
                        newAddress.SetValue("stateName", employee.State);
                        newAddress.SetValue("countryName", employee.Country);
                        newAddress.SetValue("iSOCode", employee.Country);

                        if (Services.ContentService.SaveAndPublishWithStatus(newAddress).Success)
                        {
                            TempData["Msg"] = "Employee successfully created";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");
                        }
                        else
                        {
                            TempData["Msg"] = "Employee created but address cannot. Check the logs.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                        }
                    }
                    TempData["Msg"] = "Cannot create employee.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Cannot create employee.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
            }
            TempData["Msg"] = "Cannot create employee.";
            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
        }
        /// <summary>
        ///  Edit employee in specific content
        /// </summary>
        /// <param name="parentId"> Represents parent content's (entity) id</param>
        /// <param name="employee"> Represents employee </param>
        /// <returns>Partial view</returns>
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,LastName,MiddleName,Email,Gender,Birthday,Salary,AddressLine1,AddressLine2,PostalCode,Longitude,Latitude,PositionID,Tags,Country,State")] Employee employee, int parentId)
        {
            TempData["elemId"] = "employee";
            if (ModelState.IsValid)
            {
                try
                {
                    var service = Services.ContentService;
                    var oldEmployee = service.GetById(employee.ID);
                    string locaUdi = Services.ContentService.GetById(employee.PositionID).GetUdi().ToString();

                    oldEmployee.SetValue("employeeName", employee.Name);
                    oldEmployee.SetValue("employeeMiddleName", employee.MiddleName);
                    oldEmployee.SetValue("employeeSurname", employee.LastName);
                    oldEmployee.SetValue("employeeGender", employee.Gender);
                    oldEmployee.SetValue("employeeBirthday", employee.BirthDay);
                    oldEmployee.SetValue("employeeSalary", employee.Salary);
                    oldEmployee.SetValue("employeeEmail", employee.Email);
                    oldEmployee.SetValue("position", locaUdi.ToString());

                    if (service.SaveAndPublishWithStatus(oldEmployee).Success)
                    {
                        IContent oldAddress;
                        try
                        {
                            oldAddress = service.GetChildren(employee.ID).Where(a => a.ContentType.Alias == "address").First();
                        }
                        catch (Exception ex)
                        {
                            oldAddress = service.CreateContent(employee.Name + " Address", employee.ID, "address");
                        }

                        oldAddress.SetValue("postalCode", employee.PostalCode);
                        oldAddress.SetValue("longtitude", employee.Longitude);
                        oldAddress.SetValue("latitude", employee.Latitude);
                        oldAddress.SetValue("addressType", "Home Address");
                        oldAddress.SetValue("addressDescriptionLine1", employee.AddressLine1);
                        oldAddress.SetValue("addressDescriptionLine2", employee.AddressLine2);
                        oldAddress.SetValue("stateName", employee.State);
                        oldAddress.SetValue("countryName", employee.Country);
                        oldAddress.SetValue("iSOCode", employee.Country);
                        if (service.SaveAndPublishWithStatus(oldAddress).Success)
                        {
                            TempData["Msg"] = "Employee edited successfully.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");
                        }
                        else
                        {
                            TempData["Msg"] = "Employee edited but address cannot.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                        }
                    }
                    TempData["Msg"] = "Cannot edit employee.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
                catch (Exception ex)
                {

                    TempData["Msg"] = "Cannot edit employee.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
            }

            TempData["Msg"] = "Cannot edit employee.";
            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

        }
        /// <summary>
        ///  Edit employee detail in specific content
        /// </summary>
        /// <param name="parentId"> Represents parent content's (entity) id</param>
        /// <param name="employee"> Represents employee </param>
        /// <returns>Partial view</returns>
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail(Employee employee)
        {
            TempData["elemId"] = "employee";
            if (ModelState.IsValid)
            {
                try
                {
                    var service = Services.ContentService;
                    var oldEmployee = service.GetById(employee.ID);

                    oldEmployee.SetValue("employeeName", employee.Name);
                    oldEmployee.SetValue("employeeMiddleName", employee.MiddleName);
                    oldEmployee.SetValue("employeeSurname", employee.LastName);
                    oldEmployee.SetValue("employeeGender", employee.Gender);
                    oldEmployee.SetValue("employeeBirthday", employee.BirthDay);
                    oldEmployee.SetValue("employeeSalary", employee.Salary);
                    oldEmployee.SetValue("employeeEmail", employee.Email);
                    oldEmployee.SetValue("socialSecurityNo", employee.SocialSecurityNo);
                    oldEmployee.SetValue("nationalIDNo", employee.NationalID);
                    oldEmployee.SetValue("phoneNo", employee.PhoneNumber);
                    oldEmployee.SetValue("knownIssues", employee.KnownIssues);
                    oldEmployee.SetValue("alternativeEmail", employee.AlternativeEmail);


                    if (service.SaveAndPublishWithStatus(oldEmployee).Success)
                    {
                        TempData["Msg"] = "Success.";
                        return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccessDetail.cshtml");
                    }
                        
                }
                catch (Exception ex)
                {

                    TempData["Msg"] = "Cannot edit employee.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
            }

            TempData["Msg"] = "Cannot edit employee.";
            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

        }

        public ActionResult GetEmployeeByGender()
        {
            List<IPublishedContent> employee = ContentServiceController.Instance.GetContentListByTypeAndParentId("employee", -1, -1);
            ChartData chartData = new ChartData();
            chartData.KeyValues.Add("Male", employee.Where(x => x.GetPropertyValue("employeeGender").ToString().Equals("Male")).ToList().Count);
            chartData.KeyValues.Add("Female", employee.Where(x => x.GetPropertyValue("employeeGender").ToString().Equals("Female")).ToList().Count);
            chartData.Title = "Employee By Gender";
            chartData.Style = "pie";
            return PartialView("~/Views/Partials/Charts/Chart.cshtml", chartData);
        }

        public ActionResult GetEmployeesByPosition()
        {
            ChartData chartData = new ChartData();
            List<IPublishedContent> positionList = ContentServiceController.Instance.GetContentListByTypeAndParentId("position", -1, -1);
            foreach (IPublishedContent position in positionList)
            {
                chartData.KeyValues.Add(position.GetPropertyValue("positionName").ToString(), ContentServiceController.Instance.GetContentListByTypeAndParentId("employee", -1, -1).Where(x =>((IPublishedContent)x.GetPropertyValue("position")).Id == position.Id).ToList().Count);
            }
            chartData.Title = "Employees By Position";
            chartData.Style = "pie";
            return PartialView("~/Views/Partials/Charts/Chart.cshtml", chartData);
        }
        public ActionResult GetEmployeesByEntities()
        {
            ChartData chartData = new ChartData();
            List<IPublishedContent> entity = ContentServiceController.Instance.GetContentListByTypeAndParentId("entity", -1, -1);
            foreach (IPublishedContent content in entity)
            {
                chartData.KeyValues.Add(content.GetPropertyValue("entityName").ToString(), content.Descendants().Where(x => x.DocumentTypeAlias.Equals("employee")).ToList().Count);
            }
            chartData.Title = "Employees By Entity";
            chartData.Style = "bar";
            return PartialView("~/Views/Partials/Charts/Chart.cshtml", chartData);
        }
        public List<String> GetCharts()
        {
            List<String> controllerUrls = new List<String>();
            controllerUrls.Add(CONTROLLER_URL + "GetEmployeesByPosition");
            controllerUrls.Add(CONTROLLER_URL + "GetEmployeesByEntities");
            controllerUrls.Add(CONTROLLER_URL + "GetEmployeeByGender");
            return controllerUrls;
        }
        /// <summary>
        /// Sets the given imageUrl to the related id's image
        /// This also sets the picture of the employee if there's any
        /// This should be called after the file uploaded to the system
        /// </summary>
        /// <param name="id">Id of the employee</param>
        /// <param name="imageUrl">Url of the Image</param>
        public void SetPicture(int id, String imageUrl)
        {
            IContent employee = Services.ContentService.GetById(id);
            if (imageUrl != null || !imageUrl.Equals(""))
            {
                MemberHelper memberHelper = new MemberHelper();
                employee.SetValue("employeeImageUrl", imageUrl);
                IContent member = (IContent)memberHelper.GetByEmail(employee?.GetValue("employeeEmail")?.ToString());
                try
                {
                    MemberServiceController memberService = new MemberServiceController();
                    memberService.SetMemberImage(member.Id, imageUrl);
                }
                catch (NullReferenceException nullException)
                {
                    CustomLogHelper.logHelper.Log(
                        "This should be fixed. A new member can be created with the given properties. There's no member for the" + employee.Name +
                        "Caught a null reference exception" + nullException.Message);
                }
                Services.ContentService.SaveAndPublishWithStatus(employee);

            }
            else
            {
                //TODO may return an error message later on
            }

        }
    }
}
