using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using baseCMS.Helpers;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace baseCMS.Services
{
    public class MemberServiceController : SurfaceController
    {

        protected MemberHelper memberShipHelper;

        public MemberServiceController(UmbracoContext context)
        {
            memberShipHelper = new MemberHelper(context);
        }
        public MemberServiceController(){
            this.memberShipHelper=new MemberHelper();
        }

        /// <summary>
        /// Return the member's photo Url
        /// </summary>
        /// <param name="id">id of the member as int</param>
        /// <returns>Member's photo's url as string</returns>
        public String GetMemberMedia(int id = 1)
        {
            try
            {
                IPublishedContent member = memberShipHelper.GetById(id);
                IPublishedContent employee = this.memberShipHelper.GetEmployeeFromMember(member);
                string url = employee.GetPropertyValue("employeeImage").ToString();
                if (url == null || url.Equals(""))
                {
                    return null;
                }
                else
                {
                    return url;
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public IPublishedContent GetCurrentMembertest(){
            return memberShipHelper.GetCurrentMember();
        }
        public String GetCurrentMemberMedia()
        {
            int currentMemberId= this.memberShipHelper.GetCurrentMemberId();
            return memberShipHelper.GetMemberMedia(currentMemberId);
        }

        /// <summary>
        ///  Creates a new member if the email contains "@emakina."
        ///  if an error occurs returns false
        /// </summary>
        /// <param name="name"> Name of the new member</param>
        /// <param name="surname"> Surname of the new member</param>
        /// <param name="email">Email of the new member</param>
        /// <param name="Id"> Authentication type specific Id of the new member</param>
        /// <param name="picture"> Url of a picture </param>
        /// <returns>True if successful && False if not</returns>
        public bool CreateNewMember(String name, String surname, String email, String Id, String picture)
        {
            if (email.Contains("@emakina."))
            {
                IMember member = Services.MemberService.GetByEmail(email);
                IContent employee = ContentServiceController.Instance.ToIContent(this.memberShipHelper.GetEmployeeByMemberEmail(email));
                if (member == null)
                {
                    member = Services.MemberService.CreateMemberWithIdentity("google_" + Id, email, name, "Member");
                    member.SetValue("memberName", name);
                    member.SetValue("memberSurname", surname);
                    member.SetValue("employeeContentId", employee?.Id);
                    try
                    {
                        member.SetValue("memberImageUrl", employee.GetValue("employeeImageUrl").ToString());
                    }
                    catch (NullReferenceException ex)
                    {
                        member.SetValue("memberImageUrl", picture);
                        if (employee?.Name.Length > 0)
                        {
                            employee?.SetValue("employeeImageUrl", picture);
                            Services.ContentService.SaveAndPublishWithStatus(employee);
                        };
                    }
                    Services.MemberService.Save(member);
                }
                else if (employee != null)
                {
                    this.ValidateImage(member.Id);
                }
                else
                {
                    this.RefreshMemberImage(member.Id);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Sets the member's image with the given URL
        /// This is done only with the correct url if it's empty it won't be used nor changed
        /// </summary>
        /// <param name="id">id of the memebr</param>
        /// <param name="url">Url of the image</param>
        public void SetMemberImage(int id, String url)
        {
            IMember member = Services.MemberService.GetById(id);
            if (url != null && !url.Equals(""))
            {
                member?.SetValue("memberImageUrl", url);
                Services.MemberService.Save(member);
            }
        }
        /// <summary>
        /// Valides the photo of the member from it's emplyoee
        /// If employee image is the same with the member then true
        /// else false
        /// </summary>
        /// <param name="id">Id of the member</param>
        /// <returns>
        /// Bool
        /// True if same
        /// False if not
        /// </returns>
        public bool ValidateImage(int id)
        {
            string pb = this.memberShipHelper.GetMemberMedia(id);
            if (this.memberShipHelper.GetMemberMedia(id).Equals(
                this.memberShipHelper.GetEmployeeFromMember(memberShipHelper.GetById(id)).GetPropertyValue("employeeImageUrl")))
            {
                return true;
            }
            return this.RefreshMemberImage(id);

        }
        /// <summary>
        /// if the images are not the same with the employees
        /// Refreshes the member's image with the employee's one
        /// </summary>
        /// <param name="id">Id of the member</param>
        public bool RefreshMemberImage(int id)
        {
            try
            {
                string employeeImage =   this.memberShipHelper.GetEmployeeFromMember(memberShipHelper.GetById(id)).GetPropertyValue("employeeImageUrl").ToString();
                this.SetMemberImage(id, employeeImage);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogHelper.logHelper.Log("Could not refresh the image with the one that is on the employee " + "\n Error : " + ex.Message);
                return false;
            }

        }
    }
}
