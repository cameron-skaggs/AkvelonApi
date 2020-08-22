using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    public class APIHelper
    {
        public static HttpClient apiClient { get; set; }

        public static void InitializeClient()
        {
            const string XApiToken = "4edd1b5cb33b861bc35ddb585ba9d668e0726a15";
            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri("https://api.appcenter.ms/v0.1/apps/skaggs1995-gmail.com/AppUWP/");
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
