using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace OptimaOutlook.MGraph
{
    public class MsalAuthenticationProvider : IAuthenticationProvider
    {
        private IPublicClientApplication _clientApplication;
        private string[] _scopes;

        public MsalAuthenticationProvider(IPublicClientApplication clientApplication, string[] scopes)
        {
            _clientApplication = clientApplication;
            _scopes = scopes;
        }


        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var authentication = await GetAuthenticationAsync();
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(authentication.CreateAuthorizationHeader());
        }

        public async Task<AuthenticationResult> GetAuthenticationAsync()
        {
            AuthenticationResult authResult = null;
            var accounts = await _clientApplication.GetAccountsAsync();

            try
            {
                authResult = await _clientApplication.AcquireTokenSilent(_scopes, accounts.FirstOrDefault()).ExecuteAsync();
                //AcquireTokenSilentAsync(_scopes, accounts.FirstOrDefault());
            }
            catch (MsalUiRequiredException)
            {
                authResult = await _clientApplication.AcquireTokenInteractive(_scopes)
                            .ExecuteAsync();

                //wyslij maila że trzeba się zalogować 
            }

            return authResult;
        }
    }
}
