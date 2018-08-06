using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using baseCMS.Helpers;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace baseCMS.Services {
    // A service class for content management
    public class ContentServiceController : SurfaceController {
        private static ContentServiceController instance;

        public static ContentServiceController Instance {
            get {
                if (instance == null) {
                    instance = new ContentServiceController ();
                }
                return instance;
            }
        }
        /// <summary>
        /// Parses the IPublishedContent to a IContent
        /// </summary>
        /// <param name="content">Content as IPublishedCOntent</param>
        /// <returns>IContent of the given IPublilshedContent</returns>
        public IContent ToIContent (IPublishedContent content) {
            try {
                return Services.ContentService.GetById(content.Id);
            } catch (Exception ex) {
                CustomLogHelper.logHelper.Log("Cannot convert to IContent the given \n Error : "+ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Parses the IContent to a IPublishedContent
        /// </summary>
        /// <param name="content">Content as IContent</param>
        /// <returns>IPublishedContent of the given IContent</returns>
        public IPublishedContent ToIPublishedContent (IContent content) {
            try {
                return Umbraco.Content (content.Id);
            } catch (Exception ex) {
                CustomLogHelper.logHelper.Log ("Cannot convert to IPublishedContent the given " + content.Id + "Error message :" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Gets the given media
        /// </summary>
        /// <param name="id"> id of the content</param>
        /// <returns>returns the given media && null if it does not exist</returns>
        public String GetMediaUrl (int id) {
            IPublishedContent media = Umbraco.TypedContent (Services.MediaService.GetById (id));
            return   media?.GetPropertyValue<IPublishedContent> ("umbracoFile")?.UrlAbsolute();
            /*return (media?.GetPropertyValue ("umbracoFile") != null) ?
                media.GetPropertyValue<IPublishedContent> ("umbracoFile").UrlAbsolute () :
                "~/media/1104/pp.jpg";*/
        }

        /// <summary>
        /// Gets the content from the given GUID
        /// </summary>
        /// <param name="guid">Guid of the content as String</param>
        /// <param name="objectType">Object type, This should be a umbraco model
        /// Such as UmbracoObjectTypes.Media</param>
        /// <returns>IPublishedContent found from Guid && null if empty</returns>
        public IPublishedContent GetContentFromGuid (String guid, UmbracoObjectTypes objectType) {
            var attempt = Services.EntityService.GetIdForKey (new Guid (guid), objectType);
            if (attempt.Success) {
                var media = Umbraco.TypedMedia (attempt.Result);
                return media;
            }
            return null;
        }
        //A service for getting umbraco content with pagination
        /// <summary>
        ///  Get contents
        /// </summary>
        /// <param name="type">Represents content type</param>
        /// <param name="parentId">Represents parent id of content</param>
        /// <param name="max">Represents how many contens will be taken</param>
        /// <param name="page">Represents page for pagination</param>
        /// <returns>Partial view</returns>
        //[MemberAuthorize]
        public List<IPublishedContent> GetContentListByTypeAndParentId (string type, int parentId, int page = 0, int max = 10) {
            int skipAmount = page * max;
            if (page == -1) {
                if (parentId == -1) {
                    return Umbraco.TypedContentAtRoot ().First ()
                        .Descendants ()
                        .Where (x => x.DocumentTypeAlias == type)
                        .ToList ();
                }
                return ((IPublishedContent) Umbraco.Content (parentId))
                    .Descendants ()
                    .Where (x => x.DocumentTypeAlias == type)
                    .ToList ();
            } else {
                if (parentId == -1) {
                    return Umbraco.TypedContentAtRoot ().First ()
                        .Descendants ()
                        .Where (x => x.DocumentTypeAlias == type)
                        .Skip (skipAmount)
                        .Take (max)
                        .ToList ();
                }
                return ((IPublishedContent) Umbraco.Content (parentId))
                    .Descendants ()
                    .Where (x => x.DocumentTypeAlias == type)
                    .Skip (skipAmount)
                    .Take (max)
                    .ToList ();
            }
        }

    }
}
