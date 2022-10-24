namespace SampleApp.Server.Services
{
    public interface IPipelineClient
    {
        Task<PipelineRunDetails> TriggerPipelineAsync(IDictionary<string, object> parameters = null);
    }
}
