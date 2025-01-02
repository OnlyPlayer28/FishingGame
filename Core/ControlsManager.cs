using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class ControlsManager
    {
        public static Dictionary<string, Keys> inputKeys;

        static ControlsManager()
        {
            inputKeys = new Dictionary<string, Keys>();
        }
        
        public static void SetupInputKeys(Dictionary<string, Keys> _inputKeys) 
        {
            inputKeys = _inputKeys;
        }
        public static void SetupInputKeys(string path)
        {
            inputKeys = FileWriter.ReadJson<Dictionary<string,Keys>>(path);
        }
        public static Keys GetInputKey(string action)
        {
            return inputKeys[action];
        }
    }
}
