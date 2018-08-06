using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace baseCMS.Models
{
    public class ScheduleJob
    {
        public int ID { get; set; }
        public string JobName { get; set; }
        public DateTime WorkUntil { get; set; }
        public string CronExpression { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplateFile { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Attachments { get; set; }
        public string To { get; set; }
    }
}
