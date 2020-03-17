using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ModernPizzaApi
{
    public  class DBConnector
    {
        private static Semaphore _zasoby;
        public DBConnector()
        {
            _zasoby = new Semaphore(0, 1);
        }
    }
}
