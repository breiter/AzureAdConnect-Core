using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WolfeReiter.AspNetCore.Authentication.AzureAD
{
    /// <summary>
    /// A per-request authentication handler for AzureAD that maps Groups to RoleClaims.
    /// </summary>
    public class AzureAdConnectHandler : OpenIdConnectHandler, IAuthenticationSignOutHandler
    {
        public AzureAdConnectHandler(IOptionsMonitor<OpenIdConnectOptions> options, ILoggerFactory logger, HtmlEncoder htmlEncoder, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, htmlEncoder, encoder, clock)
        {

        }

        /// <summary>
        /// Invoked to process incoming OpenIdConnect messages.
        /// </summary>
        /// <returns>An <see cref="HandleRequestResult"/>.</returns>
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