using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using baseCMS.Helpers;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace baseCMS.Services {
    public class ConfigurationApiController : UmbracoApiController {
        private const int CONFIGURATION_ID = 5647;
        private static ConfigurationApiController instance;

        public static ConfigurationApiController Instance {
            get {
                if (instance == null) {
                    instance = new ConfigurationApiController ();
                }
                return instance;
            }
        }
        /// <summary>
        /// Gets all of the configuration's properties as Directory
        /// </summary>
        /// <returns>Directory as alias and it's value as string</returns>
        [HttpGet]
        public Dictionary<String, String> GetConfiguration () {
            IContent configContent = this.GetConfigutationContent (CONFIGURATION_ID);
            PropertyCollection configProperties = configContent.Properties;
            Dictionary<String, String> keyValue = new Dictionary<string, string> ();
            try {

                foreach (Property propType in configProperties) {
                    keyValue.Add (propType.Alias, configContent.GetValue (propType.Alias)?.ToString ());
                }
                return keyValue;
            } catch (Exception ex) {
                CustomLogHelper.logHelper.Log ("Couldn't find the configuration properties!" +
                    " \n With error message: \n " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Gets specific tab of the configuration
        /// </summary>
        /// <param name="configGroupName">Tab's name</param>
        /// <returns> Directory as alias and it's value as string</returns>
        [HttpGet]
        public Dictionary<String, String> GetConfiguration (string configGroupName) {
            IContent configContent = this.GetConfigutationContent (CONFIGURATION_ID);
            PropertyGroup configGroup = configContent.PropertyGroups?.Where (z => z.Name.Equals (configGroupName)).First ();
            Dictionary<String, String> keyValue = new Dictionary<string, string> ();
            try {
                foreach (PropertyType propType in configGroup.PropertyTypes) {
                    keyValue.Add (propType.Alias, configContent.GetValue (propType.Alias).ToString ());
                }
                return keyValue;
            } catch (Exception ex) {
                CustomLogHelper.logHelper.Log ("Couldn't find the configuration properties for " + configGroupName +
                    " \n With error message: \n " + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// Get configuration content by the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Configuration as IPublishedContent</returns>
        private IContent GetConfigutationContent (int id) {
            return ContentServiceController.Instance.ToIContent (Umbraco.Content (id));
        }
    }
}
