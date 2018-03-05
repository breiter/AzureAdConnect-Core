using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace WolfeReiter.AspNetCore.Authentication.AzureAD
{
    public class AzureAdConnectOptions : OpenIdConnectOptions
    {

        public AzureAdConnectOptions()
        {
            ReadGraphAsLoggedInUser = false;
            AzureGraphEndpoint      = "https://graph.microsoft.com/v1.0";
            GroupCacheTtlSeconds    = 60 * 60;
        }

        /// <summary>
        /// Whether to authenticate to the Graph as the current user. When false, logs in as the application.
        /// Defaults to false.
        /// </summary>
        /// <returns></returns>
        public bool ReadGraphAsLoggedInUser { get; set; }

        /// <summary>
        /// URL to invoke Azure Graph API. Defaults to "https://graph.microsoft.com/v1.0".
        /// </summary>
        /// <returns></returns>
        public string AzureGraphEndpoint { get; set; }

        /// <summary>
        /// Number of seconds to cache groups locally in memory before requerying Graph. Default is 3600 (1 hour).
        /// </summary>
        /// <returns></returns>
        public int GroupCacheTtlSeconds { get; set; }
    }
}