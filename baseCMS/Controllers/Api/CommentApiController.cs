

using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Security;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers
{
    public class CommentApiController : UmbracoApiController
    {
        /// <summary>
        /// Lists all the comments for the given parent
        /// </summary>
        /// <param name="parentId">Parent's Id</param>
        /// <returns>List of commentModels</returns>
        [HttpGet]

        public List<Comment> GetCommentsById(int parentId, int page = 0, int max = 10)
        {
            List<IPublishedContent> comments = ContentServiceController.Instance.GetContentListByTypeAndParentId("comment", parentId, page, max);
            List<Comment> commentModels = new List<Comment>();
            Comment dummyComment = new Comment();
            String connectedImage;
            String creator;
            MemberHelper memberHelper=new MemberHelper(UmbracoContext.Current);
            foreach (IPublishedContent comment in comments)
            {
                IPublishedContent member = memberHelper.GetById(Convert.ToInt32(comment.GetPropertyValue("commentWrittenBy").ToString()));// memberService.FindMemberById(Convert.ToInt32(comment.GetPropertyValue("commentWrittenBy").ToString()));// memberService.FindMemberById(Convert.ToInt32(comment.GetPropertyValue("commentWrittenBy").ToString()));
                bool employeeExist = (member == null)
                    ? false
                    : true;
                if (employeeExist)
                {
                    connectedImage = memberHelper.GetMemberMedia(member.Id);
                    creator = member.GetPropertyValue("memberName").ToString() + ' ' + member.GetPropertyValue("memberSurname").ToString();
                }
                else
                {
                   connectedImage = memberHelper.GetMemberMedia();
                   creator="Unknown";
                }

                dummyComment = new Comment();
                dummyComment.Description = comment.GetPropertyValue("commentDescription").ToString();
                dummyComment.Id = comment.Id;
                dummyComment.CreationTime = comment.CreateDate;
                dummyComment.ParentComment = (comment.Ancestor().DocumentTypeAlias.Equals("comment")) ? comment.Ancestor().Id : 0;
                dummyComment.Creator = creator;
                dummyComment.EmployeePhoto = connectedImage;
                dummyComment.LastEdited = comment.UpdateDate;
                commentModels.Add(dummyComment);
            }
            return commentModels;
        }
    }
}
