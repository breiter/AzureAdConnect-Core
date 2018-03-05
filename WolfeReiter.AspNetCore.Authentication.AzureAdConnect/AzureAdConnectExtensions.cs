using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using WolfeReiter.AspNetCore.Authentication.AzureAD;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AzureAdConnectExtensions
    {
        public static AuthenticationBuilder AddAzureAdConnect(this AuthenticationBuilder builder)
            => builder.AddAzureAdConnect(OpenIdConnectDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddAzureAdConnect(this AuthenticationBuilder builder, Action<OpenIdConnectOptions> configureOptions)
            => builder.AddAzureAdConnect(OpenIdConnectDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddAzureAdConnect(this AuthenticationBuilder builder, string authenticationScheme, Action<OpenIdConnectOptions> configureOptions)
            => builder.AddAzureAdConnect(authenticationScheme, OpenIdConnectDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddAzureAdConnect(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<OpenIdConnectOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<OpenIdConnectOptions>, OpenIdConnectPostConfigureOptions>());
            return builder.AddRemoteScheme<OpenIdConnectOptions, AzureAdConnectHandler>(authenticationScheme, displayName, configureOptions);
        }

    }
}