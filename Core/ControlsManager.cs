using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class ControlsManager
    {
        public static Dictionary<string,Keys> inputKeys = new Dictionary<string,Keys>();

        public static void SetupInputKeys(Dictionary<string, Keys> _inputKeys) 
        {
            inputKeys = _inputKeys;
        }
        public static Keys GetInputKey(string action)
        {
            return inputKeys[action];
        }
    }
}
