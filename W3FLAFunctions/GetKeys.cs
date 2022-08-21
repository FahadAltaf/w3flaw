using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using W3FLA.Entities;
using System.Linq;

namespace W3FLAFunctions
{
    public static class GetKeys
    {
        [FunctionName("GetKeys")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var keys =await new DataService().GetKeys();
            
            return new OkObjectResult(keys);
        }
    }
}
