using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using baseCMS.Models;
using baseCMS.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace baseCMS.Controllers.Api
{
    [Umbraco.Web.WebApi.MemberAuthorize]
    public class NewsApiController : UmbracoApiController
    {
        // GET: NewsApi
        /// <summary>
        ///     Getting news and grouping them by updatedAt
        /// </summary>
        /// <param name="page"></param>
        /// <param name="max"></param>
        /// <param name="parentId"></param>
        /// <returns>JObjects News (Grouped by update date)</returns>
        [HttpGet]
        public JObject GetNews(string searchTerm = "", int page = 0, int max = 10, int parentId = 0)
        {
            //List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("news", parentId, page, max).OrderByDescending(x => x.UpdateDate).ToList();

            IPublishedContent parentContent = Umbraco.Content(parentId);
            int skipAmount = page * max;

            List<IPublishedContent> contents = new List<IPublishedContent>();
            
            if (searchTerm != "" && searchTerm != null) {
                contents = parentContent.Descendants().Where(x => x.DocumentTypeAlias == "news" && x.Name.ToLower().Contains(searchTerm.ToLower())).OrderByDescending(x => x.UpdateDate).ToList();
            }else{
                //contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("news", parentId, page, max).OrderByDescending(x => x.UpdateDate).ToList();
                contents = ((IPublishedContent)Umbraco.Content(parentId))
                    .Descendants()
                    .Where(x => x.DocumentTypeAlias == "news")
                    .OrderByDescending(x => x.UpdateDate)
                    .Skip(skipAmount)
                    .Take(max)
                    .ToList();
            }


            //Mapping contents to models
            List<News> newsList = MapNewsContentToModel(contents);

            //Grouping models by update date
            Dictionary<string, List<News>> newsByDate = GroupNewsByUpdateDate(newsList);

            //First converting dictionary to string then converting to JObject
            string temp = JsonConvert.SerializeObject(newsByDate, Formatting.Indented);
            JObject newsByDateJson = JObject.Parse(temp);   
            return newsByDateJson;

        }

        /// <summary>
        ///     Mapping News contents to models
        /// </summary>
        /// <param name="contents"></param>
        /// <returns> List<News> news models</returns>
        private List<News> MapNewsContentToModel(List<IPublishedContent> contents)
        {
            List<News> newsList = new List<News>();

            //Mapping contents to models
            foreach (var content in contents)
            {
                News news = new News
                {
                    ID = content.Id,
                    Title = content.GetPropertyValue("title").ToString(),
                    Description = content.GetPropertyValue("description").ToString(),
                    CreatedAt = content.CreateDate,
                    UpdatedAt = content.UpdateDate,
                    CreatedBy = content.CreatorName.ToString(),

                };

                newsList.Add(news);
            }

            return newsList;

        }

        /// <summary>
        ///     Grouping models by update date
        /// </summary>
        /// <param name="newsList"></param>
        /// <returns> Dictionary<string, List<News>> newsByDate </returns>
        private Dictionary<string, List<News>> GroupNewsByUpdateDate(List<News> newsList)
        {
            var newsByDate = new Dictionary<string, List<News>>();

            //Grouping models by update date
            foreach (var news in newsList)
            {
                string updatedAt = news.UpdatedAt.ToString("dd MMM. yyyy");
                List<News> initialNewsList = new List<News>();
                List<News> existedList = new List<News>();

                if (!newsByDate.TryGetValue(updatedAt, out existedList))
                {
                    initialNewsList.Add(news);
                    newsByDate.Add(updatedAt, initialNewsList);
                }
                else
                {
                    existedList.Add(news);
                    newsByDate[updatedAt] = existedList;
                }

            }

            return newsByDate;

        }

        
    }
}
