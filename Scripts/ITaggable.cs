using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts
{
    [Flags]
    public enum Tags
    {
        None = 0,
        AddableToMenu = 1<<0,
        DisplayableOnCounter = 1<<1

    }
    public interface ITaggable
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Tags tags { get; set; }
    }
}
