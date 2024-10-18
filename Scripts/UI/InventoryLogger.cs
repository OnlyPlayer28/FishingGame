using Core.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI
{
    internal class InventoryLogger
    {
        private Text logText;
        private string loggedText;
        private Vector2 loggerTextStartPosition;
        private float textSpacing;

        private float timeUntilNextTextRemove = 0f;
        private float textRemovalInterval = 3f;

        internal delegate string stringFormatter(string text = "",int num = 0);

        private EventHandler OnTextModification;
        public InventoryLogger(float textSpacing, float textRemovalInterval)
        {
            this.textSpacing = textSpacing;
            this.timeUntilNextTextRemove = textRemovalInterval;
            this.textRemovalInterval = textRemovalInterval;
        }

        public void AddTextToStack(stringFormatter stringFormatter)
        {
            loggedText += (loggedText.Length>0?"/n":"") +stringFormatter;
        }

         
        public void Update(GameTime gameTime)
        {
            if (loggedText.Length > 0)
            {
                timeUntilNextTextRemove-=(float)gameTime.ElapsedGameTime.TotalSeconds;
                if(timeUntilNextTextRemove <= 0)
                {
                    //loggedText-=loggedText.l
                }
            }
        }
        public string ItemFormatter(string  text ="",int num = 0)
        {
            return (num>=0?"+":"")+$"{num}: {text}";
        }
    }
}