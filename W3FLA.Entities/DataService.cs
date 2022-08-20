using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W3FLA.Entities
{
    public class DataService
    {
        IMongoClient mongoClient;
        IMongoDatabase database;

        public DataService()
        {
            mongoClient = new MongoClient(GetEnvironmentVariable("DbConnectionString"));
            database = mongoClient.GetDatabase(GetEnvironmentVariable("DbName"));

        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
        public async Task<List<Keys>> GetKeys()
        {
            var collection = database.GetCollection<Keys>(GetEnvironmentVariable("DbCollectionKeys"));
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task DeleteKey(string name)
        {
            var collection = database.GetCollection<Keys>(GetEnvironmentVariable("DbCollectionKeys"));
            var filter = Builders<Keys>.Filter.Eq(s => s.Name.ToLower(), name.ToLower());
            await collection.DeleteOneAsync(filter);
        }

        public async Task CreateKey(string name)
        {
            var collection = database.GetCollection<Keys>(GetEnvironmentVariable("DbCollectionKeys"));
            if (!string.IsNullOrEmpty(name))
            {
                var keys = await GetKeys();
                var found = keys.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
                if (found == null)
                {
                    await collection.InsertOneAsync(new Keys { Name = name });
                }
                else
                    throw new Exception("Key has been already added");
            }
            else
                throw new Exception("No key has been provided to insert");
        }
    }
}
