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
        //API Token available on request
        const string XApiToken = "PLACEHOLDER";
        static List<Build> buildList = new List<Build>();

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
        static async Task<bool> runBuild(string branch)
        {
            string url = $"https://api.appcenter.ms/v0.1/apps/skaggs1995-gmail.com/AppUWP/branches/{branch}/builds";
            HttpResponseMessage response = await client.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var build = JsonConvert.DeserializeObject<Build>(jsonString);
                buildList.Add(build);
                Console.WriteLine("Running build {0} on the {1} branch", build.id, build.sourceBranch);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return true;
        }
        static async Task<bool> getBuild(int id)
        {
            bool completed = false;
            Build build = null;
            string url = $"https://api.appcenter.ms/v0.1/apps/skaggs1995-gmail.com/AppUWP/builds/{id}";
            string urlLog = $"https://api.appcenter.ms/v0.1/apps/skaggs1995-gmail.com/AppUWP/builds/{id}/logs";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                build = JsonConvert.DeserializeObject<Build>(jsonString);
                if (build.status.Equals("completed"))
                {
                    completed = true;
                    Console.WriteLine("{0} build {1} in {2} seconds. " +
                        "Link to build logs: {3}" , build.sourceBranch, build.result, build.timeElapsed, urlLog);
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return completed;
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
                bool done = await runBuild(result.branch.name);
            }
            /*
             * While the builds aren't completed, search through them
             */
            int completedBuilds = 0;
            int totalBuilds = buildList.Count;
            while (completedBuilds < totalBuilds)
            {
                for(int i = 0; i < buildList.Count; i++)
                {
                    Build build = buildList[i];
                    bool complete = await getBuild(build.id);
                    if (complete)
                    {
                        buildList.Remove(build);
                        completedBuilds++;
                    }
                }
            }
        }
    }
}
