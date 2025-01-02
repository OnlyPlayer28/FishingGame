using Core;
using Core.Components;
using Core.Debug;
using Core.UI;
using Core.UI.Elements;
using Fishing.Scripts.Restaurant;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts.UI
{
    public class MenuItemContainer : IMenu
    {
        private Image outline;
        private Button dishNameButton;
        private Text priceText;
        public MenuItem? menuItem { get;private set; }
        public Action openItemSelectionAction { get;  set; }
        public EventHandler OnSelectionScreenOpened { get; set; }
        public MenuItemContainer(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            outline = new Image(new Rect(position,size,(Helper.HexToRgb("#bb8c6e")),layer:layer),true,false);
            dishNameButton = new Button(position + Vector2.One, new Vector2(40,10),layer,"modifyItemButton",false,"click").SetSimpleSprite(new Color(0,0,0,25),Color.Transparent).SetButtonText("default item",Color.Wheat,Game1.Font_24);
            priceText = new Text(position+new Vector2(size.X-20,1),"price:999",Color.Wheat,Game1.Font_24, layer:layer);

            canvas.AddClickableElement(dishNameButton);
            canvas.AddTextElement(priceText);
            //canvas.AddUIELement(outline);

            dishNameButton.OnButtonClickEvent += OnButtonClick;
            //SetActive(false);
        }
        public void OnButtonClick(Object o, ButtonEventArgs e)
        {
            if (!isActive) { return; }
            if(e.buttonRef.name == "modifyItemButton")
            {
                openItemSelectionAction();
                OnSelectionScreenOpened?.Invoke(this, EventArgs.Empty);
            }
        }
        public void SetMenuItem(MenuItem menuItem)
        {
            this.menuItem = menuItem;
            dishNameButton.buttonText.text = Game1.GetItem(menuItem.ID).name;
            priceText.text =/*"price:"+*/ menuItem.price.ToString();
            outline.isActive = false;
        }
        public void RemoveMenuItem()
        {
            menuItem = null;
            dishNameButton.buttonText.text = default;
        }

        public override IActive SetActive(bool active)
        {
            dishNameButton.isActive= active;
            priceText.isActive= active;
            outline.isActive = active;

            return base.SetActive(active);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
