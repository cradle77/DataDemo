using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Rest;
using SampleApp.Server.Services;
using SampleApp.Shared;
using System;
using System.Data.SqlClient;

namespace SampleApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataFilesController : ControllerBase
    {
        private string _storageEndpoint;
        private string _containerName;
        private string _accountName;
        private string _tenantId;
        private string _connectionString;

        private IPipelineClient _synapseClient;

        public DataFilesController(IConfiguration configuration, IPipelineClient synapseClient)
        {
            _storageEndpoint = configuration["Azure:Adls:StorageEndpoint"];
            _containerName = configuration["Azure:Adls:ContainerName"];
            _accountName = configuration["Azure:Adls:AccountName"];
            _tenantId = configuration["Azure:TenantId"];

            _synapseClient = synapseClient;

            _connectionString = configuration["Azure:Synapse:DbConnectionString"];
        }

        [HttpPost]
        public IActionResult UploadNewFile()
        {
            var result = new DataFile(Guid.NewGuid());

            var blobServiceClient = new BlobServiceClient(new Uri(_storageEndpoint), new VisualStudioCredential(
                new VisualStudioCredentialOptions
                {
                    TenantId = _tenantId
                }));

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            var blobClient = blobContainerClient.GetBlobClient(result.FilePath);

            var userDelegationKey = blobServiceClient.GetUserDelegationKey(DateTimeOffset.UtcNow.AddSeconds(-30),
                                                                    DateTimeOffset.UtcNow.AddHours(2));

            BlobSasBuilder sas = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b", // b for blob, c for container
                StartsOn = DateTimeOffset.UtcNow.AddSeconds(-30),
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(2),
            };

            sas.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

            var sasToken = sas.ToSasQueryParameters(userDelegationKey, _accountName).ToString();

            result.Url = $"{blobClient.Uri}?{sasToken}";

            return this.Ok(result);
        }

        [HttpPatch("{fileId}")]
        public async Task<IActionResult> UploadCompletedAsync(Guid fileId)
        {
            var result = new DataFile(fileId);

            await _synapseClient.TriggerPipelineAsync(new Dictionary<string, object>()
            {
                { "sourceFileName", result.FileName }
            });

            return this.Ok(result);
        }

        [HttpGet("{fileId}/preview")]
        public async Task<IActionResult> GetPreviewAsync(Guid fileId, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var credential = new VisualStudioCredential(
                    new VisualStudioCredentialOptions
                    {
                        TenantId = _tenantId
                    });

                var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }), cancellationToken);
                connection.AccessToken = token.Token;

                var file = new DataFile(fileId);

                var sql = @$"
SELECT TOP 10 * FROM
    OPENROWSET(
        BULK 'abfss://datafiles@desdatademo.dfs.core.windows.net/raw/{file.FileName}',
        FORMAT='CSV',
        parser_version = '2.0',
        HEADER_ROW = TRUE
    ) AS applications";

                var result = await connection.QueryAsync<dynamic>(sql);

                return this.Ok(result);
            }
        }
    }
}