// created by: Rahil Ansari
//Date : 28 October, 2022

using System;

using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace TransfloTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Intializing http client to send and recieve Api requests
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Task 1 - Get Calll for all posts
                // saveResult variable will be used to check whether to save result in text file as required by task 1 
                Boolean saveResult = true;
                GetCall(client, new Uri("https://jsonplaceholder.typicode.com/posts"), saveResult).Wait();

                //Task 2 - Get call for single post
                saveResult = false;
                GetCall(client, new Uri("https://jsonplaceholder.typicode.com/posts/1"), saveResult).Wait();

                //Task 3 - Post Call
                PostCall(client, new Uri("https://jsonplaceholder.typicode.com/posts")).Wait();

                //Task 4 - Put Call
                PutCall(client, new Uri("https://jsonplaceholder.typicode.com/posts/1")).Wait();

                //Task 6  - Delete Call
                DeleteCall(client, new Uri("https://jsonplaceholder.typicode.com/posts/1")).Wait();

            }
            Console.ReadKey();

        }

        static async Task GetCall(HttpClient client , Uri url , Boolean saveResult)
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;

                if (saveResult == true)
                    File.WriteAllText("E:\\Task_Result.txt", result); // Writing result to a new file in E drive
                else
                    Console.WriteLine(result);

                Console.WriteLine("Get Request complete. \n\n");

            }catch(Exception ex) // handling any thrown exception
            {
                Console.WriteLine("Error Occured: "+ex.Message);
            }
        }


        static async Task PostCall(HttpClient client, Uri url)
        {
            //Initializing the ItemData object with given data
            ItemData data = new ItemData()
            {
                title = "Foo",
                body = "bar",
                userid = 1
            };

            //converting the object dta into suitable format for Request
            var jsonData = JsonConvert.SerializeObject(data);
            var payload = new StringContent(jsonData, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(url , payload).Result;
                response.EnsureSuccessStatusCode();   // checking for any exceptions

                Console.WriteLine("Post request complete. \n\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured: " + ex.Message);
            }
        }

        static async Task PutCall(HttpClient client, Uri url)
        {
            // initializing ItemData object with given data
            ItemData data = new ItemData()
            {
                title = "Update Driver",
                body = "Update Driver Properties using UI",
                userid = 1
            };

            var jsonData = JsonConvert.SerializeObject(data);
            var payload = new StringContent(jsonData, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PutAsJsonAsync(url, payload).Result; // making request
                if (response.IsSuccessStatusCode)   // ensuring the successs status of request
                {
                    Console.WriteLine("Put Request complete. \n\n");
                }
                else
                {
                    Console.WriteLine("Put Request Unsuccessful.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured: " + ex.Message);
            }
        }


        static async Task DeleteCall(HttpClient client, Uri url)
        {

            try
            {
                HttpResponseMessage response = client.DeleteAsync(url).Result; // making delete request 
                if (response.IsSuccessStatusCode)     // ensuring the successs status of request
                {
                    Console.WriteLine("Delete Request complete. \n\n");
                }
                else
                {
                    Console.WriteLine("Delete Request Unsuccessful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured: " + ex.Message);
            }
        }

    }

    /// <summary>
    ///  Creaing ItemData class to hold  information to be sent in requests
    /// </summary>
    public class ItemData
    {
        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int userid { get; set; }


    }
}
