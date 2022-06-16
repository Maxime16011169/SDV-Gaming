using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace WebProjectAzure
{
    public class AzureVM
    {
        //Variables
        public const string azureClientId = "";
        public const string azureClientSecret = "";
        public const string azureTenantId = "";
        public const string subscriptionId = "";

        public void Authenticate()
        {
            try
            {
                var credentials = SdkContext.AzureCredentialsFactory
                    .FromServicePrincipal(azureClientId, azureClientSecret, azureTenantId, AzureEnvironment.AzureGlobalCloud);

                var azure = Microsoft.Azure.Management.Fluent.Azure
                    .Configure()
                    .Authenticate(credentials)
                    .WithSubscription(subscriptionId);

                var resourceGroup = azure.ResourceGroups.Define("groupTestBob")
                    .WithRegion("francecentral")
                    .Create();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
