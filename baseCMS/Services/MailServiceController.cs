using baseCMS.Helpers;
using baseCMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace baseCMS.Services
{
    public class MailServiceController : Controller
    {
        CustomLogHelper logger = CustomLogHelper.logHelper;

        private static MailServiceController Instance;

        public static MailServiceController MailService
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new MailServiceController();
                }
                return Instance;
            }
        }
        
        /// <summary>
        /// Send mail with job data
        /// </summary>
        /// <param name="scheduleJob"></param>
        [MemberAuthorize]
        public void SendMail(ScheduleJob scheduleJob)
        {
            logger.Log("Sending mail...");
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("emakinatalent@gmail.com", "tk2QQCZky8Ug");
                    smtp.EnableSsl = true;
                    //string sender = scheduleJob.SenderName.ToLower().Replace("ü", "ue").Replace('ş', 's').Replace('ı', 'i').Replace("ö", "oe").Replace("ç", "c").Replace('ğ', 'g');
                    string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Views\\Templates\\" + scheduleJob.TemplateFile + ".cshtml");
                    string decodedHtml = WebUtility.HtmlDecode(scheduleJob.Body);

                    MailMessage message = new MailMessage
                    {
                        From = new MailAddress("emakinatalent@gmail.com", scheduleJob.SenderName, Encoding.UTF8),
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        SubjectEncoding = Encoding.UTF8,
                        Body = System.IO.File.ReadAllText(path).Replace("{%body%}", decodedHtml),
                        Subject = scheduleJob.Subject
                    };
                    if (!string.IsNullOrEmpty(scheduleJob.To))
                        message.To.Add(scheduleJob.To);
                    if (!string.IsNullOrEmpty(scheduleJob.CC))
                        message.CC.Add(scheduleJob.CC);
                    if (!string.IsNullOrEmpty(scheduleJob.BCC))
                        message.Bcc.Add(scheduleJob.BCC);
                    if (!string.IsNullOrEmpty(scheduleJob.Attachments))
                    {
                        foreach (string attachment in scheduleJob.Attachments.Split(','))
                        {
                            // Attachment is physical file path, can be edited to be relative path later

                            // Create  the file attachment for this e-mail message.
                            System.Net.Mail.Attachment data = new System.Net.Mail.Attachment(attachment);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(attachment);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachment);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(attachment);
                            // Add the file attachment to this e-mail message.
                            message.Attachments.Add(data);
                        }
                    }
                    try
                    {
                        smtp.Send(message);
                        logger.Log("Mail has been sent.");
                    }
                    catch (Exception ex)
                    {
                        logger.Log(ex.ToString().Replace("\r\n", ""));
                    }

                }
            }
            catch(Exception ex)
            {
                logger.Log(ex.ToString().Replace("\r\n", ""));
            }
        }
    }
}
