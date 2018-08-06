using baseCMS.Models;
using baseCMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace baseCMS.Controllers.Api
{
    [MemberAuthorize(AllowGroup = "Human Resources,Recruitment,Admin")]
    public class CandidateApiController : UmbracoApiController
    {

        /// <summary>
        ///  Create new employee in specific content
        /// </summary>
        /// <param name="parentId"> Represents parent content's (entity) id</param>
        /// <param name="candidate"> Represents employee </param>
        /// <returns>Partial view</returns>
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "CandidateName,CandidateSurname,CandidateEmail,CandidateBirthday,PhoneNumber,HealthKnownIssues, HealthLegallyDisabled, CandidateWorkExperience, CandidateEducation, CandidateSkills, CandidateGender, AddressLine1, AddressLine2, State, Country, PostalCode, Longitude, Latitude")]Candidate candidate, int parentId = 0)
        {
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
                            return new JsonResult { Data = new { status = "Success", message = "Candidate created" } };
                        }
                        else
                        {
                            return new JsonResult { Data = new { status = "Error", message = "Candidate could not be created" } };
                        }
                    }

                    return new JsonResult { Data = new { status = "Error", message = "Candidate could not be created" } };
                }
                catch (Exception ex)
                {
                    return new JsonResult { Data = new { status = "Error", message = "Candidate could not be created" } };
                }
            }
            return new JsonResult { Data = new { status = "Error", message = "Candidate could not be created" } };
        }

        /// <summary>
        /// Get Candidates
        /// </summary>
        /// <param name="page"></param>
        /// <param name="max"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Candidate> GetCandidates(int page = 0, int max = 10, int contentId = 0)
        {
            List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("candidate", contentId, page, max);

            List<Candidate> candidateList = new List<Candidate>();

            foreach (var content in contents)
            {
                Candidate candidate = new Candidate
                {
                    ID = content.Id,
                    CandidateName = content.GetPropertyValue("candidateName").ToString(),
                    CandidateSurname = content.GetPropertyValue("candidateSurname").ToString(),
                    CandidateGender = content.GetPropertyValue("candidateGender").ToString(),
                    CandidateBirthday = Convert.ToDateTime(content.GetPropertyValue("candidateBirthdate")),
                    CandidateEmail = content.GetPropertyValue("candidateEmail").ToString(),
                    PhoneNumber = content.GetPropertyValue("phoneNo").ToString(),
                    CandidateEducation = content.GetPropertyValue("candidateEducation").ToString(),
                    CandidateSkills = content.GetPropertyValue("candidateSkills").ToString(),
                    CandidateWorkExperience = content.GetPropertyValue("candidateWorkExperience").ToString(),
                    HealthKnownIssues = content.GetPropertyValue("knownIssues").ToString(),
                    HealthLegallyDisabled = Convert.ToBoolean(content.GetPropertyValue("legallyDisabled")),
                    CandidateUrl = content.Url
                };

                candidateList.Add(candidate);
            }


            return candidateList;
        }
        /// <summary>
        /// Changes the status of a candidate
        /// </summary>
        /// <param name="candidateId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public int UpdateCandidateStatus(int candidateId, string status)
        {
            IContent content = Services.ContentService.GetById(candidateId);
            content.SetValue("candidateStatus", status);
            if (Services.ContentService.SaveAndPublishWithStatus(content))
            {
                return 200;
            }
            else
            {
                return 400;
            }

        }
        /// <summary>
        /// Gets all the candidates with statues. Used in candidate status board
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Candidate> GetCandidatesWithStatus(int contentId)
        {
            List<IPublishedContent> contents = ContentServiceController.Instance.GetContentListByTypeAndParentId("candidate", contentId, -1);

            List<Candidate> candidateList = new List<Candidate>();

            foreach (var content in contents)
            {
                Candidate candidate = new Candidate
                {
                    ID = content.Id,
                    CandidateName = content.GetPropertyValue("candidateName").ToString(),
                    CandidateSurname = content.GetPropertyValue("candidateSurname").ToString(),
                    CandidateStatus = content.GetPropertyValue("candidateStatus").ToString(),
                    CandidateEducation = content.GetPropertyValue("candidateEducation").ToString(),
                    CandidateSkills = content.GetPropertyValue("candidateSkills").ToString(),
                    ResourceDescription = content.GetPropertyValue("resourceDescription").ToString(),
                    CandidateUrl = content.Url
                };

                candidateList.Add(candidate);
            }


            return candidateList;
        }
    }
}



