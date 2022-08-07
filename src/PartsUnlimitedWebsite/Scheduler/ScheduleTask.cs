using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using PartsUnlimited.Models;
using System.Net.Http;
using Newtonsoft.Json;

using Microsoft.Extensions.Hosting;

namespace PartsUnlimited.Scheduler
{
    public class ScheduleTask : ScheduledProcessor
    {
        //PartsUnlimitedContext _context;
        IHostingEnvironment _env;
        private async Task<AzureAppService> GetAzureAppService()
        {
            FileProcessor fp = new FileProcessor(_env);
            Havok item = JsonConvert.DeserializeObject<Havok>(fp.LoadJsonFromAppFolder("\\", "havok.json"));
            string tenantId = item.TenantId; 
            string clientId = item.ClientId; 
            string clientSecret = item.ClientSecret; 
            string token = await AuthenticationHelpers.AcquireTokenBySPN(tenantId, clientId, clientSecret);

           
            string path = String.Format("https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Web/serverfarms/{2}?api-version=2016-09-01", item.SubscriptionId, item.resourceGroupName, item.AppServiceName);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.BaseAddress = new Uri("https://management.azure.com/");
                HttpResponseMessage response = await client.GetAsync(path);
                AzureAppService appService;
                return appService = JsonConvert.DeserializeObject<AzureAppService>(await response.Content.ReadAsStringAsync());

            }

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");

        }

        public ScheduleTask(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "*/1 * * * *";

        public override async Task<Task> ProcessInScope(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Processing starts here");

             _env = serviceProvider.GetService<IHostingEnvironment>();


            FileProcessor fp = new FileProcessor(_env);

            Havok item = JsonConvert.DeserializeObject<Havok>(fp.LoadJsonFromAppFolder("\\", "havok.json"));


            if (item.resourceGroupName != null)
            {
                AzureAppService apps = await GetAzureAppService();
                if (apps.properties.numberOfWorkers > 1)
                {
                    item.isScaledOut = true;
                }
                else
                {
                    item.isScaledOut = false;
                }
               string  json = JsonConvert.SerializeObject(item);
                fp.SaveJsonToAppFolder("\\", "havok.json", json);
            }

            return Task.CompletedTask;
        }
    }
}
