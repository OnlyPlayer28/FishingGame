using Core;
using Core.Components;
using Core.Debug;
using Core.UI;
using Core.UI.Elements;
using Fishing.Scripts.Scenes;
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

        private Button closeScreenButton { get; set; }
        private Text fishNameText { get; set; }
        public FishingResultsScreen(Vector2 position,Vector2 size,float layer,int fishID,Canvas canvas)
            :base(position,size,layer,canvas)
        {
            this.fishID = fishID;
            _layerRef = layer;
            backgroundRect = new Rect(position-Vector2.One, size+new Vector2(2,2), Helper.HexToRgb("#542424"), true, layer+.0005f).SetFillColor(Helper.HexToRgb("#6e3b34"));
            fishCaughtIcon = Game1.GetItem(this.fishID).sprite;
            fishCaughtIcon.position = this.position + new Vector2(2, 2);
            closeScreenButton = new Button(this.position + this.size - new Vector2(11, 11), new Vector2(10, 10), layer + .0004f,isActive:false)
                .SetButtonSprite(new Sprite(this.position + this.size - new Vector2(11, 11), new Vector2(10, 10), new Vector2(28, 9), "Art/UI/UI", layer: layer + .0004f), Game1.contentManager)
                .SetOnButtonCLickAction(OnCLoseButtonCLick);

            fishNameText = new Text(this.position, "test name", Color.White, Game1.Font_24, layer: this.layer + .0001f);
            fishCaughtIcon.layer = layer;
            AddTextEleemnt(fishNameText);
            canvas.AddClickableElement(closeScreenButton);
            LoadContent(Game1.contentManager);
        }
        public void OnCLoseButtonCLick()
        {
            SetActive(false);
        }
        public FishingResultsScreen SetFish(int ID)
        {
            this.fishID = ID;
            fishNameText.text = Game1.GetItem(ID).name;
            fishCaughtIcon = Game1.GetItem(this.fishID).sprite;
            fishCaughtIcon.position = this.position + new Vector2(2, 2);
            fishNameText.setPosition(new Vector2(fishCaughtIcon.size.X +0, 0) + position);
            fishCaughtIcon.LoadContent(Game1.contentManager);

            fishCaughtIcon.scale = .75f;
            return this;
        }
        public override void LoadContent(ContentManager contentManager)
        {
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
            if (!active)
            {
                ((FishingScene)Game1.stateManager.GetActiveGameState()).boat.fishingState = FishingState.WaitingForFish;
                Game1.player.inventory.AddItem(Game1.GetItem(fishID));
            }
            else
            {
                ((FishingScene)Game1.stateManager.GetActiveGameState()).boat.fishingState = FishingState.FishingResults;
            }
            fishNameText.isActive = active;
            closeScreenButton.isActive = active;
            return base.SetActive(active);
        }
    }
}
