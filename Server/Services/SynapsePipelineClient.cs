using Azure.Analytics.Synapse.Artifacts;
using Azure.Identity;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Rest;

namespace SampleApp.Server.Services
{
    internal class SynapsePipelineClient : IPipelineClient
    {
        private string _synapseUri;
        private string _pipelineName;

        public SynapsePipelineClient(IConfiguration configuration)
        {
            _synapseUri = configuration["Azure:Synapse:SynapseUri"];
            _pipelineName = configuration["Azure:Synapse:PipelineName"];
        }

        public async Task<PipelineRunDetails> TriggerPipelineAsync(IDictionary<string, object> parameters = null)
        {
            var client = await this.GetClientAsync();

            var response = await client.CreatePipelineRunAsync(_pipelineName, parameters: parameters);

            return new PipelineRunDetails
            {
                RunId = response.Value.RunId
            };
        }

        private async Task<PipelineClient> GetClientAsync()
        {
            var uri = new Uri(_synapseUri);
            return new PipelineClient(uri, new DefaultAzureCredential());
        }
    }
}
