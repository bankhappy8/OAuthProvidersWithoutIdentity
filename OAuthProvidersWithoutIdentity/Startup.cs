using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.GitHub;

[assembly: OwinStartup(typeof(OAuthProvidersWithoutIdentity.Startup))]

namespace OAuthProvidersWithoutIdentity
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ExternalCookie",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieName = ".AspNet.ExternalCookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
            });

            var options = new GitHubAuthenticationOptions
            {
                ClientId = "921b00c13dbb0574812f",
                ClientSecret = "00367a8f18c7d8cd45ed3e34de64e7fdacec9034",
                Provider = new GitHubAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new Claim("urn:token:github", context.AccessToken));

                        return Task.FromResult(true);
                    }
                }
            };
            app.UseGitHubAuthentication(options);
        }
    }
}
