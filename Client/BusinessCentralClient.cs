using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BCLibrary.Interface;
using BCLibrary.Options;
using Microsoft.Identity.Client;

namespace BCLibrary.Client
{
    /// <summary>
    /// A client for authenticating and interacting with the Microsoft Dynamics 365 Business Central API.
    /// </summary>
    public class BusinessCentralClient : IBusinessCentralClient
    {
        private readonly BusinessCentralOptions _options;
        private HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCentralClient"/> class with configuration options.
        /// </summary>
        /// <param name="options">The configuration options required to connect to Business Central.</param>
        public BusinessCentralClient(BusinessCentralOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Returns an authenticated <see cref="HttpClient"/> instance that can be used to make requests to the Business Central API.
        /// </summary>
        /// <returns>An authenticated <see cref="HttpClient"/> with the appropriate access token set in the Authorization header.</returns>
        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            // Return cached instance if already created
            if (_httpClient != null)
                return _httpClient;

            // Define the scope for Business Central access
            var scopes = new[] { "https://api.businesscentral.dynamics.com/.default" };

            // Build the confidential client application with Azure AD credentials
            var app = ConfidentialClientApplicationBuilder.Create(_options.ClientId)
                .WithClientSecret(_options.ClientSecret)
                .WithAuthority($"https://login.microsoftonline.com/{_options.TenantId}")
                .Build();

            // Acquire an access token using the client credentials flow
            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            // Create and configure the HttpClient with the access token
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            return _httpClient;
        }

        /// <summary>
        /// Constructs and returns the base URL for accessing Business Central API endpoints for the configured tenant and company.
        /// </summary>
        /// <returns>The fully-qualified base URL for the Business Central API.</returns>
        public string GetBaseApiUrl()
        {
            // Format: https://api.businesscentral.dynamics.com/v2.0/{tenant}/{Environment}/api/v2.0/companies({companyId})/
            return $"https://api.businesscentral.dynamics.com/v2.0/{_options.TenantId}/{_options.Environment}/api/v2.0/companies?={_options.CompanyId}";
        }

        /// <summary>
        /// Acquires and returns a valid access token for the Business Central API.
        /// </summary>
        /// <returns>The access token as a string.</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            var scopes = new[] { "https://api.businesscentral.dynamics.com/.default" };

            var app = ConfidentialClientApplicationBuilder.Create(_options.ClientId)
                .WithClientSecret(_options.ClientSecret)
                .WithAuthority($"https://login.microsoftonline.com/{_options.TenantId}")
                .Build();

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            return result.AccessToken;
        }

        /// <summary>
        /// Sends an authenticated GET request to the specified Business Central API endpoint.
        /// </summary>
        /// <param name="relativeOrFullUrl">
        /// Either a full API URL (starting with https://) or a relative path appended to the environment base URL.
        /// </param>
        /// <returns>The raw JSON response as a string.</returns>
        public async Task<string> GetFromBusinessCentralApiAsync(string relativeOrFullUrl)
        {
            var httpClient = await GetAuthenticatedClientAsync();

            string requestUrl;

            if (relativeOrFullUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                // Full URL provided, use it as-is
                requestUrl = relativeOrFullUrl;
            }
            else
            {
                // Relative path provided, combine with base API URL
                requestUrl = GetBaseApiUrl().TrimEnd('/') + "/" + relativeOrFullUrl.TrimStart('/');
            }

            var response = await httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
