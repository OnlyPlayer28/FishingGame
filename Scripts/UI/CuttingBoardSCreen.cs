using Core;
using Core.Components;
using Core.Debug;
using Core.InventoryManagement;
using Core.UI;
using Core.UI.Elements;
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
    public class CuttingBoardScreen : IMenu
    {
        private Rect cuttingBoardSprite { get; set; }
        private EmptyUIELement boardCollision { get; set; }
        private Rect backdrop { get; set; }
        private Button[] inventoryButtons { get; set; }
        private Sprite[] inventoryButtonIcons { get; set; }
        private Button[] arrowButtons { get; set; }
        private int currentInventoryPage = 0;

        private Sprite currentIngredientBeingSliced { get; set; }
        public CuttingBoardScreen(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            currentIngredientBeingSliced = new Sprite(position +new Vector2(15),new Rectangle(),"Art/UI/UI","",this.layer);
            arrowButtons = new Button[2];
            inventoryButtons= new Button[5];
            inventoryButtonIcons = new Sprite[5];
            for (int i = 0; i < 5; i++)
            {
                inventoryButtons[i] = new Button(new Sprite(position + new Vector2((i*12)+(i*2), 76), new Vector2(12, 12), new Vector2(56, 17), "Art/UI/UI", layer: layer + .0001001f), onClickSound: "click");
                inventoryButtonIcons[i] = new Sprite(position + new Vector2((i * 12) + (i * 2), 76)+(Vector2.One*2), new Vector2(0,0), new Vector2(0,0), "Art/UI/UI", layer: layer + .0001f);
                inventoryButtons[i].isActive = false;
                canvas.AddClickableElement(inventoryButtons[i]);
                inventoryButtons[i].OnButtonClickEvent += OnSelectButtonsClick;
            }
            arrowButtons[0] = new Button(new Sprite(position + new Vector2(-11, 77), new Vector2(9), new Vector2(47, 17), "Art/UI/UI", "leftArrow", layer + .0001f),false, onClickSound: "click");
            arrowButtons[1] = new Button(new Sprite(position + new Vector2((5*12)+(10), 77), new Vector2(9), new Vector2(47, 17), "Art/UI/UI", "rightArrow", layer + .0001f),false, onClickSound: "click");
            arrowButtons[1].buttonSprite.spriteEffects = SpriteEffects.FlipHorizontally;
            arrowButtons[0].OnButtonClickEvent += OnArrowButtonsClick;
            arrowButtons[1].OnButtonClickEvent += OnArrowButtonsClick;
            canvas.AddClickableElement(arrowButtons[0]);
            canvas.AddClickableElement(arrowButtons[1]);
            cuttingBoardSprite = new Rect(position, size, Helper.HexToRgb("#61748d"), true, layer).SetFillColor(Helper.HexToRgb("#8694ad"));
            boardCollision = new EmptyUIELement(position, size, false, "collision", layer+.00051f);
            backdrop = new Rect(position-new Vector2(10,10),size+new Vector2(20,50),new Color(0,0,0,125),true, layer+.0005f);
            LoadContent(Game1.contentManager);
        }
        public override IMenu SetActive(bool active)
        {
            if (active) { SetButtonIcons(); }
            foreach (var item in inventoryButtons)
            {
                item.isActive = active;

            }
            arrowButtons[0].isActive= active;
            arrowButtons[1].isActive = active;
            boardCollision.isActive= active;
            return base.SetActive(active);
        }

        private void SetButtonIcons()
        {
            for (int i = 0; i < 5; i++)
            {
                int index = i + (currentInventoryPage * 5);
                if (Game1.player.inventory.inventory.Count >index)
                {
                    
                    inventoryButtonIcons[i].SetNewPathAndLocationRect(Game1.player.inventory.inventory[index].Item2.sprite.texturePath, Game1.player.inventory.inventory[index].Item2.sprite.tilemapLocationRect, Game1.contentManager);
                    inventoryButtonIcons[i].scale = Helper.FitSizeIntoBounds(new Vector2(inventoryButtonIcons[i].tilemapLocationRect.Width, inventoryButtonIcons[i].tilemapLocationRect.Height), new Vector2(9, 9)).X;
                    inventoryButtons[i].name = Game1.player.inventory.inventory[index].Item2.ID.ToString();
                }
                else
                {
                    inventoryButtons[i].name = "-1";
                    inventoryButtonIcons[i].scale = 1f;
                    inventoryButtonIcons[i].SetNewPathAndLocationRect("default", new Rectangle(), Game1.contentManager);
                }
            }
        }

        private void OnInventoryModify(Object o,InventoryEventArgs e)
        {
            if (!isActive) { return; }
            SetButtonIcons();
        }

        public void OnArrowButtonsClick(Object o ,ButtonEventArgs e)
        {
            if(e.buttonRef.name == "leftArrow")
            {
               currentInventoryPage= currentInventoryPage == 0 ?(int)MathF.Round(Game1.player.inventory.inventory.Count/5,MidpointRounding.ToZero) : currentInventoryPage - 1;
            }else
            {
                currentInventoryPage = currentInventoryPage == (int)MathF.Round(Game1.player.inventory.inventory.Count / 5, MidpointRounding.ToZero) ? 0 : currentInventoryPage + 1;
            }

            SetButtonIcons();
        }
        public void OnSelectButtonsClick(Object o,ButtonEventArgs  e)
        {
            if(e.buttonRef.name == "-1")
            {
                currentIngredientBeingSliced.SetNewPathAndLocationRect("default", new Rectangle(), Game1.contentManager);
            }
            currentIngredientBeingSliced.SetNewPathAndLocationRect(Game1.GetItem(Convert.ToInt16(e.buttonRef.name)).sprite.texturePath, Game1.GetItem(Convert.ToInt16(e.buttonRef.name)).sprite.tilemapLocationRect, Game1.contentManager);
            currentIngredientBeingSliced.size = new Vector2(currentIngredientBeingSliced.tilemapLocationRect.Width,currentIngredientBeingSliced.tilemapLocationRect.Height);
            currentIngredientBeingSliced.scale = Helper.FitSizeIntoBounds(currentIngredientBeingSliced.size, new Vector2(30, 30),true,3).X;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                cuttingBoardSprite.Draw(spriteBatch);
                currentIngredientBeingSliced.Draw(spriteBatch);
                foreach (var item in inventoryButtonIcons)
                {
                    if (item.texture != null)
                    {
                        item.Draw(spriteBatch);

                    }
                }
               // backdrop.Draw(spriteBatch);
            }
        }
        public override void LoadContent(ContentManager contentManager)
        {
            foreach (var item in inventoryButtons)
            {
                item.LoadContent(contentManager);
            }
            currentIngredientBeingSliced.LoadContent(contentManager);
            arrowButtons[0].LoadContent(contentManager) ;
            arrowButtons[1].LoadContent(contentManager);
        }
    }
}
