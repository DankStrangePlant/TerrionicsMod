using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Dynamic;
using System.Runtime;

namespace SocketTest
{
    class Packet : DynamicObject
    {
        private readonly Dictionary<string, object> dictionary;

        public Packet() {}

        public Packet(String JSON)
        {
            dynamic dynJson = JsonConvert.DeserializeObject(JSON);
            foreach (var item in dynJson)
            {
                dictionary.Add(item, dynJson[item]);
            }
        }

        public String serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Packet(Dictionary<string, object> dictionary)
        {
            this.dictionary = dictionary;
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            return dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;

            return true;
        }
    }
}
