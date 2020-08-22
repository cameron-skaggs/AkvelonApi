using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    class Program
    {
        static HttpClient client = new HttpClient();
        const string XApiToken = "4edd1b5cb33b861bc35ddb585ba9d668e0726a15";

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }
        public static async Task<List<JsonResult>> getBranch()
        {
            List<JsonResult> branchList = null;
            string url = "https://api.appcenter.ms/v0.1/apps/skaggs1995-gmail.com/AppUWP/branches";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                branchList = JsonConvert.DeserializeObject<List<JsonResult>>(jsonString);

            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            
            return branchList;
        }
        static async Task RunAsync()
        {
            List<JsonResult> resultList = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-Token", XApiToken);
            //Get Branches
            resultList = await getBranch();
            foreach (var result in resultList)
            {
                Console.WriteLine(result.branch.name);
            }
        }
    }
}
