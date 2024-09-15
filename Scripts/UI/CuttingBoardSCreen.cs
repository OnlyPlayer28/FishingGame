using Core;
using Core.Components;
using Core.Debug;
using Core.InventoryManagement;
using Core.UI;
using Core.UI.Elements;
using Fishing.Scripts.Crafting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Rect[] cuttingRectangles { get; set; }

        private Sprite currentIngredientBeingSliced { get; set; }
        private List<CraftingRecipe> cuttableFoodRecipes { get; set; }

        private int cuttingRectWidth = 2;
        private int currentCuttingRectangle = -1;
        private float cuttingStartPosition = 0;
        private int? cuttingDirection = null;
        private bool isCuttingVertically = true;

        public CuttingBoardScreen(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            cuttableFoodRecipes = FileWriter.ReadJson<List<CraftingRecipe>>(Game1.contentManager.RootDirectory + "/Data/Crafting/cuttableFoodCrafting.json");
            InputManager.OnMouseDownEvent += OnMouseHold;
            Game1.player.inventory.OnInventoryModifyEvent += OnInventoryModify;

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

        private void OnMouseHold(Object o,MouseInputEventArgs e)
        {
            int lastRect = currentCuttingRectangle;

            if (!isActive&&e.mouseButton == MouseButton.Right) { return; }
            if(cuttingRectangles!= null &&currentCuttingRectangle!= -1&&(isCuttingVertically?
                cuttingRectangles[currentCuttingRectangle].position.Y+ cuttingRectangles[currentCuttingRectangle].size.Y-1: 
                cuttingRectangles[currentCuttingRectangle].position.X+ cuttingRectangles[currentCuttingRectangle].size.X-1) 
                <= (isCuttingVertically ? InputManager.GetMousePosition().Y: InputManager.GetMousePosition().X))
            {
                cuttingRectangles[currentCuttingRectangle] = null;
            }
            if (cuttingRectangles != null && cuttingRectangles.Any(p => p != null&&p.GetCollision().Intersects(InputManager.GetMouseRect())))
            {

                currentCuttingRectangle = cuttingRectangles.ToList().IndexOf(cuttingRectangles.Where(p =>p!=null&& p.GetCollision().Intersects(InputManager.GetMouseRect())).First());
                if(lastRect == -1 && !new Rectangle( (int)cuttingRectangles[currentCuttingRectangle].position.X, (int)cuttingRectangles[currentCuttingRectangle].position.Y, cuttingRectWidth, cuttingRectWidth).Intersects(InputManager.GetMouseRect()))
                {
                    Debug.WriteLine("failed cutting(didnt start at proper position):(");
                    currentCuttingRectangle = -1;
                }
                if (lastRect != currentCuttingRectangle)
                {
                    isCuttingVertically = cuttingRectangles[currentCuttingRectangle].size.X == cuttingRectWidth ? true : false;
                    cuttingStartPosition = isCuttingVertically? InputManager.GetMousePosition().Y : InputManager.GetMousePosition().X;
                    Debug.WriteLine("changed cutting rect");
                }

            }else if(currentCuttingRectangle != -1&& (cuttingRectangles.All(p => p!= null&&!p.GetCollision().Intersects(InputManager.GetMouseRect()))))
            {
                Debug.WriteLine("failed cutting :(");
                currentCuttingRectangle = -1;
            }
            if (currentCuttingRectangle != -1 && cuttingRectangles.All(p=>p is null)) 
            { 
                Debug.WriteLine("okay");
                GiveItemAfterCutting(cuttableFoodRecipes.Where(p => p.input.ContainsKey(Convert.ToInt16(currentIngredientBeingSliced.name))).First().name);
                currentCuttingRectangle = -1; 
            }
            else if(currentCuttingRectangle != -1 && cuttingRectangles[currentCuttingRectangle] == null) 
            { 
                currentCuttingRectangle = -1;
            }
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
        private void GiveItemAfterCutting(string recipe)
        {
            cuttableFoodRecipes.Where(p => p.name == recipe).First().Craft(Game1.player.inventory);
        }
        private void SetButtonIcons()
        {
            for (int i = 0; i < 5; i++)
            {
                int index = i + (currentInventoryPage * 5);
                if (Game1.player.inventory.inventory.Where(x=>cuttableFoodRecipes.Any(p=>p.input.ContainsKey(x.Item2.ID))).Count() > index && Game1.player.inventory.inventory[index].Item1>0)
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

        private void SetupCuttingRectangles()
        {
            
            bool isXLargerThanY = currentIngredientBeingSliced.size.X > currentIngredientBeingSliced.size.Y ?true:false;
            Vector2 startingPos = currentIngredientBeingSliced.position;
            int numberOfRects = 0;
            float sizeToOperateWith = isXLargerThanY ? currentIngredientBeingSliced.tilemapLocationRect.Width * currentIngredientBeingSliced.scale : currentIngredientBeingSliced.tilemapLocationRect.Height * currentIngredientBeingSliced.scale;
            for (int i = 0; i < 5; i++)
            {
                if(sizeToOperateWith/2 >= 3)
                {
                    sizeToOperateWith /= 2;
                    numberOfRects++;
                }else
                {
                    break;
                }
            }
            cuttingRectangles = new Rect[numberOfRects];
            for (int i = 1; i < numberOfRects+1; i++)
            {
                if (isXLargerThanY)
                {
                    cuttingRectangles[i-1] = new Rect(startingPos + new Vector2((i * 3*currentIngredientBeingSliced.scale)/*-(2*i)*/,0), new Vector2(cuttingRectWidth, currentIngredientBeingSliced.tilemapLocationRect.Height*currentIngredientBeingSliced.scale), Color.Red, true);
                }else
                {
                    cuttingRectangles[i-1] = new Rect(startingPos + new Vector2(0,(i * 3*currentIngredientBeingSliced.scale) /*- (2 * i)*/), new Vector2( currentIngredientBeingSliced.tilemapLocationRect.Width * currentIngredientBeingSliced.scale, cuttingRectWidth), Color.Red, true);
                }
            }

        }

        private void OnInventoryModify(Object o,InventoryEventArgs e)
        {
            if (!isActive) { return; }
            currentIngredientBeingSliced.SetNewPathAndLocationRect("default", new Rectangle(), Game1.contentManager);
            SetButtonIcons();
        }

        public void OnArrowButtonsClick(Object o ,ButtonEventArgs e)
        {
            currentIngredientBeingSliced.SetNewPathAndLocationRect("default", new Rectangle(), Game1.contentManager);
            cuttingRectangles =cuttingRectangles != null&&cuttingRectangles.Count() != 0 ? cuttingRectangles = new Rect[] {} : cuttingRectangles;
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
            cuttingRectangles =cuttingRectangles!=null&& cuttingRectangles.Count() != 0 ? cuttingRectangles = new Rect[] { } : cuttingRectangles;
            if (e.buttonRef.name == "-1")
            {
                currentIngredientBeingSliced.SetNewPathAndLocationRect("default", new Rectangle(), Game1.contentManager);
            }
            else
            {
                currentIngredientBeingSliced.name = e.buttonRef.name;
                currentIngredientBeingSliced.SetNewPathAndLocationRect(Game1.GetItem(Convert.ToInt16(e.buttonRef.name)).sprite.texturePath, Game1.GetItem(Convert.ToInt16(e.buttonRef.name)).sprite.tilemapLocationRect, Game1.contentManager);
                currentIngredientBeingSliced.size = new Vector2(currentIngredientBeingSliced.tilemapLocationRect.Width, currentIngredientBeingSliced.tilemapLocationRect.Height);
                currentIngredientBeingSliced.scale = Helper.FitSizeIntoBounds(currentIngredientBeingSliced.size, new Vector2(30, 30), true, 3).X;
                currentIngredientBeingSliced.layer = 0;
                SetupCuttingRectangles();
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                cuttingBoardSprite.Draw(spriteBatch);
                currentIngredientBeingSliced.Draw(spriteBatch);
                if (cuttingRectangles != null)
                {
                    foreach (var item in cuttingRectangles)
                    {
                        item?.Draw(spriteBatch);
                    }
                }
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
