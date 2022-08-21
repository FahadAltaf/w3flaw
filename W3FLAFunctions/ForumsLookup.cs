using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using RestSharp;
using W3FLA.Entities;

namespace W3FLAFunctions
{
    public class ForumsLookup
    {
        [FunctionName("ForumsLookup")]
        public async Task Run([TimerTrigger("0 0 */12 * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                List<Database> dbData =await new DataService().GetData();

                var keys = await new DataService().GetKeys();
                var websites = await new DataService().GetWebsites();

                List<Database> toSend = new List<Database>();
                foreach (var website in websites)
                {
                    foreach (var query in keys)
                    {
                        List<Database> records = new List<Database>();

                        List<Listing> data = new List<Listing>();
                        for (int i = 1; i <= 20; i++)
                        {
                            var result = GetData($"https://{website.Name}", query.Name, i, log);
                            if (result != null && result.posts.Count > 0)
                            {
                                data.Add(result);
                            }
                        }

                        foreach (var d in data)
                        {
                            foreach (var topic in d.topics)
                            {
                                var entry = new Database
                                {
                                    Key = query.Name.ToLower(),
                                    TopicId = topic.id,
                                    Website = website.Name,
                                    Data = JsonSerializer.Serialize(new DataModel
                                    {
                                        Post = d.posts[d.topics.IndexOf(topic)],
                                        Topic = topic
                                    })
                                };

                                var dbEntry = dbData.FirstOrDefault(x => x.Website == entry.Website &&
                                x.TopicId == entry.TopicId && x.Key == entry.Key);
                                if (dbEntry == null)
                                    records.Add(entry);
                            }
                        }

                        if (records.Count > 0)
                        {
                            //insert into database
                            toSend.AddRange(records);
                            await new DataService().InsertData(records);
                            log.LogInformation("New records found: "+records.Count);
                        }
                    }
                }


                //Send an email
                log.LogInformation("Email has bee sent with records count: "+toSend.Count);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
        }

        public static Listing GetData(string website, string query, int page, ILogger log)
        {
            try
            {
                var client = new RestClient($"{website}/search?q={query}&page={page}");
                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("accept", "application/json");
                request.AddHeader("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                var response = client.Execute<Listing>(request);
                if (response != null && response.Data != null)
                {
                    response.Data.website = website;
                    return response.Data;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
            return null;
        }
    }
}
