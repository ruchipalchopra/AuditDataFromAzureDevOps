using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bw.Function
{
    internal class Helper
    {
        private HttpClient client;
        internal Helper(string organization)
        {
            this.organization = organization;

        }
        public string organization { get; set; }
        public void CreateClientForRequest()
        {
            var personalaccesstoken = "ykrhouyr2jukvudmea4vqqbtbnjbhzcoexz7xrbuw2kcofvuhwna";
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalaccesstoken))));
        }

        public async Task<ProjectObject> GetProjectsinOrganization()
        {
            using (HttpResponseMessage response = await client.GetAsync("https://dev.azure.com/" + organization  + "/_apis/projects?api-version=5.1"))
                      {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var projects = JsonConvert.DeserializeObject<ProjectObject>(responseBody);
                return projects;
            }
        }
        public async Task<RepoObject> GetReposinProject(string projectId)
        {

            var Repourl = "https://dev.azure.com/" + organization + "/" + projectId + "/_apis/git/repositories?api-version=5.1";
            using (HttpResponseMessage response = await client.GetAsync(Repourl))
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var Repos = JsonConvert.DeserializeObject<RepoObject>(responseBody);

                return Repos;
            }
        }

        public async Task GetCommitsFromRepoandStoreinBlob(string projectId, string repoId)
        {

            var Repourl = "https://dev.azure.com/" + organization + "/" + projectId + "/_apis/git/repositories/" + repoId + "/commits?api-version=5.1";

            using (HttpResponseMessage response = await client.GetAsync(Repourl))
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.Content.Headers?.ContentType?.MediaType == "application/json")
  			    {
  			        string body;
    		     	body = await response.Content.ReadAsStringAsync();

			    	File.WriteAllText("./Output.jscsrc",body);
      		    	if(!string.IsNullOrEmpty(body))
       		     	{
        	    	    string name;
        	    	    name = "CommitsFromRepo" + TimeZoneInfo.Local.BaseUtcOffset;
         		        await AddRecievedAuditData(name + ".json", body, "auditlogdata");
        	    	}
   	 		    }
            }
         }


   public async Task GetAuditLogsAndStoreinBlob()
        {

          var RequestUri = "https://auditservice.dev.azure.com/ruchichopra0770/_apis/audit/auditlog?api-version=5.1-preview.1";

            using (HttpResponseMessage response = await client.GetAsync(RequestUri))
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.Content.Headers?.ContentType?.MediaType == "application/json")
  			    {
  			        string body;
    		     	body = await response.Content.ReadAsStringAsync();

			    //	File.WriteAllText("./Output.jscsrc",body);
      		    	if(!string.IsNullOrEmpty(body))
       		     	{
        	    	    string name;
        	    	    name = "CommitsFromRepo" + TimeZoneInfo.Local.BaseUtcOffset;
         		        await AddRecievedAuditData(name + ".json", body, "Auditlogdata");
        	    	}
   	 		    }
            }
         }

        private async Task AddRecievedAuditData(string name, string data, string containerName)
        {
            string accessKey;
            string accountName;
            string connectionString;
            CloudBlobClient client;
            CloudBlobContainer container;
            CloudBlockBlob blob;
            CloudStorageAccount storageAccount;

            accessKey = "NTH4rKiOGzUwQRuYPdgjEjf7+s1NMRo2LIZxeJ0eUmrvsLZPU+Oe6fi1sdD4kvSXEiUdPNcZGYZ469rMlRQuHg==";
            accountName = "storageaccountcommi84ac";
            connectionString = "DefaultEndpointsProtocol=https;AccountName=" + accountName + ";AccountKey=" + accessKey + ";EndpointSuffix=core.windows.net";
            storageAccount = CloudStorageAccount.Parse(connectionString);

            client = storageAccount.CreateCloudBlobClient();

            container = client.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();

            blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = "application/json";

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                await blob.UploadFromStreamAsync(stream);
            }
        }
    }
    }