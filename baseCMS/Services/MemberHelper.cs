using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using baseCMS.Helpers;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace baseCMS.Services {
    public class MemberHelper : MembershipHelper {
        public MemberHelper (UmbracoContext context) : base (context) {

        }

        public MemberHelper () : base (UmbracoContext.Current) {

        }

        public String GetEmployeeMediaFromMemberId (int id = -1) {
            switch (id) {
                case -1:
                    return ConfigurationApiController.Instance.GetConfiguration () ["personImageNotFound"];;
                default:
                    IPublishedContent employee = this.GetEmployeeFromMember (this.GetById (id)); // returns the member's connected employee
                    try {
                        return employee.GetPropertyValue ("employeeImage").ToString ();
                    } catch (Exception ex) {
                        CustomLogHelper.logHelper.Log ("Warning: Could not find the meedia for the" + id,ex);
                        return ConfigurationApiController.Instance.GetConfiguration () ["personImageNotFound"];;
                    }
            }
        }
        public IPublishedContent GetEmployeeFromMember (IPublishedContent member) {
            return ContentServiceController.Instance.GetContentListByTypeAndParentId ("employee", -1, -1)?.Where (x => x.GetPropertyValue ("employeeEmail")
                .Equals (member.GetPropertyValue ("Email"))).First<IPublishedContent> ();
        }
        /// <summary>
        /// Finds the connected Employee by the member's email
        /// </summary>
        /// <param name="email">Email of the member</param>
        /// <returns>returns the Employee as IPublishedContent && returns null if there's none</returns>
        public IPublishedContent GetEmployeeByMemberEmail (String email) {
            try {
                IPublishedContent employee = ContentServiceController.Instance.GetContentListByTypeAndParentId ("employee", -1, -1) ?
                    .Where (x => x.GetPropertyValue ("employeeEmail").Equals (email))?.First ();
                return employee;
            } catch (Exception ex) {
                CustomLogHelper.logHelper.Log ("related employees details could not be found");
                return null;
            }
        }
        /// <summary>
        /// Acquires the member's image if there's any
        /// </summary>
        /// <param name="id">Id of the member</param>
        /// <returns>Url of the image && null if empty</returns>
        public String GetMemberMedia (int id = -1) {
            switch (id) {
                case -1:
                    return ConfigurationApiController.Instance.GetConfiguration () ["personImageNotFound"];;
                default:
                    IPublishedContent member = this.GetById (id);
                    try {
                        return member.GetPropertyValue ("memberImageUrl").ToString ();
                    } catch (Exception ex) {
                        CustomLogHelper.logHelper.Log ("Warning: Could not find the meedia for the" + id,ex);
                        return ConfigurationApiController.Instance.GetConfiguration () ["personImageNotFound"];;
                    }
                    //TODO This actually should be done with the base class so but our base classes are currenty static and hard coded.
            }
        }
    }
}
