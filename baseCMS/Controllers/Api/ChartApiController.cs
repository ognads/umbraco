using baseCMS.Controllers.Surface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers.Api
    {
    /// <summary>
    /// For futher reference you will need to add a "GetNAMEHEREChart" function and call that.
    /// </summary>
    public class ChartApiController : UmbracoApiController
    {
        /// <summary>
        /// Generic get chart.
        /// Based on the Id it searches for the alias and then forwards the request to the applicable controllers
        /// </summary>
        /// <param name="id">Id of the content that we're in</param>
        /// <returns>Action result that comes from the different controllers</returns>
        public List<String> GetChart(int id)
        {
            String contentAlias = ((IPublishedContent)Umbraco.Content(id)).DocumentTypeAlias;
            MethodInfo mi = this.GetType().GetMethod("Get" + contentAlias + "Chart");
            List<String> partialViewUrls = (List<String>)mi.Invoke(this, null);
            return partialViewUrls;
        }
        /// <summary>
        /// Gets the itModule 's charts 
        /// </summary>
        /// <returns>List of chart url's</returns>
        public List<String> GetiTModuleChart()
        {
            InventorySurfaceController temp = new InventorySurfaceController();
            return temp.GetCharts();
        }
        /// <summary>
        /// Get the recruitment modules's charts from the employee surface controller
        /// </summary>
        /// <returns>List of chartURLS</returns>
        public List<String> GetmodulesChart()
        {
            CandidateSurfaceController temp = new CandidateSurfaceController();
            return temp.GetCharts();
        }
        /// <summary>
        /// Get the operationModule's charts from the employee surface controller
        /// </summary>
        /// <returns>List of chart url's</returns>
        public List<String> GetoperationsModuleChart()
        {
            EmployeeSurfaceController temp = new EmployeeSurfaceController();
            return temp.GetCharts();
        }
    }
}
