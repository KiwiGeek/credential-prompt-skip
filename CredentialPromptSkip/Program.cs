using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CredentialPromptSkip
{
    class Program
    {

        // The URI that's passed on to the CustomWeb class.
        static Uri endpoint = new Uri("");
        
        static async Task Main(string[] args)
        {

            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Connection","keep-alive");
            client.DefaultRequestHeaders.Add("DNT","1");
            client.DefaultRequestHeaders.Add("Host", "sts.lcg.org");
            client.DefaultRequestHeaders.Add("Pragma", "no-cache");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Users", "?1");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.193 Safari/537.36 Edg/86.0.622.68");

            HttpResponseMessage response = await client.GetAsync(endpoint.ToString());
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

           // Console.WriteLine(responseBody);

          //  Console.WriteLine("Delaying 5 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(1));
          //  Console.WriteLine("Continuing");

            var formData = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string?, string?>("UserName",""),      // the username you want to simulate
                new KeyValuePair<string?, string?>("Password", ""),          // the password you want to use
                new KeyValuePair<string?, string?>("AuthMethod", "FormsAuthentication"), 
            });

            HttpResponseMessage response2 = await client.PostAsync(endpoint.ToString(), formData);
            
            // Our bearer token comes back in the Location header...
            string bearer = $"Bearer {response2.Headers.Location}";
            bearer = bearer.Replace("", "");   // oldValue = the redirectURL, with the code query string. Soimething like "http://returnURL/?code="
            int stateIndex = bearer.IndexOf("&state");
            bearer = bearer.Substring(0, stateIndex);
            Console.WriteLine(bearer);

            string responseBody2 = await response2.Content.ReadAsStringAsync();
            //Console.WriteLine(responseBody2);

            Console.ReadKey();
        }
    }
}
