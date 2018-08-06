using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using baseCMS.Models;
using baseCMS.Services;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace baseCMS.Controllers.Surface {
    public class MemberSurfaceController : Controller {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";
        // GET: MemberSurface
        public ActionResult Index () {
            return View ();
        }

        /// <summary>
        /// Gets the sidebar info about the Member's connected employee
        /// Does not contain any info about the member Just the employee
        /// </summary>
        /// <returns>Partial view of the sidebar</returns>
        [HttpGet]
        public ActionResult GetSideBarProfile () {
            Employee employee = new Employee ();
            MemberHelper memberHelper = new MemberHelper();
            IPublishedContent content = memberHelper.GetEmployeeFromMember (
                memberHelper.GetCurrentMember ());

            employee.Name = content.GetPropertyValue ("employeeName").ToString ();
            employee.LastName = content.GetPropertyValue ("employeeSurname").ToString ();
            employee.Gender = content.GetPropertyValue ("employeeGender").ToString ();
            employee.Salary = Convert.ToDecimal (content.GetPropertyValue ("employeeSalary"));
            employee.BirthDay = Convert.ToDateTime (content.GetPropertyValue ("employeeBirthday"));
            employee.Email = content.GetPropertyValue ("employeeEmail").ToString ();
            employee.AddressLine1 = content.GetPropertyValue ("addressDescriptionLine1", "").ToString ();
            employee.AddressLine2 = content.GetPropertyValue ("addressDescriptionLine2", "").ToString ();
            employee.Country = content.GetPropertyValue ("countryName", "").ToString ();
            employee.State = content.GetPropertyValue ("stateName", "").ToString ();
            return PartialView (PARTIAL_VIEW_FOLDER + "_rightMenu", employee);
        }
    }
}
