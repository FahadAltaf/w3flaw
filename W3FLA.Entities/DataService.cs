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
        string connectionString = "mongodb+srv://admin:pakistan@cluster0.dabt0.mongodb.net/champdatatek-db?retryWrites=true&w=majority";
        string dbname = "w3fla";
        string ckeys = "keys";
        string cwebsites = "websites";
        string cdata = "dataset";

        public DataService()
        {
            mongoClient = new MongoClient(connectionString);
            database = mongoClient.GetDatabase(dbname);
        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
        public async Task<List<Keys>> GetKeys()
        {
            var collection = database.GetCollection<Keys>(ckeys);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task DeleteKey(string id)
        {
            var collection = database.GetCollection<Keys>(ckeys);
            var filter = Builders<Keys>.Filter.Eq(s => s.Id, id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task CreateKey(string name)
        {
            var collection = database.GetCollection<Keys>(ckeys);
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

        public async Task<List<Websites>> GetWebsites()
        {
            var collection = database.GetCollection<Websites>(cwebsites);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Database>> GetData()
        {
            var collection = database.GetCollection<Database>(cdata);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task InsertData(List<Database> data)
        {
            var collection = database.GetCollection<Database>(cdata);
            await collection.InsertManyAsync(data);
        }
    }
}
