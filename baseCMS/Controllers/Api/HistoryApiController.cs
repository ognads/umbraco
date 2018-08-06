using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Extensions;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers.Api
{
    public class HistoryApiController : UmbracoApiController
    {
        /// <summary>
        /// Get audit logs for content
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns>List of audit logs</returns>
        [HttpGet]
        public List<Change> GetHistory(int contentId,int max=5,int page=1,string prop="All")
        {
            List<Change> changes = new List<Change>();
            var service = Services.ContentService;
            IEnumerable<Guid> contentVersionsIds;
            try
            {
                //get versions with paging
                if (page == 1)
                {
                    contentVersionsIds = Services.ContentService.GetVersionIds(contentId, max * page);
                }
                else
                {
                    contentVersionsIds = Services.ContentService.GetVersionIds(contentId, max * page).Skip(max*(page-1));
                }
                
                IContent temp = null;

                int count = 0;

                foreach (var contentIds in contentVersionsIds)
                {
                    IContent content = Services.ContentService.GetByVersion(contentIds);

                    //get first content for compare
                    if (count == 0) { temp = content; count++;continue; }
                    
                    string[] properties;

                    //if get all properties
                    if (prop == "All")
                    {
                        properties = temp.Properties.Select(x=>x.Alias).ToArray();
                    }
                    //else desired properties
                    else
                    {
                        properties = prop.Split(',');
                    }
                    foreach (var property in properties)
                    {
                        //get two property's value's for comparing
                        string previousValue = content.GetValue<string>(property) ?? "";
                        string latestValue = temp.GetValue<string>(property) ?? "";
                        
                        if (property == "position")
                        {
                            previousValue = Udi.Parse(previousValue).ToPublishedContent().GetPropertyValue("positionName").ToString();
                            latestValue = Udi.Parse(latestValue).ToPublishedContent().GetPropertyValue("positionName").ToString();
                        }
                        //compare values and if they aren't equal than add this to changes list
                        if (previousValue != latestValue)
                            changes.Add(new Change()
                            {
                                Type = "Update",
                                FieldName = property,
                                OldValue = previousValue,
                                NewValue = latestValue,
                                UpdateDate = temp.UpdateDate
                            });

                    }
                    //for next iteration compare this content with next
                    temp = content;
                    count++;
                }
                
                
                return changes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class Change
        {
            public string Type { get; set; }
            public string FieldName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public DateTime UpdateDate { get; set; }
        }
    }
}
