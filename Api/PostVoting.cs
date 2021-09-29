using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BlazorApp.Shared;
using System.Security.Claims;

namespace BlazorApp.VotingApi
{
    public static class PostVoting
    {
        [FunctionName("PostVoting")]
        [return: Queue("votings", Connection = "VotingsDBConnection")]
        public static async Task<NewVote> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            ClaimsPrincipal principal)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newVote = JsonConvert.DeserializeObject<NewVote>(requestBody);

            return newVote;
        }
    }
}
