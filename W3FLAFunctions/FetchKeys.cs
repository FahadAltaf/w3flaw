using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using W3FLA.Entities;
using System.Linq;

namespace W3FLAFunctions
{
    public static class FetchKeys
    {
        [FunctionName("FetchKeys")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<string> keys = new List<string>();
            var ks = await new DataService().GetKeys();
            keys = ks.Select(x=>x.Name).ToList();

            return new OkObjectResult(keys);
        }
    }
}
