
using Newtonsoft.Json;
using System.IO;

namespace Fishing.Core
{
    public  class FileWriter
    {
        public static void WriteJson<T>(T objectToSerialize,string path)
        {
            string textOutput = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);
            File.WriteAllText(path,textOutput);
        }
    }
}
