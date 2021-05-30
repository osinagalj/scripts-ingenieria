
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System.Text;

///</sumary> 
/// Esta clase es la encargada de serializar y deserilizar los objetos TeamBotId desde la DB
///</sumary>     
/// @Author: Lautaro

public class DBAgileBot : DataBaseAccess<TeamBotId>
{   
    public static DBAgileBot Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        base.Start();
    }

    public List<TeamBotId> GetAll()
    {
        List<TeamBotId> collect = new List<TeamBotId>();
        collect = GetAll(new BsonDocument());
        return collect;
    }

    
    public List<TeamBotId> GetAll(BsonDocument filter)
    {
        List<BsonDocument> allBsons = collection.FindSync(filter).ToList();
        

        List<TeamBotId> collect = new List<TeamBotId>();
        foreach (var bson in allBsons)
        {
            string json = ToJson(bson);
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            TeamBotId p = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamBotId>(json,settings);

            collect.Add(p);
        }
        return collect;
    }

    public string ToJson(BsonDocument bson)
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new BsonBinaryWriter(stream))
            {
                BsonSerializer.Serialize(writer, typeof(BsonDocument), bson);
            }
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new Newtonsoft.Json.Bson.BsonReader(stream))
            {
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                using (var jWriter = new JsonTextWriter(sw))
                {
                    jWriter.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    jWriter.WriteToken(reader);
                }
                return sb.ToString();
            }
        }
    }
}

