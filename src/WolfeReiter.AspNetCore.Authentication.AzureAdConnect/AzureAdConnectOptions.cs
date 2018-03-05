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
            GraphAuthority       = "https://graph.microsoft.com";
            GraphApiVersion      = "1.0";
            GroupCacheTtlSeconds = 60 * 60;
        }

        public AzureAdConnectOptions(OpenIdConnectOptions options) : this()
        {
            this.AuthenticationMethod          = options.AuthenticationMethod;
            this.Authority                     = options.Authority;
            this.Backchannel                   = options.Backchannel;
            this.BackchannelHttpHandler        = options.BackchannelHttpHandler;
            this.BackchannelTimeout            = options.BackchannelTimeout;
            this.CallbackPath                  = options.CallbackPath;
            this.ClaimsIssuer                  = options.ClaimsIssuer;
            this.ClientId                      = options.ClientId;
            this.ClientSecret                  = options.ClientSecret;
            this.Configuration                 = options.Configuration;
            this.ConfigurationManager          = options.ConfigurationManager;
            this.CorrelationCookie             = options.CorrelationCookie;
            this.DataProtectionProvider        = options.DataProtectionProvider;
            this.DisableTelemetry              = options.DisableTelemetry;
            this.Events                        = options.Events;
            this.EventsType                    = options.EventsType;
            this.GetClaimsFromUserInfoEndpoint = options.GetClaimsFromUserInfoEndpoint;
            this.MetadataAddress               = options.MetadataAddress;
            this.NonceCookie                   = options.NonceCookie;
            this.ProtocolValidator             = options.ProtocolValidator;
            this.RefreshOnIssuerKeyNotFound    = options.RefreshOnIssuerKeyNotFound;
            this.RemoteAuthenticationTimeout   = options.RemoteAuthenticationTimeout;
            this.RemoteSignOutPath             = options.RemoteSignOutPath;
            this.RequireHttpsMetadata          = options.RequireHttpsMetadata;
            this.Resource                      = options.Resource;
            this.ResponseMode                  = options.ResponseMode;
            this.ResponseType                  = options.ResponseType;
            this.SaveTokens                    = options.SaveTokens;
            this.SecurityTokenValidator        = options.SecurityTokenValidator;
            this.SignedOutCallbackPath         = options.SignedOutCallbackPath;
            this.SignedOutRedirectUri          = options.SignedOutRedirectUri;
            this.SignInScheme                  = options.SignInScheme;
            this.SignOutScheme                 = options.SignOutScheme;
            this.SkipUnrecognizedRequests      = options.SkipUnrecognizedRequests;
            this.StateDataFormat               = options.StateDataFormat;
            this.StringDataFormat              = options.StringDataFormat;
            this.TokenValidationParameters     = options.TokenValidationParameters;
            this.UseTokenLifetime              = options.UseTokenLifetime;
        }

        /// <summary>
        /// URL to invoke Graph API. Defaults to &quot;https://graph.microsoft.com"&quot;.
        /// </summary>
        /// <returns></returns>
        public string GraphAuthority { get; set; }

        /// <summary>
        /// Microsoft Graph API version to target. Defaults to &quot;1.0&quot;.
        /// </summary>
        /// <returns></returns>
        public string GraphApiVersion { get; set; }
        public string GraphEndpoint { get { return string.Format("(0}/{1}", GraphAuthority, GraphApiVersion); } }

        /// <summary>
        /// Number of seconds to cache groups locally in memory before requerying Graph. Default is 3600 (1 hour).
        /// </summary>
        /// <returns></returns>
        public int GroupCacheTtlSeconds { get; set; }
    }
}