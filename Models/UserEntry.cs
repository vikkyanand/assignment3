using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class UserEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("DateTime")]
    public DateTime DateTime { get; set; }

    [BsonElement("IpAddress")]
    public string IpAddress { get; set; }
}
