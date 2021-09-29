using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;
using BlazorApp.Shared;
using System.Security.Claims;

namespace BlazorApp.VotingApi
{
    public class TableData : TableEntity
    {
        public string Vote { get; set; }
    }
    public static class GetVoting
    {
        [FunctionName("GetVoting")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Votings", "1", Connection = "VotingsDBConnection")] CloudTable table,
            ILogger log,
            ClaimsPrincipal principal)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (principal == null && !principal.Identity.IsAuthenticated)
            {
                log.LogWarning("Request was not authenticated");
                return new UnauthorizedResult();
            }

            var query = table.CreateQuery<TableData>();

            var votes = (await table.ExecuteQuerySegmentedAsync(query, null)).ToList();

            if (votes.Any())
            {
                var response = new ApiResponse
                {
                    NumberOfVoters = votes.Count(),
                    NumberOfYes = votes.Where(v => v.Vote == "yes").Count(),
                    NumberOfNo = votes.Where(v => v.Vote == "no").Count()
                };
                
                return new OkObjectResult(response);
            }

            return new OkResult();
        }
    }
}
