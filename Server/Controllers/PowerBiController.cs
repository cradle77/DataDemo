using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using SampleApp.Shared;

namespace SampleApp.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PowerBiController : Controller
    {
        private ITokenAcquisition _tokenAcquisition;

        public PowerBiController(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        [HttpGet("accessToken")]
        public async Task<IActionResult> GetEmbedTokenForUser()
        {
            var scopes = new[] 
            { 
                "https://analysis.windows.net/powerbi/api/Report.Read.All"
            };
            
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            
            using (var powerBiClient = new PowerBIClient(new Uri("https://api.powerbi.com"), new TokenCredentials(accessToken, "Bearer")))
            {
                var reportId = Guid.Parse(".. the report Id goes here ..");
                var groupId = Guid.Parse(".. the group Id goes here ..");

                var embedResponse = await powerBiClient.Reports.GetReportInGroupAsync(groupId, reportId);

                var generateTokenRequestParameters =
                    new GenerateTokenRequest(accessLevel: "view");

                var tokenResponse =
                    await powerBiClient.Reports.GenerateTokenAsync(
                        groupId, reportId, generateTokenRequestParameters);

                var result = new PowerBiReportEmbedDetails()
                {
                    EmbedToken = tokenResponse,
                    EmbedUrl = embedResponse.EmbedUrl,
                    Id = reportId.ToString()
                };

                return Ok(result);
            }
        }
    }
}
