using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

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

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                conn.Open();
                foreach (var user in jsonArray)
                {
                    SqlCommand insertCommand = new SqlCommand("INSERT INTO [User] (Id, name, username, email) VALUES (@id, @name, @username, @email)", conn);
                    insertCommand.Parameters.AddWithValue("@id", user["id"].ToString());
                    insertCommand.Parameters.AddWithValue("@name", user["name"].ToString());
                    insertCommand.Parameters.AddWithValue("@username", user["username"].ToString());
                    insertCommand.Parameters.AddWithValue("@email", user["email"].ToString());
                    insertCommand.ExecuteNonQuery();
                }
                Console.WriteLine("Database updated");
                conn.Close();
            }
        }
    }

}
