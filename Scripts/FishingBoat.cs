using Core;
using Core.Components;
using Core.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDownShooter.Core;

namespace Fishing.Scripts
{
    internal class FishingBoat:IComponent,ITaggable
    {
        public  Sprite boat { get; set; }
        public string name { get; set; } = "fishingBoat";

        private Line Rect { get; set; }

        public Vector2 positionToMoveTo { get; private set; }

        public float moveSpeed { get; private set; } = 13.5f;
        public bool isInDesiredFishingSpot { get;private set; }

        public Line bobber { get; set; }
        private Vector2 bobberRestingPosition { get; set; } = Vector2.Zero;

        private float maxDepth { get; set; } = 90;

        private bool isBobberdescending { get; set; }
        private bool isFishing { get; set; }    
        private bool isMousebuttonPressed { get; set; } 

        public FishingBoat(Vector2 spawnPosition,Vector2 positionToMoveTo)
        {
            boat = new Sprite(spawnPosition, new Vector2(25, 8), Vector2.Zero, "Art/Props/Boat", "boat");
            this.positionToMoveTo = positionToMoveTo;
            Rect = new Line(spawnPosition, new Vector2(25, 8), Color.Red);
            bobber = new Line(Vector2.One,Vector2.One,Color.Red);
        }

        public void LoadContent(ContentManager contentManager)
        {
            boat.LoadContent(Game1.contentManager);
        }

        public void Update(GameTime gameTime)
        {
            
            boat.position = Helper.MoveToward(boat.position,positionToMoveTo,moveSpeed*(float)gameTime.ElapsedGameTime.TotalSeconds);
            if(boat.position== positionToMoveTo&&!isInDesiredFishingSpot){ isInDesiredFishingSpot = true;bobber.position = bobberRestingPosition; }



            boat.Update(gameTime);
            Rect.position = boat.position;
            bobberRestingPosition = boat.position + new Vector2(boat.size.X + 3, -boat.size.Y);


            if (isMousebuttonPressed)
            {
                if (isBobberdescending&&!isFishing)
                {
                    bobber.position += new Vector2(0, (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                if (!isBobberdescending&&!isFishing)
                {
                    bobber.position -= new Vector2(0, (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            if (InputManager.IsMouseButtonPressed(0)&&isInDesiredFishingSpot)
            {
                Console.WriteLine("ok");
                if (isBobberdescending)
                {
                    isFishing = true;

                    isBobberdescending = false;
                    return;
                }
                if (isFishing)
                {
                    isFishing = false;
                    isBobberdescending = false;
                    //isMousebuttonPressed = false;
                    return;
                }
                if (!isBobberdescending && !isFishing)
                {
                    isBobberdescending = true;
                    isMousebuttonPressed=true;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           //Rect.Draw(spriteBatch);
            boat.Draw(spriteBatch);
            if (isInDesiredFishingSpot)
            {
                bobber.Draw(spriteBatch);
            }
        }
    }
}
