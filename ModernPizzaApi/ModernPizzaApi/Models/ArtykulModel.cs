using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class ArtykulModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Tytul")]
        public String  Tytul { get; set; }
        [BsonElement("Zawartosc")]
        public String Zawartosc { get; set; }
        [BsonElement("Data")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Data { get; set; }
        
    }
}
