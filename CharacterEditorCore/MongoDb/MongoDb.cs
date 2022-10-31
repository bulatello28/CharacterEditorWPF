using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditorCore.MongoDb
{
    public class MongoDb
    {
        public static void AddToDataBase(Character character)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("TempCharacters");
            var collection = database.GetCollection<Character>("TempCharactersCollection");
            collection.InsertOne(character);
        }

        public static void ReplaceOneInDataBase(Character character)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("TempCharacters");
            var filter = new BsonDocument("_id", character._id);
            var collection = database.GetCollection<Character>("TempCharactersCollection");
            collection.ReplaceOne(filter, character);
        }

        public static Character FindById(string id)
        {
            var client = new MongoClient("mongodb://localhost");
            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            var database = client.GetDatabase("TempCharacters");
            var collection = database.GetCollection<Character>("TempCharactersCollection");
            return collection.Find(filter).FirstOrDefault();
        }


        public static IMongoCollection<Character> GetCollection()
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("TempCharacters");
            return database.GetCollection<Character>("TempCharactersCollection");
        }

        public static IMongoCollection<Match> GetMatchesCollection()
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("TempCharacters");
            return database.GetCollection<Match>("Matches");
        }

        public static void AddMatchToDataBase(Match match)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("TempCharacters");
            var collection = database.GetCollection<Match>("Matches");
            collection.InsertOne(match);
        }
    }
}
