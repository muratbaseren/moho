using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Moho.Web.Models
{
    public static class IocHelper
    {
        public static MongoDbHelper mongoHelper = new MongoDbHelper();
    }

    public class MongoDbHelper
    {
        public IMongoClient _client = null;
        public IMongoDatabase _database = null;
        public readonly string _databaseName = string.Empty;


        public MongoDbHelper()
        {
            _databaseName = ConfigurationManager.AppSettings["DatabaseName"];

            _client = new MongoClient();
            _database = _client.GetDatabase(_databaseName);
        }

        public MongoDbHelper(string connectionString, string database)
        {
            _databaseName = database;

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(_databaseName);
        }


        public List<Screen> FindScreens()
        {
            var col = _database.GetCollection<Screen>("screens");

            return col.Find(new BsonDocument()).ToList();
        }

        public Screen FindScreenById(string id)
        {
            var col = _database.GetCollection<Screen>("screens");
            var filter = Builders<Screen>.Filter.Eq("_id", ObjectId.Parse(id));

            return col.Find(filter).FirstOrDefault();
        }

        public Screen FindScreenByUriName(string uriName)
        {
            var col = _database.GetCollection<Screen>("screens");
            var filter = Builders<Screen>.Filter.Eq("UriName", uriName);

            return col.Find(filter).FirstOrDefault();
        }

        public void InsertScreen(Screen scr)
        {
            var col = _database.GetCollection<Screen>("screens");
            col.InsertOne(scr);
        }


        public List<Dictionary<string, object>> FindUnknownCollection(Screen screen)
        {
            var col = _database.GetCollection<Dictionary<string, object>>(screen.CollectionName);
            var items = col.Find(new BsonDocument()).ToList();

            return items;
        }

        public Dictionary<string, object> FindItemFromUnknownCollection(Screen screen, string id)
        {
            var col = _database.GetCollection<Dictionary<string, object>>(screen.CollectionName);
            var filter = Builders<Dictionary<string, object>>.Filter.Eq("_id", ObjectId.Parse(id));
            var item = col.Find(filter).FirstOrDefault();

            return item;
        }

        public void UpdateToUnknownCollection(Screen screen, Dictionary<string, object> values)
        {
            var col = _database.GetCollection<Dictionary<string, object>>(screen.CollectionName);
            var filter = Builders<Dictionary<string, object>>.Filter.Eq("_id", ObjectId.Parse(values["_id"].ToString()));

            if (values.Count > 0 && screen.ScreenFields.Count > 0)
            {
                if (values.ContainsKey("_id"))
                {
                    var id = values["_id"].ToString();

                    values.Remove("_id");
                    col.ReplaceOne(filter, values);
                    values.Add("_id", id);
                }
                else
                {
                    col.ReplaceOne(filter, values);
                }
            }
        }

        public void InsertToUnknownCollection(Screen screen, Dictionary<string, object> values)
        {
            var col = _database.GetCollection<Dictionary<string, object>>(screen.CollectionName);
            col.InsertOne(values);
        }

        public void DeleteFromUnknownCollection(Screen screen, string id)
        {
            var col = _database.GetCollection<Dictionary<string, object>>(screen.CollectionName);
            var filter = Builders<Dictionary<string, object>>.Filter.Eq("_id", ObjectId.Parse(id));
            col.DeleteOne(filter);
        }
    }

    public class Screen
    {
        private string collectionName = string.Empty;

        [BsonId]
        [ScaffoldColumn(false)]
        public ObjectId Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Url Name")]
        public string UriName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [ScaffoldColumn(false)]
        public List<ScreenField> ScreenFields { get; set; }

        [ScaffoldColumn(false)]
        public string CollectionName
        {
            get
            {
                if (string.IsNullOrEmpty(Name) == false)
                    return "scr_" + Name.Replace(" ", "_");
                else
                    return string.Empty;
            }
        }

        public Screen()
        {
            ScreenFields = new List<ScreenField>();
        }
    }

    public class ScreenField
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Input Type")]
        public ScreenFieldTypeEnum Type { get; set; }

        [DisplayName("Required")]
        public bool Required { get; set; }

        [DisplayName("MaxLength")]
        public int MaxLength { get; set; }
    }

    public enum ScreenFieldTypeEnum
    {
        Text = 0,
        Number = 1,
        Date = 2,
        DateTime = 3,
        Checkbox = 4,
        Radiobox = 5,
        TextArea = 6
    }




    public class ScreenShowViewModel
    {
        public Screen Screen { get; set; }
        public List<Dictionary<string, object>> Items { get; set; }
    }

    public class ScreenEditViewModel
    {
        public Screen Screen { get; set; }
        public Dictionary<string, object> Item { get; set; }
    }
}