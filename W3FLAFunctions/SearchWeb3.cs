using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Collections.Generic;
using W3FLA.Entities;

namespace W3FLAFunctions
{
    public static class SearchWeb3
    {
        [FunctionName("SearchWeb3")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string query = req.Query["q"];
            string website = req.Query["website"];
            List<Listing> data = new List<Listing>();
            if (!string.IsNullOrEmpty(query))
                for (int i = 1; i <= 20; i++)
                {
                    var result = GetData($"https://{website}", query, i, log);
                    if (result != null && result.posts.Count > 0)
                    {
                        data.Add(result);
                    }
                }
            return new OkObjectResult(data);
        }

        public static Listing GetData(string website, string query, int page, ILogger log)
        {
            try
            {
                var client = new RestClient($"{website}/search?q={query}&page={page}");
                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("accept", "application/json");
                request.AddHeader("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                var response = client.Execute<Listing>(request);
                if (response != null && response.Data != null)
                {
                    response.Data.website = website;
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
            return null;
        }
    }
}

