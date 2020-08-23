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
        static List<Build> buildList = new List<Build>();

        static void Main(string[] args)
        {
            ApiHelper.startClient();
            RunAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }
        /* RunAsync calls three functions that do the three tasks 
         * needed for this project
         * (1) GetBranch grabs all the branch names in App Center
         *     and saves it in a list
         * (2) RunBuild builds each branch based on the name given to it
         *     by the list created in Step (1)
         * (3) RunAsync will loop calling CheckBuildStatus until all Build
         *     statuses return complete. When a build is found to be complete
         *     it will print the branch name, result, time elapsed and link 
         *     to the logs
         */
        static async Task RunAsync()
        {
            /* (1) GetBranch grabs all the branch names in App Center and saves it in a list */
            List<JsonResult> resultList = await GetBranch();
            foreach (var result in resultList)
            {
                /* (2) RunBuild builds each branch based on the name given to it by the list created in Step (1) */
                await RunBuild(result.branch.name);
            }

            int completedBuilds = 0;
            int totalBuilds = buildList.Count;
            /* (3) RunAsync will loop calling CheckBuildStatus until all Build statuses return complete */
            while (completedBuilds < totalBuilds) 
            {
                for(int i = 0; i < buildList.Count; i++) // As an improvement, I'd put this logic in another function
                {
                    Build build = buildList[i];
                    bool complete = await CheckBuildStatus(build.id);
                    if (complete)
                    {
                        buildList.Remove(build);
                        completedBuilds++;
                    }
                }
            }
        }
        /* GetBranch sends a GET request for a list of all the branches in the App Center project
         * The response is structured as JSon Array => Branch Object => Branch Attributes
         * The structure of the classes are structured as JsonResult has a Branch Object which contains a name
         */
        public static async Task<List<JsonResult>> GetBranch()
        {
            List<JsonResult> branchList = null;
            string url = "branches";

            HttpResponseMessage response = await ApiHelper.client.GetAsync(url);
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
        /* RunBuild takes a branch name as a parameter
         * It then makes a POST request to the App Center API to run the named build
         * It saves the build and adds it to the build list
         * The build id and build branch are printed to the console
         * */
        static async Task<bool> RunBuild(string branch)
        {
            bool completed = false;
            string url = $"branches/{branch}/builds";
            HttpResponseMessage response = await ApiHelper.client.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                completed = true;
                var jsonString = await response.Content.ReadAsStringAsync();
                var build = JsonConvert.DeserializeObject<Build>(jsonString);
                buildList.Add(build);
                Console.WriteLine("Running build {0} on the {1} branch", build.id, build.sourceBranch);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return completed;
        }
        /* CheckBuildStatus takes a build ID as a paramter
         * It sends a GET request to the API and saves the build as a build object
         * The build status is checked, if it's completed it prints the value of the build and the result
         * and returns true.
         * If the build is not complete CheckBuildStatus returns false
         */
        static async Task<bool> CheckBuildStatus(int id)
        {
            bool completed = false;
            Build build = null;
            string url = $"builds/{id}";
            string urlLog = $"https://api.appcenter.ms/v0.1/apps/skaggs1995-gmail.com/AppUWP/builds/{id}/logs";
            HttpResponseMessage response = await ApiHelper.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                build = JsonConvert.DeserializeObject<Build>(jsonString);
                if (build.status.Equals("completed"))
                {
                    completed = true;
                    Console.WriteLine("{0} build {1} in {2} seconds. " +
                        "Link to build logs: {3}", build.sourceBranch, build.result, build.timeElapsed, urlLog);
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
            return completed;
        }
    }
}
