using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public class RestauracjaModel
    {

        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ObjectId { get; set; }
        [BsonElement("KodRestauracji")]
        public string KodRestauracji { get; set; }
        [BsonElement("Stoliki")]
        public List<StolikModel> Stolik { get; set; }
        [BsonElement("XGeoLocalization")]
        public double XGeoLocalization { get; set; }
        [BsonElement("YGeoLocalization")]
        public double YGeoLocalization { get; set; }
    }
}
