using System.Net.Http;
using System.Threading.Tasks;

namespace BCLibrary.Interface
{
    /// <summary>
    /// Defines methods for authenticating and accessing the Microsoft Dynamics 365 Business Central API.
    /// </summary>
    public interface IBusinessCentralClient
    {
        /// <summary>
        /// Gets an authenticated <see cref="HttpClient"/> instance with a valid OAuth 2.0 access token for Business Central.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{HttpClient}"/> that represents the asynchronous operation.
        /// The task result contains the authenticated <see cref="HttpClient"/>.
        /// </returns>
        Task<HttpClient> GetAuthenticatedClientAsync();

        /// <summary>
        /// Constructs and returns the base API URL for accessing Business Central resources.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing the full API base URL, including the tenant ID, environment, and company ID.
        /// </returns>
        string GetBaseApiUrl();
    }
}
