using baseCMS.Helpers;
using baseCMS.Models;
using baseCMS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers.Surface
{
    [Umbraco.Web.Mvc.MemberAuthorize]
    public class AttachmentSurfaceController : SurfaceController
    {
        MemberHelper memberHelper= new MemberHelper();
        //Global variables (These are declared to global to escape too much argument in functions)
        //Current member name
       
        //now
        string now = string.Format("{0:yyyy-MM-dd H:mm:ss}", DateTime.Now);
        //Variable that is going to store files that could not be uploaded
        Dictionary<string, string> failedFiles = new Dictionary<string, string>();
        //Variable that is going to store individual file
        HttpPostedFileBase file;
        //Variable that is going to store individual file name
        string fileName;
        //Variable that is going to store parent content id of the attachments
        int parentId;
        //Variable that is going to store upload folder
        string folder;

        string[] allowedFileTypes = new string[] {
            "text/plain",
            "application/pdf",
            "application/vnd.ms-excel",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/msword",
            "image/jpeg",
            "image/png"
        };


        /// <summary>
        ///  Uploading files and creating contents
        /// </summary>
        /// <param name="filesToUpload"> Represents uploaded files</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> filesToUpload)
        {
            CustomLogHelper logger = CustomLogHelper.logHelper;

            //Taking request parameters from request
            parentId = Int32.Parse(Request["parentId"]);
            bool willExistedFileOverwrited = Convert.ToBoolean(Request["willExistedFileOverwrited"]);
            bool isThereExistedFile = Convert.ToBoolean(Request["isThereExistedFile"]);


            //Iterating through files
            for (int i = 0; i < Request.Files.Count; i++)
            {
                // Individual file
                file = Request.Files[i];
                fileName = Path.GetFileName(file.FileName);
                bool isFileValid = true;

                //File Size Validation
                if (file.ContentLength > 6000000)
                {
                    isFileValid = false;
                    failedFiles.Add(fileName, "Individual file size must be under 6MB !! ");
                }

                //File Type Validation
                if (!allowedFileTypes.Contains(file.ContentType))
                {
                    isFileValid = false;
                    failedFiles.Add(fileName, "File type is not supported, supported types: pdf, text, excel, csv, jpeg, png ");
                }


                //If folder does not exist for parentId then create it
                folder = Server.MapPath("~/App_Data/uploads/" + parentId);
                bool isFolderExists = Directory.Exists(folder);
                if (!isFolderExists)
                    Directory.CreateDirectory(folder);

                string path = Path.Combine(folder, fileName);

                if (isFileValid)
                {
                    try
                    {
                        //If there is existed attachment and the attachment exist in the path
                        if (isThereExistedFile && System.IO.File.Exists(path))
                        {
                            //attachment is going to be overwrited
                            if (willExistedFileOverwrited)
                            {
                                //Overwrite attachment
                                OverwriteAttachment();
                            }
                            else //Keeping existed attachment and creating new attachment
                            {
                                //Creating new attachment while keeping old attachment
                                KeepExistedAttachmentAddNew();
                            }
                        }
                        else //If attachment doesn't existed before
                        {
                            //Cretaing new attachment
                            AddNewAttachment();
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Log(ex.ToString().Replace("\r\n", ""));
                        string json = JsonConvert.SerializeObject(failedFiles, Formatting.Indented);
                        return Json(new { status = "Error", message = "file(s) could not be uploaded", files = failedFiles });

                    }
                }

            }

            if (failedFiles.Count > 0)
            {
                string json = JsonConvert.SerializeObject(failedFiles, Formatting.Indented);
                return Json(new { status = "Error", message = "file(s) could not be uploaded", files = failedFiles });
            }

            return Json(new { status = "Success", message = "file(s) successfully uploaded" });

        }

        /// <summary>
        /// overwriting the attachment
        /// </summary>
        private void OverwriteAttachment()
        {
            string path = Path.Combine(folder, fileName);
            string currentMemberFullname = memberHelper.GetCurrentMember().GetPropertyValue("memberName").ToString() + " " + memberHelper.GetCurrentMember().GetPropertyValue("memberSurname").ToString();
            //Taking parent content and parent content type alias
            IContent parentContent = Services.ContentService.GetById(parentId);

            //Finding old file's content by attachmentUrl
            string url = "/App_Data/uploads/" + parentId + "/" + fileName;
            IContent attachmentContent = parentContent.Children().Where(x => x.ContentType.Alias == "attachment" && x.GetValue("attachmentUrl").ToString().Equals(url)).First();

            //Setting new version for overwrited file
            string version = attachmentContent.GetValue("versionNumber").ToString();
            int oldCount = Int32.Parse(version.Remove(0, 1));
            int newCount = ++oldCount;
            version = "V" + newCount.ToString();
            attachmentContent.SetValue("versionNumber", version);
            attachmentContent.SetValue("updatedAt", now);
            attachmentContent.SetValue("updatedBy", currentMemberFullname);

            //Deleting old file and saving new file
            if (System.IO.File.Exists(path) && Services.ContentService.SaveAndPublishWithStatus(attachmentContent))
            {
                System.IO.File.Delete(path);
                file.SaveAs(path);
            }
            else
            {
                failedFiles.Add(fileName, "file could not be uploaded");
            }

        }

        /// <summary>
        /// Creating new attachment while keeping old attachment
        /// </summary>
        private void KeepExistedAttachmentAddNew()
        {

            string folder = Server.MapPath("~/App_Data/uploads/" + parentId);
            string currentMemberFullname = memberHelper.GetCurrentMember().GetPropertyValue("memberName").ToString() + " " + memberHelper.GetCurrentMember().GetPropertyValue("memberSurname").ToString();
            //Creating content for uploaded file
            var attachmentContent = Services.ContentService.CreateContent(fileName, parentId, "attachment");
            attachmentContent.SetValue("attachmentName", fileName);

            Services.ContentService.SaveAndPublishWithStatus(attachmentContent);

            //Taking last 3 char from new fileName and append it to begining of the fileName
            //This is for making file and contentName consistent with each other.
            string newFileName = attachmentContent.Name;
            string substring = newFileName.Substring(newFileName.Length - 3);
            string temp = newFileName.Remove(newFileName.Length - 3);
            newFileName = substring + temp;

            //new path generated according to new created attachmentContent
            var path = Path.Combine(folder, newFileName);
            string url = "/App_Data/uploads/" + parentId + "/" + newFileName;

            //Updating attachmentContent
            attachmentContent.SetValue("attachmentName", newFileName);
            attachmentContent.SetValue("attachmentUrl", url);
            attachmentContent.SetValue("createdAt", now);
            attachmentContent.SetValue("createdBy", currentMemberFullname);
            attachmentContent.SetValue("versionNumber", "V0");

            //If content is created then save the file
            if (Services.ContentService.SaveAndPublishWithStatus(attachmentContent))
            {
                file.SaveAs(path);
            }
            else
            {
                failedFiles.Add(fileName, "file could not be uploaded");
            }
        }

        /// <summary>
        /// Adding new attachment
        /// </summary>
        private void AddNewAttachment()
        {
            string currentMemberFullname = memberHelper.GetCurrentMember().GetPropertyValue("memberName").ToString() + " " + memberHelper.GetCurrentMember().GetPropertyValue("memberSurname").ToString();
            string path = Path.Combine(folder, fileName);
            string url = "/App_Data/uploads/" + parentId + "/" + fileName;
            var attachmentContent = Services.ContentService.CreateContent(fileName, parentId, "attachment");

            //Creating content for uploaded file
            attachmentContent.SetValue("attachmentName", fileName);
            attachmentContent.SetValue("attachmentUrl", url);
            attachmentContent.SetValue("createdAt", now);
            attachmentContent.SetValue("createdBy", currentMemberFullname);
            attachmentContent.SetValue("versionNumber", "V0");



            //If content is created then save the file
            if (Services.ContentService.SaveAndPublishWithStatus(attachmentContent))
            {
                file.SaveAs(path);
            }
            else
            {
                failedFiles.Add(fileName, "file could not be uploaded");
            }
        }


        /// <summary>
        /// Deletes the content by Id
        /// </summary>
        /// <param name="contentId">Id of the content</param>
        [HttpDelete]
        public ActionResult Delete(int contentId)
        {
            CustomLogHelper logger = CustomLogHelper.logHelper;

            try
            {
                IContent parentContent = Services.ContentService.GetById(contentId);
                parentContent.Name = Guid.NewGuid().ToString();

                string fileFullName = parentContent.GetValue("attachmentUrl").ToString();

                var path = Server.MapPath("~" + fileFullName);


                if (System.IO.File.Exists(path) && Services.ContentService.UnPublish(parentContent))
                {
                    System.IO.File.Delete(path);
                    return Json(new { status = "Success", message = "Attachment succesfuly deleted" });
                }
                else
                {
                    return Json(new { status = "Error", message = "Attachment could not be deleted" });
                }

            }
            catch (Exception ex)
            {
                logger.Log(ex.ToString().Replace("\r\n", ""));
                return Json(new { status = "Error", message = "Attachment(s) could not be deleted" });

            }

        }

        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="filename"></param>
        public void Download(string parentId, string filename)
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename="+filename);
            Response.WriteFile(Server.MapPath(@"~/App_Data/uploads/"+ parentId + "/"+filename));
            Response.End();
        }
    }
}
