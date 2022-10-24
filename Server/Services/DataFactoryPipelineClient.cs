using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Rest;

namespace SampleApp.Server.Services
{
    internal class DataFactoryPipelineClient : IPipelineClient
    {
        private string _tenantId;
        private string _subscriptionId;
        private string _resourceGroupName;
        private string _factoryName;
        private string _pipelineName;

        public DataFactoryPipelineClient(IConfiguration configuration)
        {
            _tenantId = configuration["Azure:TenantId"];
            _subscriptionId = configuration["Azure:SubscriptionId"];
            _resourceGroupName = configuration["Azure:ResourceGroupName"];
            _factoryName = configuration["Azure:Adf:FactoryName"];
            _pipelineName = configuration["Azure:Adf:PipelineName"];
        }

        public async Task<PipelineRunDetails> TriggerPipelineAsync(IDictionary<string, object> parameters = null)
        {
            var client = await this.GetClientAsync();

            var response = await client.Pipelines.CreateRunAsync(_resourceGroupName, _factoryName, _pipelineName, parameters: parameters);

            return new PipelineRunDetails
            {
                RunId = response.RunId
            };
        }

        private async Task<DataFactoryManagementClient> GetClientAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://management.azure.com/", _tenantId);

            var cred = new TokenCredentials(accessToken);
            return new DataFactoryManagementClient(cred) { SubscriptionId = _subscriptionId };
        }
    }
}
