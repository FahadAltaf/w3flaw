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

namespace W3FLAFunctions
{
    public static class DeleteKey
    {
        [FunctionName("DeleteKey")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string message = "ok";
            try
            {
                string name = req.Query["key"];
                await new DataService().DeleteKey(name);
            }
            catch (Exception ex)
            {
                message = "ERROR! " + ex.Message;
            }
            return new OkObjectResult(message);
        }
    }
}
