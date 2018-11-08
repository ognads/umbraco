using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Skybrud.Social.Google;
using Skybrud.Social.Google.OAuth;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using baseCMS.Services;
using Umbraco.Web.PropertyEditors.ValueConverters;

namespace baseCMS.Controllers
{
    public class AuthController : SurfaceController
    {

        /// <summary>
        /// Tries to login with Google Auth
        /// </summary>
        public void Login()
        {
            string code = Request.QueryString["code"];
            string error = Request.QueryString["error"];
            string baseUrl = System.Configuration.ConfigurationManager.AppSettings["baseUrl"];
            GoogleOAuthClient oauth = new GoogleOAuthClient
            {
                ClientId = "clientId",
                ClientSecret = "clientSecret",
                RedirectUri = baseUrl + "/umbraco/surface/auth/login"
            };
            // Handle if an error occurs during the Google authentication (eg. if the user cancels the login)
            if (!String.IsNullOrWhiteSpace(error))
            {
                return;
            }
            // Handle the state when the user is redirected back to our page after a successful login with the Google API
            if (!String.IsNullOrWhiteSpace(code))
            {
                MemberServiceController memberService = new MemberServiceController();
                // Exchange the authorization code for an access token
                GoogleAccessTokenResponse response = oauth.GetAccessTokenFromAuthorizationCode(code);
                string accessToken = response.AccessToken;
                // Initialize a new instance of the GoogleService class so we can make calls to the API
                GoogleService service = GoogleService.CreateFromAccessToken(accessToken);

                // Make a call to the API to get information about the authenticated user
                GoogleUserInfo user = service.GetUserInfo();

                //Checks whether the user is logging in with a emakina account
                if (memberService.CreateNewMember(user.GivenName, user.FamilyName, user.Email, user.Id,user.Picture)){
                    FormsAuthentication.SetAuthCookie("google_" + user.Id, false);
                    Response.Redirect("/");
                }
                else
                {
                    return;
                }
            }
            else
            {
                string redirect = (Request.QueryString["redirect"] ?? "/");
                // Set the state (a unique/random value)
                string state = Guid.NewGuid().ToString();
                Session["Google_" + state] = redirect;
                // Construct the authorization URL
                string authorizationUrl = oauth.GetAuthorizationUrl(state, GoogleScopes.Email + GoogleScopes.Profile, GoogleAccessType.Online, GoogleApprovalPrompt.Force);
                // Redirect the user to the OAuth dialog
                Response.Redirect(authorizationUrl);
            }

        }
        /// <summary>
        /// Logs out the currently loggedin user
        /// </summary>
        public void Logout()
        {
            FormsAuthentication.SignOut();
            Response.Redirect("/");
        }
    }
}
