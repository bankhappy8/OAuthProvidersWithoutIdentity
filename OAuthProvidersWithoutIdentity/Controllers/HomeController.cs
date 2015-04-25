using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OAuthProvidersWithoutIdentity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.Session["GitHubToken"] != null)
                ViewBag.AccessToken = HttpContext.Session["GitHubToken"].ToString();

            return View();
        }

        public ActionResult AuthorizeGitHub()
        {
            return new ChallengeResult("GitHub", Url.Action("AuthorizeGitHubSuccess"));
        }

        public async Task<ActionResult> AuthorizeGitHubSuccess()
        {
            var authenticateResult = await HttpContext.GetOwinContext().Authentication.AuthenticateAsync("ExternalCookie");
            if (authenticateResult != null)
            {
                var tokenClaim = authenticateResult.Identity.Claims.FirstOrDefault(claim => claim.Type == "urn:token:github");
                if (tokenClaim != null)
                    HttpContext.Session["GitHubToken"] = tokenClaim.Value;
            }

            return RedirectToAction("Index");
        }
    }
}