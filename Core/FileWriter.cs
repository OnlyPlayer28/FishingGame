
using Newtonsoft.Json;
using System.IO;

namespace Core
{
    public  class FileWriter
    {
        public static void WriteJson<T>(T objectToSerialize,string path)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, };
            string textOutput = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented,settings);
            File.WriteAllText(path,textOutput);
        }

        public static T ReadJson<T>(string path)
        {

            JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            string text  =File.ReadAllText(path);
            System.Diagnostics.Debug.WriteLine(path);
            return JsonConvert.DeserializeObject<T>(text,settings);
        }
    }
}
