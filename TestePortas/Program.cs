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
            //ServicePointManager.EnableDnsRoundRobin = true;
            //ServicePointManager.MaxServicePointIdleTime = 1;
            //ServicePointManager.ReusePort = false;
            //ServicePointManager.MaxServicePoints = 1;


            HttpClient Client = new HttpClient();


            for (int i = 0; i < 100; i++)
            {
                lista.Add(string.Format("Chamada {0}", i));
            }


            try
            {

                var exceptions = new ConcurrentQueue<Exception>();

                //Parallel.ForEach(lista, new ParallelOptions { MaxDegreeOfParallelism = 300 }, async (currentFile) =>
                Parallel.ForEach(lista, async (currentFile) =>
                {

                    Console.WriteLine($"Processing {currentFile} on thread {Thread.CurrentThread.ManagedThreadId}");

                    try
                    {
                        var result = Client.GetAsync("https://taskerswift.azurewebsites.net/openapi").Result;
                        if (result.IsSuccessStatusCode)
                            result = Client.GetAsync("https://fakerestapi.azurewebsites.net/api/Books").Result;
                        if (result.IsSuccessStatusCode)
                            result = Client.GetAsync("http://andrewgodfroyportfolioapi.azurewebsites.net/api/LinkType").Result;

                        throw new Exception("errro");

                        Console.WriteLine($"Result {result.StatusCode} on thread {Thread.CurrentThread.ManagedThreadId}");

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Error  {currentFile} on thread {Thread.CurrentThread.ManagedThreadId}");

                        exceptions.Enqueue(ex);
                    }
                    

                });

                // Throw the exceptions here after the loop completes.
                //if (exceptions.Count > 0) throw new AggregateException(exceptions);
                if (exceptions.Count > 0) throw exceptions.ToArray()[0];

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error  {ex.Message} on program");
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
