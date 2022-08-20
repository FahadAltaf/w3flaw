using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace W3FLA.Entities
{
    public class Post
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string avatar_template { get; set; }
        public DateTime created_at { get; set; }
        public int like_count { get; set; }
        public string blurb { get; set; }
        public int post_number { get; set; }
        public int topic_id { get; set; }
    }

    public class Listing
    {
        public string website { get; set; }
        public List<Post> posts { get; set; } = new List<Post>();
        public List<Topic> topics { get; set; } = new List<Topic>();
    }

    public class Database
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Website { get; set; }
        public int TopicId { get; set; }
        public string Data { get; set; } //Serialized DataModel
    }

    public class DataModel
    {
        public Topic Topic { get; set; }
        public Post Post { get; set; }
    }

    public class Topic
    {
        public int id { get; set; }
        public string title { get; set; }
        public string fancy_title { get; set; }
        public string slug { get; set; }
        public int posts_count { get; set; }

    }

    public class Keys
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}