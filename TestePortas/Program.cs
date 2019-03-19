using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TestePortas
{
    class Program
    {

       

        static void Main(string[] args)
        {

        
            List<string> lista = new List<string>();

            //ServicePointManager.DefaultConnectionLimit = 2;
            //ServicePointManager.DnsRefreshTimeout = 10000;
            ServicePointManager.EnableDnsRoundRobin = true;
            //ServicePointManager.MaxServicePointIdleTime = 1;
            ServicePointManager.ReusePort = true;
            //ServicePointManager.MaxServicePoints = 1;


            //HttpClient Client = new HttpClient();


            for (int i = 0; i < 10000; i++)
            {
                lista.Add(string.Format("Chamada {0}", i));
            }


            try
            {

                bool endProcess = false;
                bool showExp = false;
                var exceptions = new ConcurrentQueue<Exception>();

                //using (HttpClient Client = new HttpClient())

                //for (int i = 0; i < 200; i++)
                { 
                    //Parallel.ForEach(lista, new ParallelOptions { MaxDegreeOfParallelism = 300 }, async (currentFile) =>
                Parallel.ForEach(lista, async (currentFile) =>
                {
                    
                    

                    try
                    {
                        if (endProcess)
                        {

                            if ((exceptions.Count > 0) && (!showExp))
                            {
                                Console.WriteLine($"Finished {currentFile} on thread {Thread.CurrentThread.ManagedThreadId}");
                                Console.WriteLine(exceptions.ToArray()[0].ToString());
                                showExp = true;
                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            //ServicePointManager.ReusePort = false;
                            //ServicePointManager.DnsRefreshTimeout = 100;
                            //ServicePointManager.SetTcpKeepAlive(false, 100, 10);

                            for (int i = 0; i < 200; i++)
                            using (HttpClient Client = new HttpClient())
                            {
                                Console.WriteLine($"Processing {currentFile} on thread {Thread.CurrentThread.ManagedThreadId}");

                                var result = Client.GetAsync("http://taskerswift.azurewebsites.net/openapi").Result;
                                if (result.IsSuccessStatusCode)
                                    result = Client.GetAsync("http://fakerestapi.azurewebsites.net/api/Books").Result;
                                if (result.IsSuccessStatusCode)
                                    result = Client.GetAsync("http://andrewgodfroyportfolioapi.azurewebsites.net/api/LinkType").Result;
                                if (result.IsSuccessStatusCode)
                                    Client.GetAsync("https://graph.facebook.com");

                                var res = Client.GetAsync("https://www.api-football.com/demo/").Result;
                                var res2 = Client.GetAsync("http://slowwly.robertomurray.co.uk/delay/8000/url/https://www.api-football.com/demo/").Result;
                                var res3 = Client.GetAsync("http://slowwly.robertomurray.co.uk/delay/3000/url/https://graph.facebook.com/").Result;

                                //throw new Exception("errro");

                                //Console.WriteLine($"Result {result.StatusCode} on thread {Thread.CurrentThread.ManagedThreadId}");
                            }

                            //ServicePointManager.ReusePort = false;
                            using (HttpClient Client = new HttpClient())
                            {
                                Console.WriteLine($"Processing {currentFile} on thread {Thread.CurrentThread.ManagedThreadId}");

                                var result = Client.GetAsync("http://taskerswift.azurewebsites.net/openapi").Result;
                                if (result.IsSuccessStatusCode)
                                    result = Client.GetAsync("http://fakerestapi.azurewebsites.net/api/Books").Result;
                                if (result.IsSuccessStatusCode)
                                    result = Client.GetAsync("http://andrewgodfroyportfolioapi.azurewebsites.net/api/LinkType").Result;
                                if (result.IsSuccessStatusCode)
                                    Client.GetAsync("https://graph.facebook.com");

                                var res = Client.GetAsync("https://www.api-football.com/demo/").Result;
                                var res2 = Client.GetAsync("http://slowwly.robertomurray.co.uk/delay/8000/url/https://www.api-football.com/demo/").Result;

                                //throw new Exception("errro");

                                //Console.WriteLine($"Result {result.StatusCode} on thread {Thread.CurrentThread.ManagedThreadId}");
                            }

                            //ServicePointManager.ReusePort = false;
                            using (HttpClient Client = new HttpClient())
                            {
                                Console.WriteLine($"Processing {currentFile} on thread {Thread.CurrentThread.ManagedThreadId}");

                                var result = Client.GetAsync("http://taskerswift.azurewebsites.net/openapi").Result;
                                if (result.IsSuccessStatusCode)
                                    result = Client.GetAsync("http://fakerestapi.azurewebsites.net/api/Books").Result;
                                if (result.IsSuccessStatusCode)
                                    result = Client.GetAsync("http://andrewgodfroyportfolioapi.azurewebsites.net/api/LinkType").Result;
                                if (result.IsSuccessStatusCode)
                                    Client.GetAsync("https://graph.facebook.com");

                                var res = Client.GetAsync("https://www.api-football.com/demo/").Result;
                                var res2 = Client.GetAsync("http://slowwly.robertomurray.co.uk/delay/8000/url/https://www.api-football.com/demo/").Result;

                                //throw new Exception("errro");

                                //Console.WriteLine($"Result {result.StatusCode} on thread {Thread.CurrentThread.ManagedThreadId}");
                            }

                        }

                        //throw new Exception("asasas");

                    }
                    catch (Exception ex)
                    {

                        //Console.WriteLine($"Error  {currentFile} on thread {Thread.CurrentThread.ManagedThreadId}");

                        if((ex.ToString().IndexOf("de tempo") <=0) && (ex.ToString().IndexOf("cancelamento") <= 0) && (ex.ToString().IndexOf("task was") <= 0))
                        {
                            exceptions.Enqueue(ex);

                            if (exceptions.Count > 1)
                                endProcess = true;
                        }

                    }


                });

                }

                // Throw the exceptions here after the loop completes.
                //if (exceptions.Count > 0) throw new AggregateException(exceptions);
                if (exceptions.Count > 0) throw exceptions.ToArray()[0];

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error  {ex.ToString()} on program");
            }



            // Keep the console window open in debug mode.
            Console.WriteLine("Processing complete. Press any key to exit.");
            Console.ReadKey();
        }

        async Task<string> ProcessURLAsync(string url, HttpClient client)
        {
            var ret = await client.GetAsync(url);
            
            return ret.StatusCode.ToString();
        }

    }
}
