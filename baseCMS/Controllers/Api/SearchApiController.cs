using baseCMS.Services;
using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers.Api
{

    public class SearchApiController : UmbracoApiController
    {
        /// <summary>
        /// Search all contents with given term
        /// </summary>
        /// <param name="term"></param>
        /// <returns>json search result</returns>
        [HttpPost]
        public List<SearchResult> Search(string term)
        {
            Dictionary<string, string> searchConfig = ConfigurationApiController.Instance.GetConfiguration("ExcludeSearch");
            var excludedDocumentTypes = searchConfig["excludeSearch"];

            List<SearchResult> results = new List<SearchResult>();
            foreach (var result in Umbraco.TypedSearch(term,true,"InternalSearcher"))
            {
                if (excludedDocumentTypes.ToLower().Contains(result.DocumentTypeAlias.ToLower()))
                    continue;
                results.Add(new SearchResult
                {
                     id = result.Id,
                     value=result.Name,
                    Url = result.Url
                });
            }
            return results;
        }
    }

    public class SearchResult
    {
        public int id { get; set; }
        public string value { get; set; }
        public string Url { get; set; }
    }
}
