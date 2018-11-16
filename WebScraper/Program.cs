using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using 

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://jsonplaceholder.typicode.com/posts

            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/users", Method.GET);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            var jsonArray = JArray.FromObject(JsonConvert.DeserializeObject(content));

            foreach (var user in jsonArray)
            {
                
                Console.Write(user);

            }
            
        }
    }

}
