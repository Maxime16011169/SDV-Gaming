using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Network.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;

namespace WebProjectAzure.Azure
{
    public class AzureManager
    {
        #region SINGLETON
        private static AzureManager? _instance;
        public static AzureManager Instance => _instance ??= new AzureManager();
        #endregion

        private const string SUBSCRIPTION_ID = "98baef72-a037-4a11-afcf-2be02d7b930e";
        private const string TENANT_ID = "b7b023b8-7c32-4c02-92a6-c8cdaa1d189c";
        private const string APP_ID = "a3257d71-7a81-4c7b-b2f6-8a60f8c895c3";
        private const string APP_SECRET = "azs8Q~.WwsBElOBmBIwlc5ve97EZlO--HtHpFcuJ";

        private const string RESOURCE_GROUP_NAME = "sdv-tp-1-vm";
        private const string VNET_NAME = "sdv-v-net";
        private const string SUBNET_NAME = "v-subnet";
        private const string VM_NAME_PREFFIX = "sdv-vm";

        private readonly AzureCredentials credetials;
        private readonly IAzure azure;

        private AzureManager()
        {
            credetials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(APP_ID, APP_SECRET, TENANT_ID, AzureEnvironment.AzureGlobalCloud);

            azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .WithLogLevel(Microsoft.Azure.Management.ResourceManager.Fluent.Core.HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credetials)
                .WithSubscription(SUBSCRIPTION_ID);
        }

        public IVirtualMachine? CreateVM(int vmId)
        {
            IResourceGroup resourceGroup = azure.ResourceGroups.GetByName(RESOURCE_GROUP_NAME);
            INetwork network = azure.Networks.GetByResourceGroup(RESOURCE_GROUP_NAME, VNET_NAME);

            IPublicIPAddress ipAddress = azure.PublicIPAddresses
                .Define(VM_NAME_PREFFIX + "-ip-" + vmId)
                .WithRegion(resourceGroup.RegionName)
                .WithExistingResourceGroup(resourceGroup)
                .Create();

            INetworkInterface networkInterface = azure.NetworkInterfaces
                .Define(VM_NAME_PREFFIX + "-network-interface-" + vmId)
                .WithRegion(resourceGroup.RegionName)
                .WithExistingResourceGroup(resourceGroup)
                .WithExistingPrimaryNetwork(network)
                .WithSubnet(SUBNET_NAME)
                .WithPrimaryPrivateIPAddressDynamic()
                .WithExistingPrimaryPublicIPAddress(ipAddress)
                .Create();

            return azure.VirtualMachines
                .Define(VM_NAME_PREFFIX + "-" + vmId)
                .WithRegion(resourceGroup.RegionName)
                .WithExistingResourceGroup(resourceGroup)
                .WithExistingPrimaryNetworkInterface(networkInterface)
                .WithLatestWindowsImage("MicrosoftWindowsServer", "WindowsServer", "2019-Datacenter")
                .WithAdminUsername("martinbarre29")
                .WithAdminPassword("barremartin29!")
                .WithSize(VirtualMachineSizeTypes.StandardDS2)
                .WithOSDiskName(VM_NAME_PREFFIX + "-OsDisk-" + vmId)
                .Create();
        }

    }
}
