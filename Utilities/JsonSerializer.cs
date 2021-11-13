using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Xrm
{
    public static class JsonSerializer
    {
        public static string SerializeJson<T>(T instance)
        {
            byte[] json;

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, instance);
                json = stream.ToArray();
            }

            return Encoding.UTF8.GetString(json);
        }

        public static T DeserializeJson<T>(string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
                {
                    UseSimpleDictionaryFormat = true
                });
                
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}