using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SocketTest
{
    class Packet : Dictionary<string, object>
    {
        public Packet() {}

        public Packet(String JSON)
        {
            Packet dynJson = JsonConvert.DeserializeObject<Packet>(JSON);
            foreach (var item in dynJson)
            {
                this.Add(item.Key, item.Value);
            }
        }

        public String Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
