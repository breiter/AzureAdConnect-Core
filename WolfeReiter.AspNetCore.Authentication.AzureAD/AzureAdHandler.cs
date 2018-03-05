using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WolfeReiter.AspNetCore.Authentication.AzureAD
{
    public class AzureAdHandler : OpenIdConnectHandler, IAuthenticationSignOutHandler
    {
        public AzureAdHandler(IOptionsMonitor<OpenIdConnectOptions> options, ILoggerFactory logger, HtmlEncoder htmlEncoder, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, htmlEncoder, encoder, clock)
        {

        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var baseResult = await base.HandleRemoteAuthenticateAsync();
            if(baseResult.Principal == null || !baseResult.Principal.Identity.IsAuthenticated) return baseResult;

            var principal = baseResult.Principal;

            throw new NotImplementedException("Map AzureAD Groups as RoleClaims.");
        }

         protected override async Task<bool> HandleRemoteSignOutAsync()
         {
             return await base.HandleRemoteSignOutAsync();
         }
    }
}