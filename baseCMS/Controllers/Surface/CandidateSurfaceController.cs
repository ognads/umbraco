using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace baseCMS.Controllers.Surface
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
    public class CandidateSurfaceController : SurfaceController
    {
        const String CONTROLLER_URL = "/umbraco/surface/CandidateSurface/";
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";
        /// <summary>
        ///  Get employee add form to show in modal
        /// </summary>
        /// <returns>Partial view</returns>
        ///
        [HttpGet]
        public ActionResult GetAddForm()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "Candidate/Add.cshtml");
        }
        /// <summary>
        ///  Get employee edit form with data on it to show in modal
        /// </summary>
        /// <param name="contentId"> employee id</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public ActionResult GetEditForm(int contentId)
        {
            //Get content (employee) with id
            IPublishedContent content = (IPublishedContent)Umbraco.Content(contentId);
            IPublishedContent address = content;
            try
            {

                address = content.Descendants().Where(a => a.DocumentTypeAlias == "address").First();
            }
            catch (Exception ex)
            {

            }
            //string date = Convert.ToDateTime(content.GetPropertyValue("employeeBirthday", "01-01-2000")).ToString("MM/dd/yyyy");
            //set emplyee to employee model
            Candidate candidate = new Candidate
            {
                ID = content.Id,
                CandidateName = content.GetPropertyValue("candidateName", "").ToString(),
                CandidateSurname = content.GetPropertyValue("candidateSurname", "").ToString(),
                CandidateGender = content.GetPropertyValue("candidateGender", "").ToString(),
                CandidateBirthday = Convert.ToDateTime(content.GetPropertyValue("candidateBirthdate", "01-01-2000")),
                CandidateEmail = content.GetPropertyValue("candidateEmail", "").ToString(),
                PhoneNumber = content.GetPropertyValue("phoneNo").ToString(),
                CandidateEducation = content.GetPropertyValue("candidateEducation").ToString(),
                CandidateSkills = content.GetPropertyValue("candidateSkills").ToString(),
                CandidateWorkExperience = content.GetPropertyValue("candidateWorkExperience").ToString(),
                HealthKnownIssues = content.GetPropertyValue("knownIssues").ToString(),
                HealthLegallyDisabled = Convert.ToBoolean(content.GetPropertyValue("legallyDisabled")),
                AddressLine1 = address.GetPropertyValue("addressDescriptionLine1", "").ToString(),
                AddressLine2 = address.GetPropertyValue("addressDescriptionLine2", "").ToString(),
                Country = address.GetPropertyValue("countryName", "").ToString(),
                Latitude = address.GetPropertyValue("latitude", "").ToString(),
                Longitude = address.GetPropertyValue("longtitude", "").ToString(),
                PostalCode = address.GetPropertyValue("postalCode", "").ToString(),
                State = address.GetPropertyValue("stateName", "").ToString()
            };
            //Candidate candidate = new Candidate
            //{
            //    ID = content.Id,
            //    CandidateName = content.GetPropertyValue("candidateName", "").ToString(),
            //    CandidateSurname = content.GetPropertyValue("candidateSurname", "").ToString(),
            //    CandidateGender = content.GetPropertyValue("candidateGender", "").ToString(),
            //    CandidateBirthday = Convert.ToDateTime(content.GetPropertyValue("candidateBirthdate", "01-01-2000")),
            //    CandidateEmail = content.GetPropertyValue("candidateEmail", "").ToString(),
            //    PhoneNumber = content.GetPropertyValue("phoneNo").ToString(),
            //    CandidateEducation = content.GetPropertyValue("candidateEducation").ToString(),
            //    CandidateSkills = content.GetPropertyValue("candidateSkills").ToString(),
            //    CandidateWorkExperience = content.GetPropertyValue("candidateWorkExperience").ToString(),
            //    HealthKnownIssues = content.GetPropertyValue("knownIssues").ToString(),
            //    HealthLegallyDisabled = Convert.ToBoolean(content.GetPropertyValue("legallyDisabled")),
            //    AddressLine1 = address.GetPropertyValue("addressDescriptionLine1", "").ToString(),
            //    AddressLine2 = address.GetPropertyValue("addressDescriptionLine2", "").ToString(),
            //    Country = address.GetPropertyValue("countryName", "").ToString(),
            //    Latitude = address.GetPropertyValue("latitude", "").ToString(),
            //    Longitude = address.GetPropertyValue("longtitude", "").ToString(),
            //    PostalCode = address.GetPropertyValue("postalCode", "").ToString(),
            //    State = address.GetPropertyValue("stateName", "").ToString()
            //};
            //return to partial view with employee
            return PartialView(PARTIAL_VIEW_FOLDER + "Candidate/Edit.cshtml", candidate);
        }
        /// <summary>
        ///  Get candidate detail edit form with data on it 
        /// </summary>
        /// <param name="contentId"> candidate id</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public ActionResult GetEditDetailForm(int contentId)
        {
            //Get content (employee) with id
            IPublishedContent content = (IPublishedContent)Umbraco.Content(contentId);
            IPublishedContent address = content;
            try
            {

                address = content.Descendants().Where(a => a.DocumentTypeAlias == "address").First();
            }
            catch (Exception ex)
            {

            }

            //set emplyee to employee model
            Candidate candidate = new Candidate
            {
                ID = content.Id,
                CandidateName = content.GetPropertyValue("candidateName", "").ToString(),
                CandidateMiddleName = content.GetPropertyValue("candidateMiddleName", "").ToString(),
                CandidateSurname = content.GetPropertyValue("candidateSurname", "").ToString(),
                CandidateGender = content.GetPropertyValue("candidateGender", "").ToString(),
                CandidateBirthday = Convert.ToDateTime(content.GetPropertyValue("candidateBirthdate", "01-01-2000")),
                CandidateEmail = content.GetPropertyValue("candidateEmail", "").ToString(),
                PhoneNumber = content.GetPropertyValue("phoneNo").ToString(),
                CandidateEducation = content.GetPropertyValue("candidateEducation").ToString(),
                CandidateSkills = content.GetPropertyValue("candidateSkills").ToString(),
                CandidateWorkExperience = content.GetPropertyValue("candidateWorkExperience").ToString(),
                HealthKnownIssues = content.GetPropertyValue("knownIssues").ToString(),
                HealthLegallyDisabled = Convert.ToBoolean(content.GetPropertyValue("legallyDisabled")),
                AddressLine1 = address.GetPropertyValue("addressDescriptionLine1", "").ToString(),
                AddressLine2 = address.GetPropertyValue("addressDescriptionLine2", "").ToString(),
                Country = address.GetPropertyValue("countryName", "").ToString(),
                Latitude = address.GetPropertyValue("latitude", "").ToString(),
                Longitude = address.GetPropertyValue("longtitude", "").ToString(),
                PostalCode = address.GetPropertyValue("postalCode", "").ToString(),
                State = address.GetPropertyValue("stateName", "").ToString(),
                AlternativeEmail = address.GetPropertyValue("alternativeEmail", "").ToString()
            };
            
            return PartialView(PARTIAL_VIEW_FOLDER + "Candidate/EditDetail.cshtml", candidate);
        }
        /// <summary>
        ///  Create new employee in specific content
        /// </summary>
        /// <param name="parentId"> Represents parent content's (entity) id</param>
        /// <param name="candidate"> Represents employee </param>
        /// <returns>Partial view</returns>
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CandidateName,CandidateSurname,CandidateEmail,CandidateBirthday,PhoneNumber,HealthKnownIssues, HealthLegallyDisabled, CandidateWorkExperience, CandidateEducation, CandidateSkills, CandidateGender, AddressLine1, AddressLine2, State, Country, PostalCode, Longitude, Latitude")]Candidate candidate, int parentId = 0)
        {
            TempData["elemId"] = "candidate";
            if (ModelState.IsValid)
            {
                try
                {
                    var newCandidate = Services.ContentService.CreateContent(candidate.CandidateName + " " + candidate.CandidateSurname, parentId, "candidate");
                    newCandidate.SetValue("candidateName", candidate.CandidateName);
                    newCandidate.SetValue("candidateSurname", candidate.CandidateSurname);
                    newCandidate.SetValue("candidateGender", candidate.CandidateGender);
                    newCandidate.SetValue("candidateBirthdate", candidate.CandidateBirthday);
                    newCandidate.SetValue("candidateEmail", candidate.CandidateEmail);
                    newCandidate.SetValue("candidateEducation", candidate.CandidateEducation);
                    newCandidate.SetValue("candidateSkills", candidate.CandidateSkills);
                    newCandidate.SetValue("candidateWorkExperience", candidate.CandidateWorkExperience);
                    newCandidate.SetValue("knownIssues", candidate.HealthKnownIssues);
                    newCandidate.SetValue("legallyDisabled", candidate.HealthLegallyDisabled);
                    newCandidate.SetValue("phoneNo", candidate.PhoneNumber);
                    newCandidate.SetValue("candidateStatus", "waiting");
                    //newEmployee.SetValue("resourceTags", new []{ "asd","adss"});

                    if (Services.ContentService.SaveAndPublishWithStatus(newCandidate).Success)
                    {
                        var newAddress = Services.ContentService.CreateContent(newCandidate.Name + " Address", newCandidate.Id, "address");
                        newAddress.SetValue("postalCode", candidate.PostalCode);
                        newAddress.SetValue("longtitude", candidate.Longitude);
                        newAddress.SetValue("latitude", candidate.Latitude);
                        newAddress.SetValue("addressType", "Home Address");
                        newAddress.SetValue("addressDescriptionLine1", candidate.AddressLine1);
                        newAddress.SetValue("addressDescriptionLine2", candidate.AddressLine2);
                        newAddress.SetValue("stateName", candidate.State);
                        newAddress.SetValue("countryName", candidate.Country);
                        newAddress.SetValue("iSOCode", candidate.Country);

                        if (Services.ContentService.SaveAndPublishWithStatus(newAddress).Success)
                        {
                            TempData["Msg"] = "Candidate successfully created";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");
                        }
                        else
                        {
                            TempData["Msg"] = "Candidate created but address cannot. Check the logs.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                        }
                    }
                    TempData["Msg"] = "Cannot create candidate.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Cannot create candidate.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
            }
            TempData["Msg"] = "Cannot create candidate.";
            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
        }
        /// <summary>
        ///  Edit employee in specific content
        /// </summary>
        /// <param name="parentId"> Represents parent content's (entity) id</param>
        /// <param name="candidate"> Represents employee </param>
        /// <returns>Partial view</returns>
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CandidateName,CandidateSurname,CandidateEmail,CandidateBirthday,PhoneNumber,HealthKnownIssues, HealthLegallyDisabled, CandidateWorkExperience, CandidateEducation, CandidateSkills, CandidateGender, AddressLine1, AddressLine2, State, Country, PostalCode, Longitude, Latitude")]Candidate candidate, int parentId)
        {
            TempData["elemId"] = "candidate";
            if (ModelState.IsValid)
            {
                try
                {
                    var service = Services.ContentService;
                    var oldCandidate = service.GetById(candidate.ID);
                    oldCandidate.SetValue("candidateName", candidate.CandidateName);
                    oldCandidate.SetValue("candidateSurname", candidate.CandidateSurname);
                    oldCandidate.SetValue("candidateGender", candidate.CandidateGender);
                    oldCandidate.SetValue("candidateBirthdate", candidate.CandidateBirthday);
                    oldCandidate.SetValue("candidateEmail", candidate.CandidateEmail);
                    oldCandidate.SetValue("candidateEducation", candidate.CandidateEducation);
                    oldCandidate.SetValue("candidateSkills", candidate.CandidateSkills);
                    oldCandidate.SetValue("candidateWorkExperience", candidate.CandidateWorkExperience);
                    oldCandidate.SetValue("knownIssues", candidate.HealthKnownIssues);
                    oldCandidate.SetValue("legallyDisabled", candidate.HealthLegallyDisabled);
                    oldCandidate.SetValue("phoneNo", candidate.PhoneNumber);

                    if (service.SaveAndPublishWithStatus(oldCandidate).Success)
                    {
                        IContent oldAddress;
                        try
                        {
                            oldAddress = service.GetChildren(candidate.ID).Where(a => a.ContentType.Alias == "address").First();
                        }
                        catch (Exception ex)
                        {
                            oldAddress = service.CreateContent(candidate.CandidateName + " Address", candidate.ID, "address");
                        }

                        oldAddress.SetValue("postalCode", candidate.PostalCode);
                        oldAddress.SetValue("longtitude", candidate.Longitude);
                        oldAddress.SetValue("latitude", candidate.Latitude);
                        oldAddress.SetValue("addressType", "Home Address");
                        oldAddress.SetValue("addressDescriptionLine1", candidate.AddressLine1);
                        oldAddress.SetValue("addressDescriptionLine2", candidate.AddressLine2);
                        oldAddress.SetValue("stateName", candidate.State);
                        oldAddress.SetValue("countryName", candidate.Country);
                        oldAddress.SetValue("iSOCode", candidate.Country);
                        if (service.SaveAndPublishWithStatus(oldAddress).Success)
                        {
                            TempData["Msg"] = "Candidate edited successfully.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccess.cshtml");
                        }
                        else
                        {
                            TempData["Msg"] = "Candidate edited but address cannot.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                        }
                    }
                    TempData["Msg"] = "Cannot edit candidate.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
                catch (Exception ex)
                {

                    TempData["Msg"] = "Cannot edit candidate.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
            }

            TempData["Msg"] = "Cannot edit candidate.";
            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

        }

        /// <summary>
        ///  Edit candidate detail in specific content
        /// </summary>
        /// <param name="candidate"> Represents candidate </param>
        /// <returns>Partial view</returns>
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var service = Services.ContentService;
                    var oldCandidate = service.GetById(candidate.ID);
                    oldCandidate.SetValue("candidateName", candidate.CandidateName);
                    oldCandidate.SetValue("candidateSurname", candidate.CandidateSurname);
                    oldCandidate.SetValue("candidateGender", candidate.CandidateGender);
                    oldCandidate.SetValue("candidateBirthdate", candidate.CandidateBirthday);
                    oldCandidate.SetValue("candidateEmail", candidate.CandidateEmail);
                    oldCandidate.SetValue("candidateEducation", candidate.CandidateEducation);
                    oldCandidate.SetValue("candidateSkills", candidate.CandidateSkills);
                    oldCandidate.SetValue("candidateWorkExperience", candidate.CandidateWorkExperience);
                    oldCandidate.SetValue("knownIssues", candidate.HealthKnownIssues);
                    oldCandidate.SetValue("legallyDisabled", candidate.HealthLegallyDisabled);
                    oldCandidate.SetValue("phoneNo", candidate.PhoneNumber);
                    oldCandidate.SetValue("alternativeEmail", candidate.AlternativeEmail);

                    if (service.SaveAndPublishWithStatus(oldCandidate).Success)
                    {
                       
                            TempData["Msg"] = "Candidate edited successfully.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormSuccessDetail.cshtml");
                        }
                        else
                        {
                            TempData["Msg"] = "Cannot edit candidate.";
                            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                        }
                   
                }
                catch (Exception ex)
                {
                    
                    TempData["Msg"] = "Cannot edit candidate.";
                    return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");
                }
            }

            TempData["Msg"] = "Cannot edit candidate.";
            return PartialView(PARTIAL_VIEW_FOLDER + "FormError.cshtml");

        }

        public ActionResult GetCandidatesByJobPositions()
        {
            ChartData chartData = new ChartData();
            List<IPublishedContent> jobPosts = ContentServiceController.Instance.GetContentListByTypeAndParentId("position", -1, -1);
            foreach (IPublishedContent content in jobPosts)
            {
                chartData.KeyValues.Add(content.GetPropertyValue("positionName").ToString(), content.Descendants().Where(x => x.DocumentTypeAlias.Equals("candidate")).ToList().Count);
            }
            chartData.Title = "Candidates By Job Positions";
            chartData.Style = "bar";
            return PartialView("~/Views/Partials/Charts/Chart.cshtml", chartData);
        }
        public ActionResult GetCandidatesByStatus()
        {
            ChartData chartData = new ChartData();
            List<IPublishedContent> candidates = ContentServiceController.Instance.GetContentListByTypeAndParentId("candidate", -1, -1);
            Dictionary<String, String> statues = new Dictionary<string, string>() {
                        {"waiting","Waiting"},
                        {"phone-interview",  "Phone Interview"},
                        {"assesment",  "Assesment"},
                        {"technical-interview", "Technical Interview"},
                        {"hr-interview", "HR Interview"},
                        {"closed", "Closed"}
            };
            foreach (KeyValuePair<String,String> state in statues)
            {
                chartData.KeyValues.Add(state.Value,candidates.Where(x => x.GetPropertyValue("candidateStatus").ToString().Equals(state.Key)).ToList().Count);
            }
            chartData.Title = "Candidates By Their Status";
            chartData.Style = "pie";
            return PartialView("~/Views/Partials/Charts/Chart.cshtml", chartData);
        }
        /// <summary>
        /// Get the url of the recritment module charts
        /// </summary>
        /// <returns> List of string that contains controller URL's</returns>
        public List<String> GetCharts()
        {
            List<String> controllerUrls = new List<String>();
            controllerUrls.Add(CONTROLLER_URL + "GetCandidatesByJobPositions");
            controllerUrls.Add(CONTROLLER_URL + "GetCandidatesByStatus");
            return controllerUrls;
        }
    }
}
