using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;


namespace bw.Function
{
 [StorageAccount("MyStorageConnectionAppSetting")]
    public static class ReadAuditLogs
    {
        [FunctionName("ReadAuditLogs")]
        public static async void Run([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
        {
	
			try
            {
               
                Helper clientObj = new Helper("ruchichopra0770");
                clientObj.CreateClientForRequest();
                await clientObj.GetAuditLogsAndStoreinBlob(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        			
		}

    }
}
