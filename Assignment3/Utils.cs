#nullable enable
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Assignment3
{
    public class Utils
    {
        public static string ToJson(object data)
        {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static T? FromJson<T>(string element)
        {
            return JsonSerializer.Deserialize<T>(element, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
        
        public static bool ValidateJson(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
        
        public static Request? ReadRequest(NetworkStream clientStream)
        {
            using var stream = new MemoryStream();
            byte[] buffer = new byte[2048];
            int bytesRead = clientStream.Read(buffer, 0, buffer.Length);

            stream.Write(buffer, 0, bytesRead);
            while(bytesRead == 2048)
            {
                stream.Write(buffer, 0, bytesRead);
                bytesRead = clientStream.Read(buffer, 0, buffer.Length);
            }

            var responseData = Encoding.UTF8.GetString(stream.ToArray());
            return responseData != "" ? FromJson<Request>(responseData) : null;
        }
    }
}