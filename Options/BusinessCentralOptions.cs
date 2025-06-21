using BCLibrary.Enums;

namespace BCLibrary.Options
{
    /// <summary>
    /// Configuration options required to authenticate and interact with the Microsoft Dynamics 365 Business Central API.
    /// </summary>
    public class BusinessCentralOptions
    {
        /// <summary>
        /// Gets or sets the Azure Active Directory tenant ID.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the client (application) ID registered in Azure AD.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret associated with the Azure AD application.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the Business Central company ID to target.
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the target environment (Production or Sandbox).
        /// Defaults to <see cref="BusinessCentralEnvironment.Sandbox"/>.
        /// </summary>
        public string Environment { get; set; } 
    }
}
