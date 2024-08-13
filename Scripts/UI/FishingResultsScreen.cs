using Core;
using Core.Components;
using Core.Debug;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI
{
    public class FishingResultsScreen : IMenu
    {
        
        private Rect backgroundRect { get; set; }
        private float _layerRef { get; set; }
        private int fishID { get; set; }
        private Sprite fishCaughtIcon { get; set; }
        public FishingResultsScreen(Vector2 position,Vector2 size,float layer,int fishID)
            :base(position,size,layer)
        {
            this.fishID = fishID;
            _layerRef = layer;
            backgroundRect = new Rect(position-Vector2.One, size+new Vector2(2,2), new Color(0, 0, 0, 125), true, layer+.0001f);
            fishCaughtIcon = Game1.GetItem(this.fishID).sprite;
            fishCaughtIcon.layer = layer;

            LoadContent(Game1.contentManager);
        }

        public void SetFish(int ID)
        {
            this.fishID = ID;
        }
        public override void LoadContent(ContentManager contentManager)
        {
            Console.WriteLine("loaded");
            fishCaughtIcon.LoadContent(contentManager);
        }
        public override  void OnMouseOver(object sender, EventArgs  e)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isActive)
            {
                fishCaughtIcon?.Draw(spriteBatch);
                backgroundRect.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
        }

        public override IMenu SetActive(bool active)
        {
            //Disable Text !!!!!!!

            return base.SetActive(active);
        }
    }
}
