using Quartz;
using Quartz.Impl;
using System;
using baseCMS.Models;
using Umbraco.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using baseCMS.Helpers;

namespace baseCMS.Services
{
    public class ScheduleServiceController : SurfaceController
    {
        CustomLogHelper logger =  CustomLogHelper.logHelper;
        private static ScheduleServiceController instance;

        public static ScheduleServiceController ScheduleService
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScheduleServiceController();
                }
                return instance;
            }
        }

        /// <summary>
        /// Schedule a job with cron expression
        /// </summary>
        /// <param name="WorkUntil"></param>
        /// <param name="CronExpression"></param>
        /// <param name="JobId"></param>
        public void ScheduleJob(DateTime WorkUntil, string CronExpression,int JobId)
        {
            logger.Log("Scheduling job...");

            try
            {
                // Get scheduler factory
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                // Create job and pass data as job id
                IJobDetail job = JobBuilder.Create<JobClass>()
                    .WithIdentity(JobId.ToString())
                    .WithDescription(JobId.ToString())
                    .UsingJobData("JobId", JobId)
                    .Build();

                // Create trigger with Job id and cron expression and make it work workuntil
                ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(JobId.ToString(), "group1")
                .StartNow()
                .WithCronSchedule(CronExpression)
                .EndAt(WorkUntil)
                .Build();

                // Schedule job
                scheduler.ScheduleJob(job, trigger);
                logger.Log("Job has been scheduled.");
            }
            catch(Exception ex)
            {
                logger.Log(ex.ToString().Replace("\r\n", ""));
            }
        }
    }
    internal class JobClass: IJob
    {
        CustomLogHelper logger = CustomLogHelper.logHelper;
        private static readonly HttpClient client = new HttpClient();
        
        /// <summary>
        /// Execute every time cron worked
        /// </summary>
        /// <param name="context"></param>
        async void IJob.Execute(IJobExecutionContext context)
        {
            logger.Log("Executing job...");
            // Get job data
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            try
            {
                int JobId = dataMap.GetInt("JobId");
                // Get base url
                string url = System.Configuration.ConfigurationManager.AppSettings["baseUrl"];
                // Get scheduled job data
                var response = await client.GetAsync(url+"/umbraco/surface/schedulesurface/getscheduledjob?JobId="+JobId);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                ScheduleJob scheduleJob = JsonConvert.DeserializeObject<ScheduleJob>(content);
                // Send mail with returned data
                MailServiceController.MailService.SendMail(scheduleJob);
            }
            catch (Exception ex)
            {
                logger.Log(ex.ToString().Replace("\r\n", ""));
            }
        }
    }
}
