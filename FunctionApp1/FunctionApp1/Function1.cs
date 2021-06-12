using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Storage;

namespace ThumbnailGenerator.Azure
{
    public static class ThumbnailGenerator
    {
        [FunctionName("upload")]
        [StorageAccount("AzureWebJobsStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "{name}")] HttpRequest req,
            [Blob("input/{name}", FileAccess.Write)] Stream inputImage,
            string name,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            byte[] data = System.Convert.FromBase64String(requestBody);
            await inputImage.WriteAsync(data);

            return new OkObjectResult("Succeed:" + name);
        }
    }
}