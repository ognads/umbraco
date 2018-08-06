using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using baseCMS.Services;
using baseCMS.Helpers;

namespace baseCMS.EventHandlers
{
    public class RegisterEvents:ApplicationEventHandler
    {
        CustomLogHelper logger = CustomLogHelper.logHelper;
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        //     try
        //     {
        //         // Get all jobs and schedule them while application starting
        //         var jobs = applicationContext.Services.ContentService.GetContentOfContentType(5295);
        //         foreach (var job in jobs)
        //         {
        //             logger.Log("Job will be scheduled: " + job.Id);
        //             ScheduleServiceController.ScheduleService.ScheduleJob(Convert.ToDateTime(job.GetValue("workUntil")), job.GetValue("cronExpression").ToString(), job.Id);
        //         }
        //     }
        //     catch(Exception ex)
        //     {
        //         logger.Log(ex.ToString().Replace("\r\n", ""));
        //     }
    }
}
}
