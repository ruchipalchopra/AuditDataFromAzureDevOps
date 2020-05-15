using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace bw.Function
{
    public static class ReadCommitsDetailsFromOrganization
    {
        [FunctionName("ReadCommitsDetailsFromOrganization")]
        
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            ProjectObject projects = null;
            RepoObject Repos = null;
            try
            {
               
               Helper clientObj = new Helper("ruchichopra0770");
                    // Get List of Projects in Org
                  clientObj.CreateClientForRequest();
                  projects = await clientObj.GetProjectsinOrganization(); 
                   // For each Project get the list of repositories
                    
                foreach (var project in projects.value)
                {
                    Repos = await clientObj.GetReposinProject(project.id); 
                    foreach (var Repo in Repos.value)
                    {
                        await clientObj.GetCommitsFromRepoandStoreinBlob(project.id, Repo.id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            string name = req.Query["name"];;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
