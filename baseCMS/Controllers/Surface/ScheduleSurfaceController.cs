using baseCMS.Helpers;
using baseCMS.Models;
using baseCMS.Services;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    public class ScheduleSurfaceController : SurfaceController
    {
        CustomLogHelper logger = CustomLogHelper.logHelper;
        /// <summary>
        /// Schedule a job for sending mail
        /// </summary>
        /// <param name="scheduleJob"></param>
        public void ScheduleJob(ScheduleJob scheduleJob)
        {
            if (ModelState.IsValid)
            {
                logger.Log("Creating new scheduled job...");
                try
                {
                    // Create job content
                    var service = Services.ContentService;
                    IContent job = service.CreateContent(scheduleJob.JobName, -1, "scheduleJob");
                    job.SetValue("jobName", scheduleJob.JobName);
                    job.SetValue("workUntil", scheduleJob.WorkUntil);
                    job.SetValue("cronExpression", scheduleJob.CronExpression);
                    job.SetValue("senderName", scheduleJob.SenderName);
                    job.SetValue("subject", scheduleJob.Subject);
                    job.SetValue("body", scheduleJob.Body);
                    job.SetValue("templateFile", scheduleJob.TemplateFile);
                    job.SetValue("cC", scheduleJob.CC);
                    job.SetValue("bCC", scheduleJob.BCC);
                    job.SetValue("attachments", scheduleJob.Attachments);
                    job.SetValue("to", scheduleJob.To);
                    if (service.SaveAndPublishWithStatus(job).Success)
                    {
                        // If job content created successfully schedule job
                        logger.Log("Job created successfully. Scheduling it now...");
                        ScheduleServiceController.ScheduleService.ScheduleJob(scheduleJob.WorkUntil, scheduleJob.CronExpression, job.Id);
                    }
                    else
                    {
                        logger.Log("Job couldn't created.");
                    }
                }
                catch(Exception ex)
                {
                    logger.Log(ex.ToString().Replace("\r\n", ""));
                }
            }
        }
        
        /// <summary>
        /// Get scheduled job data
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns>Scheduled job data in JSON string</returns>
        public string GetScheduledJob(int JobId)
        {
            try
            {
                logger.Log("Getting scheduled job with ID: " + JobId);
                IPublishedContent job = Umbraco.Content(JobId);
                ScheduleJob scheduleJob = new ScheduleJob
                {
                    Attachments = job.GetProperty("attachments").Value.ToString(),
                    BCC = job.GetProperty("bCC").Value.ToString(),
                    CC = job.GetProperty("cC").Value.ToString(),
                    Body = job.GetProperty("body").Value.ToString(),
                    CronExpression = job.GetProperty("cronExpression").Value.ToString(),
                    SenderName = job.GetProperty("sendername").Value.ToString(),
                    Subject = job.GetProperty("subject").Value.ToString(),
                    TemplateFile = job.GetProperty("templateFile").Value.ToString(),
                    To = job.GetProperty("to").Value.ToString(),
                    WorkUntil = Convert.ToDateTime(job.GetProperty("workUntil").Value)
                };
                return JsonConvert.SerializeObject(scheduleJob);
            }catch(Exception ex)
            {
                logger.Log(ex.ToString().Replace("\r\n", ""));
                return null;
            }
            
        }
    }
}
