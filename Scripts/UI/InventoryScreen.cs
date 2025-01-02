using Core;
using Core.Audio;
using Core.Components;
using Core.Debug;
using Core.InventoryManagement;
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
    public class InventoryScreen : IMenu
    {

        private Rect background { get; set; }
        private Rect descriptionBackground { get; set; }
        public List<Sprite> icons { get; set; }
        public List<Text> itemAmountsText { get; set; }
        private EmptyUIELement UICollision { get; set; }

        private Text itemDescriptionText { get; set; }

        private int verticalSpacing = 5;
        private int textHorizontalSpacing = 2;
        private int textVerticalSpacing = 1;
        private float ySizeFirstColumn = 2;
        private float ySizeSecondColumn = 2;
        public InventoryScreen(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            descriptionBackground = new Rect(Vector2.One,new Vector2(0,0), new Color(0, 0, 0, 125), true, layer ).SetFillColor(new Color(0,0,0,125));
            itemDescriptionText = new Text(Vector2.One, "", Color.Wheat, Game1.Font_24);
            icons  = new List<Sprite>();
            itemAmountsText = new List<Text>();
            background = new Rect(this.position-new Vector2(2,0) , this.size+new Vector2(3,2), Helper.HexToRgb("#542424"), true, layer + .0005f).SetFillColor(Helper.HexToRgb("#6e3b34"));
            UICollision = new EmptyUIELement(this.position - new Vector2(2, 0), this.size + new Vector2(3, 2), false, "collsion", 0);
            Game1.player.inventory.OnInventoryModifyEvent += SetIcons;
            canvas.AddUIELement(UICollision);
            AddTextEleemnt(itemDescriptionText);
            
        }

        public override void LoadContent(ContentManager contentManager)
        {

            base.LoadContent(contentManager);
        }

        public override IActive SetActive(bool active)
        {
            AudioManager.PlayCue("fridge_opening_and_closing");
            foreach (var item in itemAmountsText)
            {
                item.isActive = active;
            }
            UICollision.isActive = active;
            itemDescriptionText.isActive = active;
            return base.SetActive(active);
        }
        public Vector2 CalculatePosition(Vector2 objectSize)
        {
            Vector2 positionToReturn = this.position;
            if (ySizeFirstColumn + objectSize.Y  > size.Y)
            {
                positionToReturn+= new Vector2(size.X / 2, ySizeSecondColumn);
                ySizeSecondColumn += objectSize.Y + verticalSpacing;
                return positionToReturn;

            }
            else
            {
                positionToReturn+= new Vector2(0, ySizeFirstColumn);
                ySizeFirstColumn+= objectSize.Y+verticalSpacing;
                return positionToReturn;
            }

        }
        public void SetIcons(Object o,InventoryEventArgs e)
        {
            int? removedIndex = null;
            if (e.totalItemAmount <= 0&&icons.Count >0)
            {

                removedIndex = icons.IndexOf(icons.Where(p => p.name == e.ID.ToString()).First());
                if (icons[(int)removedIndex].position.X == (size.X / 2) + this.position.X)
                {
                    ySizeSecondColumn = icons[(int)removedIndex].position.Y - this.position.Y;
                }
                else
                {
                    ySizeFirstColumn =icons[(int)removedIndex].position.Y-this.position.Y;
                }
                icons.RemoveAt((int)removedIndex);
                RemoveTextElement(itemAmountsText[(int)removedIndex]);
                itemAmountsText.RemoveAt((int)removedIndex);

            }
            if(icons.Any(p=>p.name == e.ID.ToString())) 
            { 
                 itemAmountsText[icons.IndexOf(icons.Where(p=>p.name ==e.ID.ToString()).First())].text = e.totalItemAmount.ToString();
            }
            if(removedIndex != null)
            {
                
                Span<Sprite> iconSpan = icons.Take(new Range((int)removedIndex,icons.Count)).ToArray();
                foreach(Sprite icon in iconSpan)
                {
                    icon.position = CalculatePosition(icon.size);
                    itemAmountsText[icons.IndexOf(icon)].setPosition(icon.position+new Vector2(icon.size.X+textHorizontalSpacing,textVerticalSpacing));
                }
            }
            if(icons.All(p=>p.name != e.ID.ToString())&&e.totalItemAmount >0)
            {
                Sprite spriteToAdd = ((IAddableToInventory)Game1.GetItem(e.ID).Clone()).sprite;
                spriteToAdd.position = CalculatePosition(spriteToAdd.size);
                spriteToAdd.LoadContent(Game1.contentManager);
                spriteToAdd.layer = layer + .0001f;
                spriteToAdd.name = e.ID.ToString();
                icons.Add(spriteToAdd);

                itemAmountsText.Add(new Text(spriteToAdd.position+new Vector2(spriteToAdd.size.X+textHorizontalSpacing,textVerticalSpacing), e.totalItemAmount.ToString(), Color.White, Game1.Font_24, layer: this.layer+.0001f));
                itemAmountsText.Last().isActive = isActive;
                AddTextEleemnt(itemAmountsText.Last());
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                if (itemDescriptionText.isActive)
                {
                    descriptionBackground.Draw(spriteBatch);
                }
                icons.ForEach(p => p.Draw(spriteBatch));
                background.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (icons.Any(p => InputManager.GetMouseRect().Intersects(Helper.GetRectFromVector2(p.position, p.size))))
                {
                    try
                    {
                        Sprite icon = icons.Where(p => InputManager.GetMouseRect().Intersects(Helper.GetRectFromVector2(p.position, p.size))).FirstOrDefault();
                        itemAmountsText[icons.IndexOf(icon)].isActive = false;
                        itemDescriptionText.text = Game1.GetItem(Convert.ToInt16(icon.name)).name;
                        itemDescriptionText.setPosition(icon.position + new Vector2(icon.size.X + .5f, 0));
                        itemDescriptionText.isActive = true;
                        descriptionBackground.position = icon.position + new Vector2(icon.size.X + .5f, 0);
                        descriptionBackground.SetSize(new Vector2(2.39f * itemDescriptionText.text.Length, 8));
                    }
                    catch (Exception)
                    {

                    }

                }
                else
                {
                    if (itemDescriptionText.isActive)
                    {
                        itemAmountsText.ForEach(p => p.isActive = true);
                        itemDescriptionText.isActive = false;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
