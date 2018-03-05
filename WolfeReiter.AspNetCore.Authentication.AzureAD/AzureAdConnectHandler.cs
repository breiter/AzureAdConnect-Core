using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Principal;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace WolfeReiter.AspNetCore.Authentication.AzureAD
{
    /// <summary>
    /// A per-request authentication handler for AzureAD that maps Groups to RoleClaims.
    /// </summary>
    public class AzureAdConnectHandler : OpenIdConnectHandler, IAuthenticationSignOutHandler
    {
        public AzureAdConnectHandler(IOptionsMonitor<AzureAdConnectOptions> options, ILoggerFactory logger, HtmlEncoder htmlEncoder, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, htmlEncoder, encoder, clock)
        {
            Options = options.CurrentValue;
            AzureGraphHelper = new AzureGraphHelper(options.CurrentValue);
        }

        new protected AzureAdConnectOptions Options { get; set; }
        AzureGraphHelper AzureGraphHelper { get; set; }

        /// <summary>
        /// Invoked to process incoming OpenIdConnect messages.
        /// </summary>
        /// <returns>An <see cref="HandleRequestResult"/>.</returns>
        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var result = await base.HandleRemoteAuthenticateAsync();
            if(result.Succeeded) await AddAzureAdGroupRoleClaims(result.Principal);
            return result;  
        }

        async Task AddAzureAdGroupRoleClaims(ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal != null && incomingPrincipal.Identity.IsAuthenticated == true)
            {
                try
                {
                    Tuple<DateTime, IEnumerable<string>> grouple = null;
                    IEnumerable<string> groups = Enumerable.Empty<string>();
                    var identity = (ClaimsIdentity)incomingPrincipal.Identity;
                    var identityKey = identity.Name;
                    var cacheValid = false;

                    if (PrincipalRoleCache.TryGetValue(identityKey, out grouple))
                    {
                        var expiration = grouple.Item1.AddSeconds(Options.GroupCacheTtlSeconds);
                        if (DateTime.UtcNow > expiration ||
                            grouple.Item2.Count() != identity.Claims.Count(x => x.Type == "groups"))
                        {
                            //don't need to check return because if it failed, then the entry was removed already
                            PrincipalRoleCache.TryRemove(identityKey, out grouple);
                        }
                        else
                        {
                            cacheValid = true;
                            groups = grouple.Item2;
                        }
                    }

                    if (!cacheValid)
                    {
                        var result = await AzureGraphHelper.AzureGroups(incomingPrincipal);
                        groups = result.Select(x => x.DisplayName);
                        grouple = new Tuple<DateTime, IEnumerable<string>>(DateTime.UtcNow, groups);
                        PrincipalRoleCache.AddOrUpdate(identityKey, grouple, (key, oldGrouple) => grouple);
                    }

                    foreach (var group in groups)
                    {
                        //add AzureAD Group claims as Roles.
                        identity.AddClaim(new Claim(ClaimTypes.Role, group, ClaimValueTypes.String, "AzureAD"));
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Exception Mapping Groups to Roles");
                    string userObjectID = incomingPrincipal.FindFirst(AzureClaimTypes.ObjectIdentifier).Value;
                    var authContext = new AuthenticationContext(Options.Authority);
                    var cacheitem = authContext.TokenCache.ReadItems().Where(x => x.UniqueId == userObjectID).SingleOrDefault();
                    if (cacheitem != null) authContext.TokenCache.DeleteItem(cacheitem);
                }
            }
        }

         protected override async Task<bool> HandleRemoteSignOutAsync()
         {
             return await base.HandleRemoteSignOutAsync();
         }

        static ConcurrentDictionary<string, Tuple<DateTime, IEnumerable<string>>> PrincipalRoleCache { get; set; }
        static AzureAdConnectHandler()
        {
            PrincipalRoleCache = new ConcurrentDictionary<string, Tuple<DateTime, IEnumerable<string>>>();
        }
    }
}