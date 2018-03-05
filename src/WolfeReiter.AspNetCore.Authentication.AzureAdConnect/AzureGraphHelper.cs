using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace WolfeReiter.AspNetCore.Authentication.AzureAD
{
    public class AzureGraphHelper
    {
        public AzureGraphHelper(AzureAdConnectOptions options)
        {
            Options = options;
        }

        AzureAdConnectOptions Options { get; set; }

        public async Task<IEnumerable<Group>> AzureGroups(ClaimsPrincipal principal)
        {
            var userObjectID = principal.FindFirst(AzureClaimTypes.ObjectIdentifier).Value;
            var ids = GroupIDs(principal);
            var graphClient = GetAuthenticatedClient();

            var tasks = new List<Task<Group>>();
            foreach(var id in ids)
            {
                tasks.Add(graphClient.Groups[id].Request().GetAsync());
            }
            return await Task.WhenAll(tasks);
        }

        GraphServiceClient GetAuthenticatedClient()
        {
            var authenticationProvider = new DelegateAuthenticationProvider(
                async(requestMessage) => 
                {
                    var token = await GraphToken();
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                });
            var graphClient = new GraphServiceClient(Options.GraphEndpoint, authenticationProvider);
            return graphClient;
        }
        async Task<string> GraphToken()
        {
            var credential = new ClientCredential(Options.ClientId, Options.ClientSecret);
            var authContext = new AuthenticationContext(Options.Authority);
            AuthenticationResult result = await authContext.AcquireTokenAsync(Options.GraphAuthority, credential);
            return result.AccessToken;
        }
        static IEnumerable<string> GroupIDs(ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == "groups").Select(x => x.Value.ToLower());
        }
    }
}