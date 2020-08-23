using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    class ApiHelper
    {
        const string XApiToken = "4edd1b5cb33b861bc35ddb585ba9d668e0726a15";
        public static HttpClient client { get; set; }

        public static void startClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-Token", XApiToken);
            client.BaseAddress = new Uri("https://api.appcenter.ms/v0.1/apps/skaggs1995-gmail.com/AppUWP/");
        }
    }
}
